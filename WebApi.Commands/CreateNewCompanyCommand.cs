using NServiceBus;

namespace WebApi.Commands
{
	public class CreateNewCompanyCommand : ICommand
	{
		public string CompanyName { get; set; }
	}
}
