using System.Data.SqlClient;
using WebApi.Data.People.Events;

namespace WebApi.Lurker.Handlers
{
	class PersonDenormalizerHandler : NServiceBus.IHandleMessages<PersonCreated>
	{
		public void Handle( PersonCreated message )
		{
			using ( var connection = new SqlConnection( @"Data Source=.\SqlExpress;Initial Catalog=ImplementingCQRS;Integrated Security=True" ) )
			{
				using ( var cmd = new SqlCommand( "INSERT INTO [dbo].[Search] ([DisplayName] ,[Collection] ,[ExternalId]) VALUES (@DisplayName, @Collection, @ExternalId)" ) )
				{
					cmd.Parameters.Add( "@DisplayName", ( message.FirstName + " " + message.LastName ) );
					cmd.Parameters.Add( "@Collection", "Persons" );
					cmd.Parameters.Add( "@ExternalId", message.AggregateId );

					cmd.Connection = connection;

					connection.Open();

					cmd.ExecuteNonQuery();

					connection.Close();
				}
			}
		}
	}
}
