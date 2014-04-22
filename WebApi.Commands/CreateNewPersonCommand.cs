using System.ComponentModel.DataAnnotations;
using NServiceBus;

namespace WebApi.Commands
{
	public class CreateNewPersonCommand : ICommand
	{
		[Required]
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}
