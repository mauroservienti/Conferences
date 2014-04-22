using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApiServer
{
    public class RouteConfig
    {
        public static void RegisterRoutes( RouteCollection routes )
        {
            routes.IgnoreRoute( "{*favicon}", new 
            { 
                favicon = @"(.*/)?favicon.([iI][cC][oO]|[gG][iI][fF])(/.*)?" 
            } );
            routes.IgnoreRoute( "{resource}.axd/{*pathInfo}" );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
				defaults: new { controller = "App", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}