using System;
using System.Configuration;
using System.Diagnostics;
using System.Web.Http;
using ConferenceWebApi;
using Tavis.Owin;


namespace ConferenceApi
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var uri = new Uri(ConfigurationManager.AppSettings.Get("host"));

            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
          
            var appFunc = WebApiAdapter.CreateWebApiAppFunc(config);

            var host = new OwinServiceHost(uri, appFunc)
            {
                ServiceDescription = "Hypermedia driven Conference API",
                ServiceName = "ConferenceApi",
                ServiceDisplayName = "Conference API"
            };

            host.Initialize();
        }

      
    }

   


}
