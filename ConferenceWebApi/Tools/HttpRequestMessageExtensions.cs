using System;
using System.Net.Http;
using System.Web.Http.Routing;

namespace ConferenceWebApi.Tools
{
    public static class HttpRequestMessageExtensions
    {
        public const string GatewayUrlKey = "GatewayUrl";
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
            
            Uri targetUrl = GetTargetUrl(request, routeName, parameters, queryParams);
            var link = new T { Target = targetUrl };

            return link;
        }

        private static Uri GetTargetUrl(HttpRequestMessage request, string routeName, object parameters, string queryParams)
        {
            var urlHelper = new UrlHelper(request);
            var routeHref = urlHelper.Route(routeName, parameters) + (queryParams ?? "");
            Uri targetUrl;

            if (request.Properties.ContainsKey(GatewayUrlKey))
            {
                targetUrl = new Uri(request.Properties[GatewayUrlKey] as Uri, routeHref);
            }
            else
            {
                targetUrl = new Uri(request.RequestUri, routeHref);
            }

            return targetUrl;
        }

        public static T ResolveLinkTemplate<T>(this HttpRequestMessage request, string routeName) where T : Tavis.Link, new()
        {
            var urlHelper = new UrlHelper(request);
            var route = request.GetConfiguration().Routes[routeName];
            var link = new T { Target = GetTargetUrl(request, routeName, null,null) };
            return link;
        }
    }
}