using System.Data.SqlClient;
using WebApi.Data.Companies.Events;

namespace WebApi.Lurker.Handlers
{
	class CompanyDenormalizerHandler : NServiceBus.IHandleMessages<CompanyCreated>
	{
		public void Handle( CompanyCreated message )
		{
			using ( var connection = new SqlConnection( @"Data Source=.\SqlExpress;Initial Catalog=ImplementingCQRS;Integrated Security=True" ) )
			{
				using ( var cmd = new SqlCommand( "INSERT INTO [dbo].[Search] ([DisplayName] ,[Collection] ,[ExternalId]) VALUES (@DisplayName, @Collection, @ExternalId)" ) )
				{
					cmd.Parameters.Add( "@DisplayName", ( message.CompanyName ) );
					cmd.Parameters.Add( "@Collection", "Companies" );
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
