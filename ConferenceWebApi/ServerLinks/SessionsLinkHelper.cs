using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using ConferenceWebApi.DataModel;
using ConferenceWebApi.Tools;
using ConferenceWebPack;
using Tavis;
using Tavis.IANA;
using Tavis.Home;
using CollectionJson;

namespace ConferenceWebApi.ServerLinks
{
    public static class SessionsLinkHelper 
    {
        public const string SessionsSearchRoute = "SessionsSearch";
        public const string TopicSessionsRoute = "TopicSessions";
        public const string SpeakerSessionsRoute = "SpeakerSessions";

        public static SessionsLink CreateLink(HttpRequestMessage request)
        {
            var link = request.ResolveLink<SessionsLink>(SessionsSearchRoute, "{?dayno,keyword,speakername}");

            return link;
        }

        public static SessionsLink CreateLink(HttpRequestMessage request, string speakername)
        {
            return request.ResolveLink<SessionsLink>(SessionsSearchRoute, new { speakername });
        }

        public static SessionsLink CreateLink(HttpRequestMessage request, int dayno)
        {
            return request.ResolveLink<SessionsLink>(SessionsSearchRoute, new { dayno });
        }

        public static SessionsLink CreateLink(HttpRequestMessage request, Speaker speaker)
        {
            return request.ResolveLink<SessionsLink>(SpeakerSessionsRoute, new { id = speaker.Id });
        }

        public static SessionsLink CreateLink(HttpRequestMessage request, Topic topic)
        {
            return request.ResolveLink<SessionsLink>(TopicSessionsRoute, new { id = topic.Id });
        }

        public static SessionsLink WithHints(this SessionsLink link)
        {
            link.AddHint<AllowHint>(h => h.AddMethod(HttpMethod.Get));
            link.AddHint<FormatsHint>(h => h.AddMediaType("application/vnd.collection+json"));
            return link;
        }

        public static IHttpActionResult CreateResponse(IEnumerable<Session> sessions, DataService _dataService, HttpRequestMessage Request)
        {
            Collection collection = GetSessionsCollection(sessions, _dataService, Request);

            return new OkResult(Request)
                .WithContent(new CollectionJsonContent(collection)); 

        }

        // Converts list of domain objects into C+J media type DOM
        // Static so it can be re-used by multiple controllers ( e.g. Sessions?topicid={topicid}  and topic/{id}/sessions)

        public static Collection GetSessionsCollection(IEnumerable<Session> sessions, DataService dataService, HttpRequestMessage request)
        {
            var eventsCollection = new Collection();

            foreach (var session in sessions)
            {
                var item = new Item();
                item.Href = SessionLinkHelper.CreateLink(request,session).Target;
                item.Data.Add(new Data { Name = "Title", Value = session.Title });
                item.Data.Add(new Data { Name = "Timeslot", Value = session.TimeslotDescription });
                if (session.SpeakerId != 0)
                {
                    item.Data.Add(new Data
                    {
                        Name = "Speaker",
                        Value = dataService.SpeakerRepository.Get(session.SpeakerId).Name
                    });
                    item.Links.Add(SpeakerLinkHelper.CreateLink(request, dataService.SpeakerRepository.Get(session.SpeakerId)).ToCJLink());
                }

                item.Links.Add(TopicsLinkHelper.CreateLink(request, session).ToCJLink());
                eventsCollection.Items.Add(item);
            }
            return eventsCollection;
        }
    }
}
