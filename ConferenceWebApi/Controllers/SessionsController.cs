using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using ConferenceWebApi.DataModel;
using ConferenceWebApi.ServerLinks;
using ConferenceWebApi.Tools;
using ConferenceWebPack;
using CollectionJson;
using System.Linq.Expressions;
using System;

namespace ConferenceWebApi.Controllers
{
    [RoutePrefix("sessions")]
    public class SessionsController : ApiController
    {

        private readonly DataService _dataService;

        public SessionsController(DataService dataService)
        {
            _dataService = dataService;
        }

        [Route("", Name = SessionsLinkHelper.SessionsSearchRoute)]
        public IHttpActionResult Get(string speakerName = null, string dayno = null, string keyword = null)
        {
            // Filter parameters are currently ignored

            var sessions = _dataService.SessionRepository.GetAll();

            return SessionsLinkHelper.CreateResponse(sessions, _dataService, Request);
        }

        [Route("byspeakerid")] //Links.SessionsSearch
        public IHttpActionResult GetSessionsBySpeaker(int speakerId)
        {
            var sessions = _dataService.SessionRepository.GetAll().Where(e => e.SpeakerId == speakerId);

            return SessionsLinkHelper.CreateResponse(sessions, _dataService, Request);
        }

    }
}
