using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Tavis;
using Tavis.Home;
using ndc.LinkTypes;
using ndc.Tools;

namespace ndc.Controllers
{

    [Route("")]
    public class HomeController : ApiController
    {
        
        public HttpResponseMessage Get()
        {
            var home = new HomeDocument();

            home.AddResource(Request.ResolveLink<EventsLink>(Links.GetEventsByDay,null,"{?dayno}"));
            home.AddResource(Request.ResolveLink<EventLink>(Links.GetEventById, new { id = "[id]" }));
            home.AddResource(Request.ResolveLink<SpeakersLink>(Links.GetAllSpeakers));
            home.AddResource(Request.ResolveLink<SpeakerLink>(Links.GetSpeakerById, new { id = "[id]" }));

          
            return new HttpResponseMessage() {
                    Content = new HomeContent(home)
                };
        }
    }

    


    // Link to a list of events.  Could be events for the day, events that a speaker is doing, events on a topic
}
