using System.Globalization;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using WebActivatorEx;
using ConferenceApiApp;
using ConferenceWebApi;



namespace ConferenceApiApp
{
    public class SwaggerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var swaggerEnabledConfig = new SwaggerEnabledConfiguration(config, SwaggerDocsConfig.DefaultRootUrlResolver, new[] { " " });  // Empty string causes "undefined" to appear
            swaggerEnabledConfig.EnableSwaggerUi();
        }
    }
}