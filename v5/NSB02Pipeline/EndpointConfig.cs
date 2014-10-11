
namespace NSB02Pipeline
{
	using NServiceBus;
	using NServiceBus.Pipeline;
	using NServiceBus.Pipeline.Contexts;

    /*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/the-nservicebus-host
	*/
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UsePersistence<InMemoryPersistence>();
			configuration.Pipeline.Register( "MyStep", typeof( MyBehavior ), "behavior description" );
			configuration.Pipeline.Register<MyOtherBehaviorSetup>();
        }
    }

    class MyOtherBehaviorSetup : RegisterStep
    {
        public MyOtherBehaviorSetup()
            : base( "MyOtherBehavior", typeof( MyOtherBehavior ), "description here..." )
        {
            InsertAfter( WellKnownStep.DeserializeMessages );
        }
    }


	class MyBehavior : IBehavior<IncomingContext>
	{
		public void Invoke( IncomingContext context, System.Action next )
		{
			//can do something before moving on

			//context.IncomingLogicalMessage;
			//context.LogicalMessages;
			//context.MessageHandler;
			//context.PhysicalMessage;
			//context.Builder;
			//context.DoNotInvokeAnyMoreHandlers();

			next();

			//can do something after the message has been handled
		}
	}

	
	class MyOtherBehavior : IBehavior<IncomingContext>
	{
		public void Invoke( IncomingContext context, System.Action next )
		{
			//can do something before moving on

			next();

			//can do something after the message has been handled
		}
	}

}
