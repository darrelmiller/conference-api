using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Tavis;

namespace ConferenceWebApi.Tools
{
    public static class ResponseFactory
    {
   


        public static HttpResponseMessage RespondOk(this HttpRequestMessage request, HttpContent content = null)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                RequestMessage = request,
                Content = content
            };
            return response;
        }

       
    }


    public class NotFoundResult : IHttpActionResult
    {
        private readonly string _errorMessage;

        public NotFoundResult(string errorMessage)
        {
            _errorMessage = errorMessage;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage() { ReasonPhrase = _errorMessage});
        }
    }

    public class ContentResult : IHttpActionResult
    {
        private readonly IHttpActionResult _actionResult;
        private readonly HttpContent _content;

        public ContentResult(IHttpActionResult actionResult, HttpContent content)
        {
            _actionResult = actionResult;
            _content = content;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {

            return _actionResult.ExecuteAsync(cancellationToken)
                .ContinueWith(t =>
                {
                    t.Result.Content = _content;
                    return t.Result;
                },cancellationToken);
        }
    }

    public class LinkHeadersResult : IHttpActionResult
    {
        private readonly IHttpActionResult _actionResult;
        private readonly List<Link> _linkHeaders;


        public LinkHeadersResult(IHttpActionResult actionResult, List<Link> linkHeaders)
        {
            _actionResult = actionResult;
            _linkHeaders = linkHeaders;
      
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {

            return _actionResult.ExecuteAsync(cancellationToken)
                .ContinueWith(t =>
                {
                    t.Result.Headers.AddLinkHeaders(_linkHeaders);
                    return t.Result;
                }, cancellationToken);
        }
    }

    public class CachingResult : IHttpActionResult
    {
        private readonly IHttpActionResult _actionResult;
        private readonly CacheControlHeaderValue _cachingHeaderValue;


        public CachingResult(IHttpActionResult actionResult, CacheControlHeaderValue cachingHeaderValue)
        {
            _actionResult = actionResult;
            _cachingHeaderValue = cachingHeaderValue;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {

            return _actionResult.ExecuteAsync(cancellationToken)
                .ContinueWith(t =>
                {
                    t.Result.Headers.CacheControl = _cachingHeaderValue;
                    return t.Result;
                }, cancellationToken);
        }
    }


    public static class ActionResultExtensions
    {
        public static IHttpActionResult WithContent(this IHttpActionResult actionResult, HttpContent content)
        {
            return new ContentResult(actionResult, content);
        }
        public static IHttpActionResult WithCaching(this IHttpActionResult actionResult, CacheControlHeaderValue cacheControl)
        {
            return new CachingResult(actionResult, cacheControl);
        }
        public static IHttpActionResult WithLinkHeaders(this IHttpActionResult actionResult, List<Link> linkHeaders)
        {
            return new LinkHeadersResult(actionResult, linkHeaders);
        }
    }
}
