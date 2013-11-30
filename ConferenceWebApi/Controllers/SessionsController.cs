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
             
             var events = _dataService.SessionRepository.GetAll();
             var eventsCollection = GetCollection(events);

            return new HttpResponseMessage() {
                Content = new CollectionJsonContent(eventsCollection)
            };
        }

       

        [Route("", Name = Links.SessionsBySpeaker)]
        public HttpResponseMessage GetSessionsBySpeaker(int speakerId)
        {
            var events = _dataService.SessionRepository.GetAll().Where(e => e.SpeakerId == speakerId);
            var eventsCollection = GetCollection(events);

            return new HttpResponseMessage()
            {
                Content = new CollectionJsonContent(eventsCollection)
            };
        }


        [Route("", Name = Links.SessionsByDay)]
        public HttpResponseMessage GetSessionsByDay(int dayno)
        {
            
            var sessions = _dataService.SessionRepository.GetSessionsByDay(dayno).ToList();
            var collection = GetCollection(sessions);

            return new HttpResponseMessage()
            {
                Content = new CollectionJsonContent(collection)
            };
        }

        [Route("", Name = Links.SessionsByTopic)]
        public HttpResponseMessage GetSessionsByTopic(int topicid)
        {
            var events = _dataService.SessionRepository.GetAll().Where(e => true);
            var eventsCollection = GetCollection(events);

            return new HttpResponseMessage()
            {
                Content = new CollectionJsonContent(eventsCollection)
            };
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
                eventsCollection.Items.Add(item);
            }
            return eventsCollection;
        }
    }
}
