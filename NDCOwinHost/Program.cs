using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using Owin;

namespace ndc
{
    class Program
    {
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            string uri = String.Format("http://{0}:1001/", Environment.MachineName);

            using (WebApp.Start(uri, builder =>
                {
                    var config = new HttpConfiguration();
                    ConferenceWebApi.WebApiConfig.Register(config);
                    builder.UseWebApi(config);
                }))
            {

                Console.WriteLine("Started Owin HttpListener on " + uri);
                Console.ReadLine();

            }
            
        }

       
    }

   
}
