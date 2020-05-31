using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApi.Utils;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Конфигурация и службы веб-API

            // Маршруты веб-API
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
              name: "DefaultApi2",
              routeTemplate: "{controller}/{action}",
              defaults: new { id = RouteParameter.Optional }
          );

            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
