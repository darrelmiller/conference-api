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
using Tavis.IANA;
using Tavis.Home;
using WebApiContrib.CollectionJson;

namespace ConferenceWebApi.ServerLinks
{
    public static class TopicsLinkHelper
    {
        public const string TopicsSearchRoute = "TopicsSearch";
        public const string SessionTopicsRoute = "SessionTopics";
        public const string SpeakerTopicsRoute = "SpeakerTopics";


        public static TopicsLink CreateLink(HttpRequestMessage request)
        {
            return request.ResolveLink<TopicsLink>(TopicsSearchRoute);
        }
        
        public static TopicsLink CreateLink(HttpRequestMessage request, int dayno)
        {
            return request.ResolveLink<TopicsLink>(TopicsSearchRoute, new { dayno });
        }

        public static TopicsLink CreateLink(HttpRequestMessage request,  Session session)
        {
            return request.ResolveLink<TopicsLink>(SessionTopicsRoute, new { id = session.Id });
        }

        public static TopicsLink CreateLink(HttpRequestMessage request, Speaker speaker)
        {
            return request.ResolveLink<TopicsLink>(SpeakerTopicsRoute, new { id = speaker.Id });
        }

        public static TopicsLink WithHints(this TopicsLink topicsLink)
        {
            topicsLink.AddHint<AllowHint>(h =>
            {
                h.AddMethod(HttpMethod.Get);
                h.AddMethod(HttpMethod.Put);
                h.AddMethod(HttpMethod.Delete);
            });
            topicsLink.AddHint<FormatsHint>(h => h.AddMediaType("application/collection+json"));
            return topicsLink;
        }


        public static  IHttpActionResult GetResponse(IEnumerable<Topic> topics, HttpRequestMessage request)
        {
            var topicsCollection = GetCollection(topics, request);

            return new OkResult(request).WithContent(new CollectionJsonContent(topicsCollection));
        }


        public static Collection GetCollection(IEnumerable<Topic> topics, HttpRequestMessage request)
        {
            var eventsCollection = new Collection();

            foreach (var topic in topics)
            {
                var item = new Item();
                item.Href = request.ResolveLink<TopicLink>(TopicLinkHelper.TopicByIdRoute, new { id = topic.Id }).Target;
                item.Data.Add(new Data { Name = "Title", Value = topic.Name });
                item.Links.Add(SessionsLinkHelper.CreateLink(request,topic).ToCJLink());
                eventsCollection.Items.Add(item);
            }
            return eventsCollection;
        }
    }
}
