using System;
using System.Net.Http;
using System.Web.Http;
using ConferenceWebApi.DataModel;
using ConferenceWebApi.Tools;
using ConferenceWebPack;
using Tavis;
using Tavis.IANA;

namespace ConferenceWebApi.Controllers
{
     [RoutePrefix("speaker")]
    public class SpeakerController : ApiController
    {
         private readonly DataService _dataService;


         public SpeakerController(DataService dataService)
         {
             _dataService = dataService;
         }

         [Route("{id}",Name = Links.SpeakerById)]
        public HttpResponseMessage GetSpeaker(int id)
        {

            var speakerInfo = _dataService.SpeakerRepository.Get(id);

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(speakerInfo.Name + Environment.NewLine + speakerInfo.Bio)
            };

             // TODO:  Add topics they are presenting on
             // TODO: Add hal variant
            response.Headers.AddLinkHeader(Request.ResolveLink<SessionsLink>(Links.SessionsBySpeaker, new { speakerId = speakerInfo.Id }));
            response.Headers.AddLinkHeader(new IconLink() { Target = new Uri(speakerInfo.ImageUrl) });
            return response;
        }
    }
}
