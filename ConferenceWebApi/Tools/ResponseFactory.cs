using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Tavis;

namespace ConferenceWebApi.Tools
{
    public static class ResponseFactory
    {
        public static HttpResponseMessage RespondNotFound(this HttpRequestMessage request, string reasonPhrase = null)
        {
            
            var response = new HttpResponseMessage(HttpStatusCode.NotFound) {RequestMessage = request};
            if (reasonPhrase != null) response.ReasonPhrase = reasonPhrase;
            return response;
        }
        public static HttpResponseMessage RespondSeeOther(this HttpRequestMessage request, Link link)
        {

            var response = new HttpResponseMessage(HttpStatusCode.SeeOther) {RequestMessage = request};
            response.Headers.Location = link.Target;
            return response;
        }

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
}
