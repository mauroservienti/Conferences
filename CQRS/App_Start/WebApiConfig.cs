using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace CQRS
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

















			config.Routes.MapHttpRoute(
				"DefaultApiWithId",
				"api/{controller}/{id}",
				new { id = RouteParameter.Optional }, new { id = @"\d+" } );

			config.Routes.MapHttpRoute(
				"DefaultApiWithAction",
				"api/{controller}/{action}" );

			config.Routes.MapHttpRoute(
				"DefaultApiWithActionAndId",
				"api/{controller}/{action}/{id}",
				new { id = RouteParameter.Optional }, new { id = @"\d+" } );

			config.Routes.MapHttpRoute(
				"DefaultApiGet",
				"api/{controller}",
				new { action = "Get", controller = "Root" },
				new { httpMethod = new HttpMethodConstraint( HttpMethod.Get ) } );

			config.Routes.MapHttpRoute(
				"DefaultApiPost",
				"api/{controller}",
				new { action = "Post" },
				new { httpMethod = new HttpMethodConstraint( HttpMethod.Post ) } );

			config.Routes.MapHttpRoute(
				"DefaultApiPostWithAction",
				"api/{controller}/{action}",
				new { httpMethod = new HttpMethodConstraint( HttpMethod.Post ) } );



















			config.Formatters
				.JsonFormatter
				.SerializerSettings
				.ContractResolver = new CamelCasePropertyNamesContractResolver();

			//config.Routes.MapHttpRoute(
			//	"DefaultApiWithId",
			//	"api/{controller}/{id}",
			//	new { id = RouteParameter.Optional },
			//	new { id = @"\d+" } );

			//config.Routes.MapHttpRoute(
			//	"DefaultApiWithAction",
			//	"api/{controller}/{action}" );

			//config.Routes.MapHttpRoute(
			//	"DefaultApiWithActionAndId",
			//	"api/{controller}/{action}/{id}",
			//	new { id = RouteParameter.Optional },
			//	new { id = @"\d+" } );

			//config.Routes.MapHttpRoute(
			//	"DefaultApiGet",
			//	"api/{controller}",
			//	new { action = "Get", controller = "Root" },
			//	new { httpMethod = new HttpMethodConstraint( HttpMethod.Get ) } );

			//config.Routes.MapHttpRoute(
			//	"DefaultApiPost",
			//	"api/{controller}",
			//	new { action = "Post" },
			//	new { httpMethod = new HttpMethodConstraint( HttpMethod.Post ) } );

			//config.Routes.MapHttpRoute(
			//	"DefaultApiPostWithAction",
			//	"api/{controller}/{action}",
			//	new { action = "Post" },
			//	new { httpMethod = new HttpMethodConstraint( HttpMethod.Post ) } );
        }
    }
}
