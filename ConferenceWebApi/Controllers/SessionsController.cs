using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using ConferenceWebApi.DataModel;
using ConferenceWebApi.ServerLinks;
using ConferenceWebApi.Tools;
using ConferenceWebPack;
using WebApiContrib.CollectionJson;

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
         public IHttpActionResult Get()
         {
             
             var sessions = _dataService.SessionRepository.GetAll();

            return SessionsLinkHelper.CreateResponse(sessions, _dataService, Request);
        }

        [Route("")] //Links.SessionsSearch
        public IHttpActionResult GetSessionsBySpeaker(int speakerId)
        {
            var sessions = _dataService.SessionRepository.GetAll().Where(e => e.SpeakerId == speakerId);

            return SessionsLinkHelper.CreateResponse(sessions, _dataService, Request);
        }

        [Route("")] //Links.SessionsSearch
        public IHttpActionResult GetSessionsBySpeakerName(string speakername)
        {
            var speaker = _dataService.SpeakerRepository.GetAll().FirstOrDefault(s => s.Name == speakername);
            if (speaker == null) return new NotFoundResult("Unknown speaker - " + speakername);

            var sessions = _dataService.SessionRepository.GetAll().Where(e => e.SpeakerId == speaker.Id);

            return SessionsLinkHelper.CreateResponse(sessions, _dataService, Request);
        }

        [Route("")] //Links.SessionsSearch
        public IHttpActionResult GetSessionsByDay(int dayno)
        {
            if (dayno > _dataService.TotalDays)
            {
                return new BadRequestResult(new Tavis.ProblemDocument()
                {
                    ProblemType = new System.Uri("urn:conference:invalid-day"),
                    Title = string.Format("Day {0}  is after the end of the conference",dayno)
                });
            }
            var sessions = _dataService.SessionRepository.GetSessionsByDay(dayno).ToList();

            return SessionsLinkHelper.CreateResponse(sessions, _dataService, Request);
        }

        
        [Route("")] //Links.SessionsSearch
        public IHttpActionResult GetSessionsByKeyword(string keyword)
        {

            var sessions = _dataService.SessionRepository.GetAll().Where(e => e.Description.Contains(keyword));
            // Empty list when not found

            return SessionsLinkHelper.CreateResponse(sessions, _dataService, Request);
        }

 

       
    }
}
