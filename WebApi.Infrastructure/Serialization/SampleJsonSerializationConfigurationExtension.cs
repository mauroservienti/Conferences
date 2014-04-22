using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Features;
using NServiceBus.Settings;

namespace NServiceBus.Serialization
{
	public static class SampleJsonSerializationConfigurationExtension
	{
		/// <summary>
		/// Enables the json message serializer with support for private setters
		/// </summary>
		/// <param name="settings"></param>
		/// <returns></returns>
		public static SerializationSettings AdvancedJson( this SerializationSettings settings )
		{
			Feature.Enable<SampleJsonSerialization>();

			return settings;
		}
	}
}
