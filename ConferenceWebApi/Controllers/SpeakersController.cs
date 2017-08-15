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
        public IHttpActionResult Get(string speakerName = null, string dayno = null)
        {
            var speakers = _dataService.SpeakerRepository.GetAll();
            return SpeakersLinkHelper.CreateResponse(speakers, Request);
        }
     
      

    }
}
