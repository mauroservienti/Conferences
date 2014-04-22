using System;
using Raven.Imports.Newtonsoft.Json;

namespace WebApi.Data.Requests
{
	public class CommandException
	{
		[JsonConstructor]
		private CommandException()
		{

		}

		public CommandException( String correlationId, Object rawCommand, String userAccount, Exception error )
		{
			this.CreatedOn = DateTimeOffset.Now;
			this.CorrelationId = correlationId;
			this.Command = rawCommand;
			this.UserAccount = userAccount;
			this.Error = error;
		}

		public String Id { get; private set; }
		public DateTimeOffset CreatedOn { get; private set; }
		public String CorrelationId { get; private set; }

		public Object Command { get; private set; }

		public String UserAccount { get; private set; }

		public Exception Error { get; private set; }
	}
}
