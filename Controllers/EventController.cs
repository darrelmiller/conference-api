using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using Tavis;
using ndc.LinkTypes;
using ndc.Model;
using ndc.Tools;

namespace ndc.Controllers
{
   [Route("event/{id}", Name = "GetEventById")]
    public class EventController : ApiController
    {
        private readonly DataService _dataService = new DataService();

        
        public HttpResponseMessage Get(int id)
        {
            var eventInfo = _dataService.EventRepository.Get(id);


            var response = new HttpResponseMessage()
                {
                    Content = new StringContent(eventInfo.Title)
                };
            response.Headers.AddLinkHeader(Request.ResolveLink<SpeakerLink>(Links.GetSpeakerById, new { id = eventInfo.SpeakerId}));
            return response;
        }

       

    }
}
