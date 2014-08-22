using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Owin;
using ConferenceWebApi;
using Microsoft.Owin;
using Tavis.Owin;


namespace ConferenceApi
{
    class Program
    {
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            var uri = new Uri(ConfigurationManager.AppSettings.Get("host"));

            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
          
            var appFunc = WebApiAdapter.CreateWebApiAppFunc2(config);

            var host = new OwinServiceHost(uri, appFunc)
            {
                ServiceDescription = "Hypermedia driven Conference API",
                ServiceName = "ConferenceApi",
                ServiceDisplayName = "Conference API"
            };

            host.Initialize();
        }

      
    }
    public static class WebApiAdapter
    {
        public static Func<IDictionary<string, object>, Task> CreateWebApiAppFunc2(HttpConfiguration config, HttpMessageHandlerOptions options = null)
        {
            var app = new HttpServer(config);
            if (options == null)
            {
                options = new HttpMessageHandlerOptions()
                {
                    MessageHandler = app,
                    BufferPolicySelector = new OwinBufferPolicySelector(),
                    ExceptionLogger = new GlobalErrorLoggingService(),
                    ExceptionHandler = new GlobalErrorHandlerService()
                };
            }
            var handler = new HttpMessageHandlerAdapter(new NotFoundMiddleware(), options);
            return (env) => handler.Invoke(new OwinContext(env));
        }
    
    }

    public class NotFoundMiddleware : OwinMiddleware
    {
        public NotFoundMiddleware() : base(null)
        {
        }

        public override Task Invoke(IOwinContext context)
        {
            context.Response.StatusCode = 404;
            return Task.FromResult(0);
        }
    }

   


}
