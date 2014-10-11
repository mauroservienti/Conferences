
namespace NSB03Features
{
	using NServiceBus;
	using NServiceBus.Features;

	/*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/the-nservicebus-host
	*/
	public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
	{
		public void Customize( BusConfiguration configuration )
		{
			configuration.UsePersistence<InMemoryPersistence>();
			configuration.EnableFeature<MyAmazingFeature>();
		}
	}

	class MyAmazingFeature : Feature
	{
		protected override void Setup( FeatureConfigurationContext context )
		{
			//context.Container;
			//context.Pipeline;
			//context.Settings;
		}
	}

}
