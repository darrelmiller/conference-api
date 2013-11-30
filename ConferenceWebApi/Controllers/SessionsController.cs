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

        [Route("", Name = Links.GetAllSessions)]
         public HttpResponseMessage Get()
         {
             
             var events = _dataService.SessionRepository.GetAll();
             var eventsCollection = GetCollection(events);

            return new HttpResponseMessage() {
                Content = new CollectionJsonContent(eventsCollection)
            };
        }

       

        [Route("", Name = Links.GetSessionsBySpeaker)]
        public HttpResponseMessage GetEventsBySpeaker(int speakerId)
        {
            var events = _dataService.SessionRepository.GetAll().Where(e => e.SpeakerId == speakerId);
            var eventsCollection = GetCollection(events);

            return new HttpResponseMessage()
            {
                Content = new CollectionJsonContent(eventsCollection)
            };
        }


        [Route("", Name = Links.GetSessionsByDay)]
        public HttpResponseMessage GetEventsByDay(int dayno)
        {
            
            var events = _dataService.SessionRepository.GetAll().Where(e => e.Dayno == dayno);
            var eventsCollection = GetCollection(events);

            return new HttpResponseMessage()
            {
                Content = new CollectionJsonContent(eventsCollection)
            };
        }

        [Route("", Name = Links.GetSessionsByTopic)]
        public HttpResponseMessage GetEventsByTopic(int topicid)
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
                item.Links.Add(Request.ResolveLink<SessionLink>(Links.GetSessionById, new { id = session.Id }).ToCJLink());
                item.Links.Add(Request.ResolveLink<SpeakerLink>(Links.GetSpeakerById, new { id = session.SpeakerId }).ToCJLink());
                eventsCollection.Items.Add(item);
            }
            return eventsCollection;
        }
    }
}
