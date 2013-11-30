using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using ConferenceWebApi.DataModel;
using ConferenceWebApi.Tools;
using Microsoft.Practices.Unity;
using Unity.WebApi;


namespace ConferenceWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var unityContainer = new UnityContainer();
            unityContainer.RegisterInstance<DataService>(new DataService());
            unityContainer.RegisterTypes(
                AllClasses.FromLoadedAssemblies().Where(t => t.IsAssignableFrom(typeof(IHttpController))),
                WithMappings.FromAllInterfaces);
            config.DependencyResolver = new UnityDependencyResolver(unityContainer);
            config.MessageHandlers.Add(new DescribedByHandler());
            config.MessageHandlers.Add(new HeadMessageHandler());
            config.MapHttpAttributeRoutes();


            //config.EnableSystemDiagnosticsTracing();
        }
    }
}
