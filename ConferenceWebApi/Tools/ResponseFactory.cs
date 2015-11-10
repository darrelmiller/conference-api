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



    public class BadRequestResult : BaseChainedResult
    {
        private readonly ProblemDocument _problem;

        public BadRequestResult(ProblemDocument problem)
            : base(null)
        {
            _problem = problem;

        }

        public override void ApplyAction(HttpResponseMessage response)
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            response.Content = new ProblemContent(_problem);
        }
    }

    public class NotFoundResult : BaseChainedResult
    {
        private readonly string _errorMessage;

        public NotFoundResult(string errorMessage)
            : base(null)
        {
            _errorMessage = errorMessage;
        }

        public override void ApplyAction(HttpResponseMessage response)
        {
            response.StatusCode = HttpStatusCode.NotFound;
            response.ReasonPhrase = _errorMessage;
        }
    }


    public class ContentResult2 : BaseChainedResult
    {
        private readonly HttpContent _content;

        public ContentResult2(IHttpActionResult actionResult, HttpContent content)
            : base(actionResult)
        {
            _content = content;
        }

        public override void ApplyAction(HttpResponseMessage response)
        {
            response.Content = _content;
        }
    }

    public class LinkHeadersResult2 : BaseChainedResult
    {
        private readonly List<ILink> _linkHeaders;

        public LinkHeadersResult2(IHttpActionResult actionResult, List<ILink> linkHeaders)
            : base(actionResult)
        {
            _linkHeaders = linkHeaders;
        }

        public override void ApplyAction(HttpResponseMessage response)
        {
            response.Headers.AddLinkHeaders(_linkHeaders);
        }
    }

    public class CachingResult2 : BaseChainedResult
    {
        private readonly CacheControlHeaderValue _cachingHeaderValue;

        public CachingResult2(IHttpActionResult actionResult, CacheControlHeaderValue cachingHeaderValue) : base(actionResult)
        {
            _cachingHeaderValue = cachingHeaderValue;
        }

        public override void ApplyAction(HttpResponseMessage response)
        {
            response.Headers.CacheControl = _cachingHeaderValue;
        }
    }

   


    public static class ActionResultExtensions
    {
        public static IHttpActionResult WithContent(this IHttpActionResult actionResult, HttpContent content)
        {
            return new ContentResult2(actionResult, content);
        }
        public static IHttpActionResult WithCaching(this IHttpActionResult actionResult, CacheControlHeaderValue cacheControl)
        {
            return new CachingResult2(actionResult, cacheControl);
        }
        public static IHttpActionResult WithLinkHeaders(this IHttpActionResult actionResult, List<ILink> linkHeaders)
        {
            return new LinkHeadersResult2(actionResult, linkHeaders);
        }
    }
}
