using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ConferenceWebApi.Tools;
using ConferenceWebPack;
using Tavis;

namespace ConferenceWebApi.ServerLinks
{
    public static class TopicLinkHelper
    {
        public const string TopicByIdRoute = "TopicById";

        public static TopicLink CreateLink(HttpRequestMessage request, Topic session)
        {
            return request.ResolveLink<TopicLink>(TopicByIdRoute, new { id = session.Id});
        }

        public static HttpResponseMessage CreateResponse(Topic topicInfo, HttpRequestMessage request)
        {
            var response = request.RespondOk();
            response.Content = new StringContent(topicInfo.Name);
            response.Headers.AddLinkHeader(SessionsLinkHelper.CreateLink(request, topicInfo));
            response.Headers.AddLinkHeader(SpeakersLinkHelper.CreateLink(request, topicInfo));

            return response;
        }

    }
}
