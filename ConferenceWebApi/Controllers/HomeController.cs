using System.Net.Http;
using System.Web.Http;
using ConferenceWebApi.Tools;
using ConferenceWebPack;
using Tavis.Home;

namespace ConferenceWebApi.Controllers
{

    [Route("")]
    public class HomeController : ApiController
    {
        
        public HttpResponseMessage Get()
        {
            var home = new HomeDocument();

            home.AddResource(Request.ResolveLink<SessionsLink>(Links.GetSessionsByDay,null,"{?dayno}"));
            home.AddResource(Request.ResolveLink<SessionLink>(Links.GetSessionById, new { id = "[id]" }));
            home.AddResource(Request.ResolveLink<SpeakersLink>(Links.GetAllSpeakers));
            home.AddResource(Request.ResolveLink<SpeakerLink>(Links.GetSpeakerById, new { id = "[id]" }));

          
            return new HttpResponseMessage() {
                    Content = new HomeContent(home)
                };
        }
    }

    


    // Link to a list of events.  Could be events for the day, events that a speaker is doing, events on a topic
}
