using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using ConferenceWebApi.DataModel;
using ConferenceWebApi.ServerLinks;


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

        [Route("{id}",Name = SpeakerLinkHelper.SpeakerByIdRoute)]
        public IHttpActionResult GetSpeaker(int id)
        {
            var speakerInfo = _dataService.SpeakerRepository.Get(id);

            return SpeakerLinkHelper.CreateResponse(speakerInfo, Request);
        }

    
        [Route("{id}/sessions", Name = SessionsLinkHelper.SpeakerSessionsRoute)]
         public IHttpActionResult GetSessionsBySpeaker(int id)
         {
             var sessions = _dataService.SessionRepository.GetAll().Where(s => s.SpeakerId == id);

             return SessionsLinkHelper.CreateResponse(sessions, _dataService, Request);
            
         }

         [Route("{id}/topics", Name = TopicsLinkHelper.SpeakerTopicsRoute)]
         public IHttpActionResult GetTopicsBySpeaker(int id)
         {
             var topics = _dataService.SessionRepository.GetAll()
                 .Where(s => s.SpeakerId == id)
                 .SelectMany(s => _dataService.SessionTopicRepository.GetTopicsBySession(s.Id))
                 .Select(st => st.TopicId)
                 .Distinct()
                 .Select(t => _dataService.TopicRepository.Get(t));

             return TopicsLinkHelper.GetResponse(topics, Request);
         }

    }
}
