using RavenDBApplication;
using RavenDBApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SetupRavenDBData
{
	class Program
	{
		static void Main( string[] args )
		{
			var store = Server.CreateDocumentStore();

			#region setup Person/Order

			using( var session = store.OpenSession() )
			{
				var p1 = new Person()
				{
					FirstName = "Mauro",
					LastName = "Servienti"
				};

				session.Store( p1 );
				session.Store( new Order()
				{
					Customer = p1
				} );

				var p2 = new Person()
				{
					FirstName = "Nazareno",
					LastName = "Manco"
				};

				session.Store( p2 );
				session.Store( new Order()
				{
					Customer = p2
				} );

				var c1 = new Company()
				{
					Name = "topics.it"
				};

				session.Store( c1 );
				session.Store( new Order()
				{
					Customer = c1
				} );

				var c2 = new Company()
				{
					Name = "Managed Designs"
				};

				session.Store( c2 );
				session.Store( new Order()
				{
					Customer = c2
				} );

				session.SaveChanges();
			}

			#endregion

			#region sample product setup

			using( var session = store.OpenSession() )
			{
				session.Store( new Product()
				{
					Name = "Trousers",
					Attributes = new List<ProductAttribute>()
					{
						new ProductAttribute()
						{
							Name = "Size",
							Value = new AttributeValue<int>()
							{ 
								Value = 40
							}
						},
						new ProductAttribute()
						{
							Name = "Color",
							Value = new AttributeValue<String>()
							{ 
								Value = "Blue"
							}
						}
					}
				} );

				session.Store( new Product()
				{
					Name = "Trousers",
					Attributes = new List<ProductAttribute>()
					{
						new ProductAttribute()
						{
							Name = "Size",
							Value = new AttributeValue<int>()
							{ 
								Value = 45
							}
						},
						new ProductAttribute()
						{
							Name = "Color",
							Value = new AttributeValue<String>()
							{ 
								Value = "Blue"
							}
						}
					}
				} );

				session.Store( new Product()
				{
					Name = "Trousers",
					Attributes = new List<ProductAttribute>()
					{
						new ProductAttribute()
						{
							Name = "Size",
							Value = new AttributeValue<int>()
							{ 
								Value = 38
							}
						},
						new ProductAttribute()
						{
							Name = "Color",
							Value = new AttributeValue<String>()
							{ 
								Value = "Blue"
							}
						}
					}
				} );


				session.Store( new Product()
				{
					Name = "Trousers",
					Attributes = new List<ProductAttribute>()
					{
						new ProductAttribute()
						{
							Name = "Size",
							Value = new AttributeValue<int>()
							{ 
								Value = 41
							}
						},
						new ProductAttribute()
						{
							Name = "Color",
							Value = new AttributeValue<String>()
							{ 
								Value = "Blue"
							}
						}
					}
				} );

				session.Store( new Product()
				{
					Name = "Trousers",
					Attributes = new List<ProductAttribute>()
					{
						new ProductAttribute()
						{
							Name = "Size",
							Value = new AttributeValue<int>()
							{ 
								Value = 43
							}
						},
						new ProductAttribute()
						{
							Name = "Color",
							Value = new AttributeValue<String>()
							{ 
								Value = "Blue"
							}
						}
					}
				} );

				session.Store( new Product()
				{
					Name = "Trousers",
					Attributes = new List<ProductAttribute>()
					{
						new ProductAttribute()
						{
							Name = "Size",
							Value = new AttributeValue<int>()
							{ 
								Value = 39
							}
						},
						new ProductAttribute()
						{
							Name = "Color",
							Value = new AttributeValue<String>()
							{ 
								Value = "Blue"
							}
						}
					}
				} );

				session.SaveChanges();
			}

			#endregion
		}
	}
}
