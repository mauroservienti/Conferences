using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NServiceBus.MessageInterfaces;

namespace NServiceBus.Serialization
{
	public class SampleMessageContractResolver : DefaultContractResolver
	{
		private readonly IMessageMapper _messageMapper;

		public SampleMessageContractResolver( IMessageMapper messageMapper )
            : base(true)
        {
            _messageMapper = messageMapper;
        }

        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            Type mappedTypeFor = _messageMapper.GetMappedTypeFor(objectType);

            if (mappedTypeFor == null)
                return base.CreateObjectContract(objectType);

            var jsonContract = base.CreateObjectContract(mappedTypeFor);
            jsonContract.DefaultCreator = () => _messageMapper.CreateInstance(mappedTypeFor);

            return jsonContract;
        }

		protected override JsonProperty CreateProperty( MemberInfo member, MemberSerialization memberSerialization )
		{
			var prop = base.CreateProperty( member, memberSerialization );

			if ( !prop.Writable )
			{
				var property = member as PropertyInfo;
				if ( property != null )
				{
					var hasPrivateSetter = property.GetSetMethod( true ) != null;
					prop.Writable = hasPrivateSetter;
				}
			}

			return prop;
		}
	}
}
