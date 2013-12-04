using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using ConferenceWebApi.DataModel;
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

        [Route("", Name = Links.AllSessions)]
         public HttpResponseMessage Get()
         {
             
             var sessions = _dataService.SessionRepository.GetAll();
             var collection = GetCollection(sessions);

            return Request.RespondOk(new CollectionJsonContent(collection));
        }


        [Route("", Name = Links.SessionsBySpeaker)]
        public HttpResponseMessage GetSessionsBySpeaker(int speakerId)
        {
            var sessions = _dataService.SessionRepository.GetAll().Where(e => e.SpeakerId == speakerId);
            var collection = GetCollection(sessions);

            return Request.RespondOk(new CollectionJsonContent(collection));
        }

        [Route("", Name = Links.SessionsBySpeakerName)]
        public HttpResponseMessage GetSessionsBySpeakerName(string speakername)
        {
            var speaker = _dataService.SpeakerRepository.GetAll().FirstOrDefault(s => s.Name == speakername);
            if (speaker == null) return Request.RespondNotFound("Unknown speaker - " + speakername);

            var sessions = _dataService.SessionRepository.GetAll().Where(e => e.SpeakerId == speaker.Id);
            var collection = GetCollection(sessions);

            return Request.RespondOk(new CollectionJsonContent(collection));
        }

        [Route("", Name = Links.SessionsByDay)]
        public HttpResponseMessage GetSessionsByDay(int dayno)
        {
            var sessions = _dataService.SessionRepository.GetSessionsByDay(dayno).ToList();
            var collection = GetCollection(sessions);

            return Request.RespondOk(new CollectionJsonContent(collection));
        }


        [Route("", Name = Links.SessionsByKeyword)]
        public HttpResponseMessage GetSessionsByKeyword(string keyword)
        {

            var sessions = _dataService.SessionRepository.GetAll().Where(e => e.Description.Contains(keyword));
            var collection = GetCollection(sessions);

            return Request.RespondOk(new CollectionJsonContent(collection));
        }

        [Route("", Name = Links.SessionsByTopic)]
        public HttpResponseMessage GetSessionsByTopic(int topicid)
        {
            var sessiontopics = _dataService.SessionTopicRepository.GetAll().Where(s => s.TopicId == topicid);
            var sessions = sessiontopics.Select(sessiontopic => _dataService.SessionRepository.Get(sessiontopic.SessionId)).ToList();

            var collection = GetCollection(sessions);

            return Request.RespondOk(new CollectionJsonContent(collection));
        }


        private Collection GetCollection(IEnumerable<Session> sessions)
        {
            var eventsCollection = new Collection();

            foreach (var session in sessions)
            {
                var item = new Item();

                item.Data.Add(new Data { Name = "Title", Value = session.Title });
                if (session.SpeakerId != 0)
                {
                    item.Data.Add(new Data
                        {
                            Name = "Speaker",
                            Value = _dataService.SpeakerRepository.Get(session.SpeakerId).Name
                        });
                }
                

                item.Links.Add(Request.ResolveLink<SessionLink>(Links.SessionById, new { id = session.Id }).ToCJLink());
                item.Links.Add(Request.ResolveLink<SpeakerLink>(Links.SpeakerById, new { id = session.SpeakerId }).ToCJLink());
                item.Links.Add(Request.ResolveLink<TopicsLink>(Links.TopicsBySession, new { sessionid = session.Id }).ToCJLink());
                eventsCollection.Items.Add(item);
            }
            return eventsCollection;
        }
    }
}
