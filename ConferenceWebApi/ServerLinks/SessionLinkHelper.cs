using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using ConferenceWebApi.DataModel;
using ConferenceWebApi.Tools;
using ConferenceWebPack;
using Newtonsoft.Json.Linq;
using Tavis;
using Tavis.Home;
using Tavis.IANA;

namespace ConferenceWebApi.ServerLinks
{
    public static class SessionLinkHelper
    {
        public const string SessionByTitleRoute = "SessionByTitle";
        public const string SessionByIdRoute = "SessionById";

        public static SessionLink CreateLink(HttpRequestMessage request)
        {
            return request.ResolveLink<SessionLink>(SessionByTitleRoute, "{?title}");
        }

        public static SessionLink CreateLink(HttpRequestMessage request, Session session)
        {
            return request.ResolveLink<SessionLink>(SessionByIdRoute, new { id = session.Id});
        }

        public static SessionLink WithHints(this SessionLink sessionLink)
        {
            sessionLink.AddHint<AllowHint>(h =>
            {
                h.AddMethod(HttpMethod.Get);
                h.AddMethod(HttpMethod.Put);
                h.AddMethod(HttpMethod.Delete);
            });

            sessionLink.AddHint<FormatsHint>(h =>
            {
                h.AddMediaType("text/plain");
                h.AddMediaType("application/hal+json");
            });
            return sessionLink;
        }

        public static IHttpActionResult CreateResponse(Session session, HttpRequestMessage request)
        {
            IHttpActionResult response = new OkResult(request);

            if (request.Headers.Accept.Contains(new MediaTypeWithQualityHeaderValue("application/hal+json")))
            {
                response = response.WithContent(CreateHalContent(session,request));
            }
            else
            {
                response = response
                    .WithContent(new StringContent(session.Title + Environment.NewLine + session.Description))
                    .WithLinkHeaders(new List<ILink> {SpeakerLinkHelper.CreateLink(request, session.SpeakerId)});
            
                
            }
            return response;
        }

        private static HttpContent CreateHalContent(Session session, HttpRequestMessage request)
        {
            dynamic jsession = new JObject();
            jsession.title = session.Title;
            jsession.description = session.Description;
            jsession.timeslot = session.TimeslotDescription;

            dynamic links = new JObject();

            dynamic speakerLink = new JObject();
            speakerLink.href = SpeakerLinkHelper.CreateLink(request,session.SpeakerId).Target;
            links[LinkHelper.GetLinkRelationTypeName<SpeakerLink>()] = speakerLink;

            jsession["_links"] = links;

            return new DynamicHalContent(jsession);
        }
    }
}
