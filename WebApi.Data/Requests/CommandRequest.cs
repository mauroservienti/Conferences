using System;
using Raven.Imports.Newtonsoft.Json;

namespace WebApi.Data.Requests
{
	public class CommandRequest
	{
		[JsonConstructor]
		private CommandRequest()
		{

		}

		public CommandRequest( String correlationId, Object rawCommand, String userAccount )
		{
			this.CreatedOn = DateTimeOffset.Now;
			this.CorrelationId = correlationId;
			this.Command = rawCommand;
			this.UserAccount = userAccount;
		}

		public String Id { get; private set; }
		public DateTimeOffset CreatedOn { get; private set; }
		public String CorrelationId { get; private set; }

		public Object Command { get; private set; }

		public String UserAccount { get; private set; }
	}
}
