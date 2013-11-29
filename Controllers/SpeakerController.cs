using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Tavis;
using ndc.LinkTypes;
using ndc.Model;
using ndc.Tools;

namespace ndc.Controllers
{
     [RoutePrefix("speaker")]
    public class SpeakerController : ApiController
    {
         private readonly DataService _dataService = new DataService();

        [Route("{id}",Name = Links.GetSpeakerById)]
        public HttpResponseMessage GetSpeaker(int id)
        {

            var speakerInfo = _dataService.SpeakerRepository.Get(id);

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(speakerInfo.Name)
            };
            response.Headers.AddLinkHeader(Request.ResolveLink<EventsLink>(Links.GetEventsBySpeaker, new { speakerInfo.Id }));
            return response;
        }
    }
}
