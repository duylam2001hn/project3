using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Gallery_art_3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "TestHome", action = "Artwork", id = UrlParameter.Optional }
            ) ;
            routes.MapRoute(
    name: "Default1",
    url: "{controller}/{action}/{id}",
    defaults: new { controller = "artworks", action = "Index", id = UrlParameter.Optional },
    namespaces: new string[] { "artworksController.Controller" }
);
        }
    }
}
