using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Hosting;
using System.Web.Http.Owin;
using System.Web.Http.Results;
using ConferenceWebApi.DataModel;
using ConferenceWebApi.Tools;
using Microsoft.Owin;
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
          
            config.MapHttpAttributeRoutes();
            config.Services.Replace(typeof(IExceptionHandler),new GlobalErrorHandlerService());
            config.MessageHandlers.Add(new ErrorHandlerMessageHandler());
            config.MessageHandlers.Add(new BufferNonStreamedContentHandler());
            config.EnableSystemDiagnosticsTracing();
        }
    }


    
    public class GlobalErrorLoggingService : IExceptionLogger
    {
        public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
           Console.WriteLine("Exception : " + context.Exception.Message);
            return Task.FromResult(0);
        }
    }


public class GlobalErrorHandlerService : IExceptionHandler
{
    public Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
    {
        if (context.CatchBlock != ExceptionCatchBlocks.HttpServer)
        {
            context.Result = null;
        }
        return Task.FromResult(0);
    }
}
    public class ErrorHandlerMessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                return await base.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                return ConvertExceptionToHttpResponse(ex);
            }
        }

        private static HttpResponseMessage ConvertExceptionToHttpResponse(Exception ex)
        {
            // Centralized error handling code
            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }

public class BufferNonStreamedContentHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        if (response.Content != null)
        {
            var bufferPolicy = (IHostBufferPolicySelector)request.GetConfiguration().Services.GetService(typeof (IHostBufferPolicySelector));
            if (bufferPolicy.UseBufferedOutputStream(response))  // If the host is going to buffer it anyway,
            {
                await response.Content.LoadIntoBufferAsync();  // Buffer it now so we can catch the exception
            }
        }
        return response;
    }
}
}
