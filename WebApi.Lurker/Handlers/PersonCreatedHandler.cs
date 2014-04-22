using System.Data.SqlClient;
using WebApi.Data.People.Events;

namespace WebApi.Lurker.Handlers
{
	class PersonCreatedHandler : NServiceBus.IHandleMessages<PersonCreated>
	{
		public void Handle( PersonCreated message )
		{
			using ( var connection = new SqlConnection( @"Data Source=.\SqlExpress;Initial Catalog=ImplementingCQRS;Integrated Security=True" ) ) 
			{
				using ( var cmd = new SqlCommand( "INSERT INTO [dbo].[Persons] ([FirstName] ,[LastName] ,[ExternalId]) VALUES (@FirstName, @LastName, @ExternalId)"))
				{
					cmd.Parameters.Add( "@FirstName", message.FirstName );
					cmd.Parameters.Add( "@LastName", message.LastName );
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
