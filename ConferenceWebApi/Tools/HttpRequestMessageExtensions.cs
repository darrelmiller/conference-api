using System;
using System.Net.Http;
using System.Web.Http.Routing;

namespace ConferenceWebApi.Tools
{
    public static class HttpRequestMessageExtensions
    {
        public static Uri ResolveUrl(this HttpRequestMessage request, string routeName, object parameters = null)
        {
            var urlHelper = new UrlHelper(request);
            return new Uri(urlHelper.Link(routeName, parameters));  
        }

        public static T ResolveLink<T>(this HttpRequestMessage request, string routeName, string queryParams) where T : Tavis.Link, new()
        {
            return request.ResolveLink<T>(routeName, null,queryParams);
        }

        public static T ResolveLink<T>(this HttpRequestMessage request, string routeName, object parameters = null, string queryParams = null)  where T : Tavis.Link, new()
        {
            var urlHelper = new UrlHelper(request);
            
            var link = new T {Target = new Uri(urlHelper.Link(routeName, parameters).Replace("[","{").Replace("]","}") + (queryParams ?? ""))};

            return link;
        }
        public static T ResolveLinkTemplate<T>(this HttpRequestMessage request, string routeName) where T : Tavis.Link, new()
        {
            var urlHelper = new UrlHelper(request);
            var route = request.GetConfiguration().Routes[routeName];
            var link = new T { Target = new Uri(urlHelper.Link(routeName, null)) };
            return link;
        }
    }
}