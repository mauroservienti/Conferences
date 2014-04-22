using System;
using Raven.Imports.Newtonsoft.Json;

namespace WebApi.Data.Requests
{
	public class CommandResponse
	{
		[JsonConstructor]
		private CommandResponse()
		{

		}

		public CommandResponse( String correlationId, Object rawResponse, String userAccount )
		{
			this.CreatedOn = DateTimeOffset.Now;
			this.CorrelationId = correlationId;
			this.Response = rawResponse;
			this.UserAccount = userAccount;
		}

		public String Id { get; private set; }
		public DateTimeOffset CreatedOn { get; private set; }
		public String CorrelationId { get; private set; }

		public Object Response { get; private set; }

		public String UserAccount { get; private set; }
	}
}
