using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.MessageInterfaces.MessageMapper.Reflection;
using NServiceBus.Settings;

namespace NServiceBus.Serialization
{
	public class SampleJsonSerialization : NServiceBus.Features.Feature<NServiceBus.Features.Categories.Serializers>
	{
		public override void Initialize()
		{
			Configure.Component<MessageMapper>( DependencyLifecycle.SingleInstance );
			Configure.Component<SampleJsonMessageSerializer>( DependencyLifecycle.SingleInstance )
				 .ConfigureProperty( s => s.SkipArrayWrappingForSingleMessages, !SettingsHolder.GetOrDefault<bool>( "SerializationSettings.WrapSingleMessages" ) );
				 //.ConfigureProperty( s => s.IgnorePrivateSetters, SettingsHolder.GetOrDefault<bool>( "SerializationSettings.IgnorePrivateSetters" ) );
		}
	}
}
