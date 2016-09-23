using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ConferenceWebApi.Tools;
using ConferenceWebPack;
using Tavis;
using System.Web.Http;
using System.Web.Http.Results;

namespace ConferenceWebApi.ServerLinks
{
    public static class TopicLinkHelper
    {
        public const string TopicByIdRoute = "TopicById";

        public static TopicLink CreateLink(HttpRequestMessage request, Topic session)
        {
            return request.ResolveLink<TopicLink>(TopicByIdRoute, new { id = session.Id});
        }

        public static IHttpActionResult CreateResponse(Topic topicInfo, HttpRequestMessage request)
        {
            var response = new OkResult(request)
            .WithContent( new StringContent(topicInfo.Name))
            .WithLinkHeaders(new List<ILink>
                {
                    SessionsLinkHelper.CreateLink(request, topicInfo),
                    SpeakersLinkHelper.CreateLink(request, topicInfo)
               });

            return response;
        }

    }
}
