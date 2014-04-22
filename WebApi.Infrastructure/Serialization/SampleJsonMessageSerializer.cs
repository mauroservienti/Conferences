using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NServiceBus;
using NServiceBus.MessageInterfaces;
using NServiceBus.Serialization;
using NServiceBus.Serializers.Json.Internal;

namespace NServiceBus.Serialization
{
	public class SampleJsonMessageSerializer : IMessageSerializer

	{
		private readonly IMessageMapper messageMapper;

		readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
		{
			TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
			TypeNameHandling = TypeNameHandling.Objects,
			Converters = { new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.RoundtripKind }, new SampleXContainerConverter() }
		};

		public SampleJsonMessageSerializer( IMessageMapper messageMapper )
		{
			this.messageMapper = messageMapper;
		}

		/// <summary>
		/// Removes the wrapping array if serializing a single message 
		/// </summary>
		public bool SkipArrayWrappingForSingleMessages { get; set; }

		/// <summary>
		/// Serializes the given set of messages into the given stream.
		/// </summary>
		/// <param name="messages">Messages to serialize.</param>
		/// <param name="stream">Stream for <paramref name="messages"/> to be serialized into.</param>
		public void Serialize( object[] messages, Stream stream )
		{
			var jsonSerializer = JsonSerializer.Create( serializerSettings );
			jsonSerializer.Binder = new MessageSerializationBinder( messageMapper );

			var jsonWriter = CreateJsonWriter( stream );

			if ( SkipArrayWrappingForSingleMessages && messages.Length == 1 )
				jsonSerializer.Serialize( jsonWriter, messages[ 0 ] );
			else
				jsonSerializer.Serialize( jsonWriter, messages );

			jsonWriter.Flush();
		}

		/// <summary>
		/// Deserializes from the given stream a set of messages.
		/// </summary>
		/// <param name="stream">Stream that contains messages.</param>
		/// <param name="messageTypes">The list of message types to deserialize. If null the types must be inferred from the serialized data.</param>
		/// <returns>Deserialized messages.</returns>
		public object[] Deserialize( Stream stream, IList<Type> messageTypes = null )
		{
			var settings = serializerSettings;

			var dynamicTypeToSerializeTo = messageTypes != null ? messageTypes.FirstOrDefault( t => t.IsInterface ) : null;
			if ( dynamicTypeToSerializeTo != null )
			{
				settings = new JsonSerializerSettings
				{
					TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
					TypeNameHandling = TypeNameHandling.Objects,
					Converters = { new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.RoundtripKind }, new SampleXContainerConverter() }
				};
			}

			JsonSerializer jsonSerializer = JsonSerializer.Create( settings );
			jsonSerializer.ContractResolver = new SampleMessageContractResolver( messageMapper );

			var reader = CreateJsonReader( stream );
			reader.Read();

			var firstTokenType = reader.TokenType;

			if ( firstTokenType == JsonToken.StartArray )
			{
				if ( dynamicTypeToSerializeTo != null )
				{
					return ( object[] )jsonSerializer.Deserialize( reader, dynamicTypeToSerializeTo.MakeArrayType() );
				}
				return jsonSerializer.Deserialize<object[]>( reader );
			}
			if ( messageTypes != null && messageTypes.Any() )
			{
				return new[] { jsonSerializer.Deserialize( reader, messageTypes.First() ) };
			}

			return new[] { jsonSerializer.Deserialize<object>( reader ) };
		}

		/// <summary>
		/// Gets the content type into which this serializer serializes the content to 
		/// </summary>
		public string ContentType { get { return GetContentType(); } }

		protected string GetContentType()
		{
			return ContentTypes.Json;
		}

		protected JsonWriter CreateJsonWriter( Stream stream )
		{
			var streamWriter = new StreamWriter( stream, Encoding.UTF8 );
			return new JsonTextWriter( streamWriter ) { Formatting = Formatting.None };
		}

		protected JsonReader CreateJsonReader( Stream stream )
		{
			var streamReader = new StreamReader( stream, Encoding.UTF8 );
			return new JsonTextReader( streamReader );
		}
	}
}
