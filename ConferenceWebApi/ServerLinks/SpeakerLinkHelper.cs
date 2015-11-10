using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using ConferenceWebApi.Tools;
using ConferenceWebPack;
using Newtonsoft.Json.Linq;
using Tavis;
using Tavis.Home;
using Tavis.IANA;

namespace ConferenceWebApi.ServerLinks
{
    public static class SpeakerLinkHelper
    {
        public const string SpeakerByIdRoute = "SpeakerById";

        public static SpeakerLink CreateLink(HttpRequestMessage request, int speakerId)
        {
            return request.ResolveLink<SpeakerLink>(SpeakerByIdRoute, new { id = speakerId });
        }
        public static SpeakerLink CreateLink(HttpRequestMessage request, Speaker speaker)
        {
            return request.ResolveLink<SpeakerLink>(SpeakerByIdRoute, new { id = speaker.Id });
        }

        public static SpeakerLink WithHints(this SpeakerLink speakerLink)
        {
            speakerLink.AddHint<AllowHint>(h =>
            {
                h.AddMethod(HttpMethod.Get);
                h.AddMethod(HttpMethod.Put);
                h.AddMethod(HttpMethod.Delete);
            });
            speakerLink.AddHint<FormatsHint>(h =>
            {
                h.AddMediaType("text/plain");
                h.AddMediaType("application/hal+json");
            });
            return speakerLink;
        }


        public static IHttpActionResult CreateResponse(Speaker speakerInfo, HttpRequestMessage request)
        {
            
            if (request.Headers.Accept.Contains(new MediaTypeWithQualityHeaderValue("application/hal+json")))
            {
                return new OkResult(request)
                    .WithContent(CreateHalContent(speakerInfo, request));

            }

            // fall back to text/plain response
            return new OkResult(request)
                .WithContent(new StringContent(speakerInfo.Name + Environment.NewLine + speakerInfo.Bio))
                .WithLinkHeaders(new List<ILink>
                {
                    SessionsLinkHelper.CreateLink(request, speakerInfo),
                    new IconLink() { Target = new Uri(speakerInfo.ImageUrl) }
                });
        }


        private static HttpContent CreateHalContent(Speaker speakerInfo, HttpRequestMessage request)
        {
            dynamic jspeaker = new JObject();
            jspeaker.name = speakerInfo.Name;
            jspeaker.bio = speakerInfo.Bio;

            dynamic links = new JObject();

            dynamic iconLink = new JObject();
            iconLink.href = speakerInfo.ImageUrl;
            links.icon = iconLink;

            dynamic sessionsLink = new JObject();
            sessionsLink.href = SessionsLinkHelper.CreateLink(request, speakerInfo).Target;
            links[LinkHelper.GetLinkRelationTypeName<SessionsLink>()] = sessionsLink;

            jspeaker["_links"] = links;

            return new DynamicHalContent(jspeaker);
        }

    }
}
