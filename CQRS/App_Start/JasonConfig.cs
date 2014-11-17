using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Castle.Windsor;
using Jason.Configuration;
using Jason.WebAPI.Validation;
using Newtonsoft.Json;
using Jason.WebAPI.Filters;
using System.Net.Http;
using CQRS.Infrastructure;

namespace CQRS
{
    public class JasonConfig
    {
        public static void Initialize( String pathToScanForAssemblies, String assemblySelectPattern, IWindsorContainer container )
        {
            var jasonConfig = new DefaultJasonServerConfiguration
            (
				pathToScanForAssemblies: pathToScanForAssemblies,
                assemblySelectPattern: assemblySelectPattern
            );

			jasonConfig.Container = new WindsorJasonContainerProxy( container );
			jasonConfig.AddEndpoint( new Jason.WebAPI.JasonWebAPIEndpoint() 
			{
				IsCommandConvention = t =>
				{
					return t.Namespace != null
						&& t.Namespace.EndsWith( ".Commands" );
				}
			} )
			.UsingAsFallbackCommandValidator<ObjectDataAnnotationValidator>()
			.Initialize();
        }
    }
}