using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using ConferenceWebApi.DataModel;
using ConferenceWebApi.ServerLinks;
using System.Linq;
using NotFoundResult = ConferenceWebApi.Tools.NotFoundResult;


namespace ConferenceWebApi.Controllers
{
    [RoutePrefix("speakers")]
    public class SpeakersController : ApiController
    {

        private readonly DataService _dataService;

        public SpeakersController(DataService dataService)
        {
            _dataService = dataService;
        }

        [Route("", Name = SpeakersLinkHelper.SpeakersSearchRoute)]
        public IHttpActionResult Get()
        {
            var speakers = _dataService.SpeakerRepository.GetAll();
            return SpeakersLinkHelper.CreateResponse(speakers, Request);
        }
     

        [Route("byday")]
        public IHttpActionResult Get(int dayno)
        {
            var speakers = _dataService.SessionRepository.GetSessionsByDay(dayno).Select(s => s.SpeakerId).Distinct().Where(sp=> sp != 0).Select(s => _dataService.SpeakerRepository.Get(s));
            return SpeakersLinkHelper.CreateResponse(speakers, Request);
        }

        [Route("byspeakername")]
        public IHttpActionResult Get(string speakername)
        {
            

            var speakers = _dataService.SpeakerRepository.GetAll().Where(s => s.Name.Contains(speakername)).ToList();
            var count = speakers.Count();
            if (count > 1)
            {
                return SpeakersLinkHelper.CreateResponse(speakers, Request);

            } else if (count == 1)
            {
                var speaker = speakers.First();
                return new RedirectResult(SpeakerLinkHelper.CreateLink(Request, speaker).Target,Request);
            }
            else
            {
                return new NotFoundResult("Speaker not found " + speakername); 
            }
           
        }

       

    }
}
