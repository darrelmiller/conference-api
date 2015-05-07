using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Hosting;
using ConferenceWebApi.DataModel;
using Microsoft.Practices.Unity;
using Tavis;


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
            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
               
            };
        }
    }

public class BufferNonStreamedContentHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        if (response.Content != null)
        {
            var bufferPolicy = request.GetConfiguration().Services.GetHostBufferPolicySelector();
            if (bufferPolicy.UseBufferedOutputStream(response))  // If the host is going to buffer it anyway,
            {
                await response.Content.LoadIntoBufferAsync();  // Buffer it now so we can catch the exception
            }
        }
        return response;
    }
}

public sealed class UnityDependencyResolver : IDependencyResolver
{
    private IUnityContainer container;
    private SharedDependencyScope sharedScope;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnityDependencyResolver"/> class for a container.
    /// </summary>
    /// <param name="container">The <see cref="IUnityContainer"/> to wrap with the <see cref="IDependencyResolver"/>
    /// interface implementation.</param>
    public UnityDependencyResolver(IUnityContainer container)
    {
        if (container == null)
        {
            throw new ArgumentNullException("container");
        }
        this.container = container;
        this.sharedScope = new SharedDependencyScope(container);
    }

    /// <summary>
    /// Reuses the same scope to resolve all the instances.
    /// </summary>
    /// <returns>The shared dependency scope.</returns>
    public IDependencyScope BeginScope()
    {
        return this.sharedScope;
    }

    /// <summary>
    /// Disposes the wrapped <see cref="IUnityContainer"/>.
    /// </summary>
    public void Dispose()
    {
        this.container.Dispose();
        this.sharedScope.Dispose();
    }

    /// <summary>
    /// Resolves an instance of the default requested type from the container.
    /// </summary>
    /// <param name="serviceType">The <see cref="Type"/> of the object to get from the container.</param>
    /// <returns>The requested object.</returns>
    /// 
    [DebuggerStepThrough]
    public object GetService(Type serviceType)
    {
        try
        {
            return this.container.Resolve(serviceType);
        }
        catch (ResolutionFailedException)
        {
            return null;
        }
    }

    /// <summary>
    /// Resolves multiply registered services.
    /// </summary>
    /// <param name="serviceType">The type of the requested services.</param>
    /// <returns>The requested services.</returns>
    public IEnumerable<object> GetServices(Type serviceType)
    {
        try
        {
            return this.container.ResolveAll(serviceType);
        }
        catch (ResolutionFailedException)
        {
            return null;
        }
    }

    private sealed class SharedDependencyScope : IDependencyScope
    {
        private IUnityContainer container;

        public SharedDependencyScope(IUnityContainer container)
        {
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            return this.container.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.container.ResolveAll(serviceType);
        }

        public void Dispose()
        {
            // NO-OP, as the container is shared.
        }
    }
}
}
