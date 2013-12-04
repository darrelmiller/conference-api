using System.Net.Http;
using System.Web.Http;
using ConferenceWebApi.Tools;
using ConferenceWebPack;
using Tavis.Home;
using Tavis.IANA;

namespace ConferenceWebApi.Controllers
{

    [Route("")]
    public class HomeController : ApiController
    {
        
        public HttpResponseMessage Get()
        {
            var home = new HomeDocument();

            // AllDays
            var resolveLink = Request.ResolveLink<DaysLink>(Links.AllDays);
            resolveLink.AddHint<AllowHint>(h => h.AddMethod(HttpMethod.Get));
            resolveLink.AddHint<FormatsHint>(h => h.AddMediaType("application/vnd.collection+json"));
            home.AddResource(resolveLink);

            // SessionsByDay
            var sessionsLink = Request.ResolveLink<SessionsLink>(Links.SessionsByDay, null, "{?dayno,keyword,speakername}"); // ,keyword,speakername
            sessionsLink.AddHint<AllowHint>(h => h.AddMethod(HttpMethod.Get));

            sessionsLink.AddHint<FormatsHint>(h => h.AddMediaType("application/vnd.collection+json"));
            home.AddResource(sessionsLink);

            // SessionById
            var sessionLink = Request.ResolveLink<SessionLink>(Links.SessionById, new { id = "[id]" });
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
            
            home.AddResource(sessionLink);

            // AllSpeakers
            var speakersLink = Request.ResolveLink<SpeakersLink>(Links.AllSpeakers);
            speakersLink.AddHint<AllowHint>(h =>
            {
                h.AddMethod(HttpMethod.Get);
                h.AddMethod(HttpMethod.Put);
                h.AddMethod(HttpMethod.Delete);
            });
            speakersLink.AddHint<FormatsHint>(h => h.AddMediaType("application/vnd.collection+json"));
            home.AddResource(speakersLink);

            // SpeakerById
            var speakerLink = Request.ResolveLink<SpeakerLink>(Links.SpeakerById, new { id = "[id]" });
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
            home.AddResource(speakerLink);

            // AllTopics
            var topicsLink = Request.ResolveLink<TopicsLink>(Links.AllTopics);
            topicsLink.AddHint<AllowHint>(h =>
            {
                h.AddMethod(HttpMethod.Get);
                h.AddMethod(HttpMethod.Put);
                h.AddMethod(HttpMethod.Delete);
            });
            topicsLink.AddHint<FormatsHint>(h => h.AddMediaType("application/collection+json"));
            home.AddResource(topicsLink);
           
          
            return Request.RespondOk(new HomeContent(home));
        }
    }

    


    // Link to a list of events.  Could be events for the day, events that a speaker is doing, events on a topic
}
