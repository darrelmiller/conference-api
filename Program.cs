using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
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

            var server = WebApp.Start(uri, builder =>
            {
                var config = new HttpConfiguration();
                Program.Configure(config);
                builder.UseWebApi(config);
            });

            Console.WriteLine("Started Owin HttpListener on " + uri);
            Console.ReadLine();
            server.Dispose();
        }

        public static void Configure(HttpConfiguration config)
        {
            
            config.MessageHandlers.Add(new DescribedByHandler());
            config.MessageHandlers.Add(new HeadMessageHandler());
            config.MapHttpAttributeRoutes();
            
    
            config.EnableSystemDiagnosticsTracing();

        }
    }

 
}
