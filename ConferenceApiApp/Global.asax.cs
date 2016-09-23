using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using ConferenceWebApi;

namespace ConferenceApiApp
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            GlobalConfiguration.Configure(c =>
            {
                WebApiConfig.Register(c);
                SwaggerConfig.Register(c);
                WebApiConfig.RegisterRoutes(c);
            }
            );

        }
    }
}
