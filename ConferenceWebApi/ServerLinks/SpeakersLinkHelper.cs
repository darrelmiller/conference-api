using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using ConferenceWebApi.Tools;
using ConferenceWebPack;
using Tavis.Home;
using Tavis.IANA;
using WebApiContrib.CollectionJson;

namespace ConferenceWebApi.ServerLinks
{
    public static class SpeakersLinkHelper
    {
        public const string SpeakersSearchRoute = "SpeakersSearch";
        public const string TopicSpeakersRoute = "TopicSpeakers";
       

        public static SpeakersLink CreateLink(HttpRequestMessage request)
        {
            return request.ResolveLink<SpeakersLink>(SpeakersSearchRoute, "{?speakername}");
        }

        public static SpeakersLink CreateLink(HttpRequestMessage request, int dayno)
        {
            return request.ResolveLink<SpeakersLink>(SpeakersSearchRoute, new { dayno });
        }

        public static SpeakersLink CreateLink(HttpRequestMessage request, Topic topic)
        {
            return request.ResolveLink<SpeakersLink>(TopicSpeakersRoute,new {id =topic.Id});
        }

        public static SpeakersLink WithHints(this SpeakersLink speakersLink)
        {
            speakersLink.AddHint<AllowHint>(h =>
            {
                h.AddMethod(HttpMethod.Get);
                h.AddMethod(HttpMethod.Put);
                h.AddMethod(HttpMethod.Delete);
            });
            speakersLink.AddHint<FormatsHint>(h => h.AddMediaType("application/vnd.collection+json"));
            return speakersLink;
        }

        public static IHttpActionResult CreateResponse(IEnumerable<Speaker> speakers, HttpRequestMessage request)
        {
            var collection = new Collection();

            foreach (var speaker in speakers)
            {
                var item = new Item();
                item.Href = SpeakerLinkHelper.CreateLink(request, speaker).Target;
                item.Data.Add(new Data { Name = "Name", Value = speaker.Name });
                item.Links.Add(SessionsLinkHelper.CreateLink(request,speaker).ToCJLink());
                collection.Items.Add(item);
            }
            collection.Href = request.RequestUri;

            return new OkResult(request)
                .WithContent(new CollectionJsonContent(collection));
        }
    }
}
