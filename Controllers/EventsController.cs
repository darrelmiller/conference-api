using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using WebApiContrib.CollectionJson;
using ndc.LinkTypes;
using ndc.Model;
using ndc.Tools;

namespace ndc.Controllers
{
    [RoutePrefix("events")]
    public class EventsController : ApiController
    {
    
        private readonly DataService _dataService = new DataService();

         [Route("", Name = Links.GetAllEvents)]
         public HttpResponseMessage Get()
         {
             
             var events = _dataService.EventRepository.GetAll();
             var eventsCollection = GetCollection(events);

            return new HttpResponseMessage() {
                Content = new CollectionJsonContent(eventsCollection)
            };
        }

       

        [Route("", Name = Links.GetEventsBySpeaker)]
        public HttpResponseMessage GetEventsBySpeaker(int speakerId)
        {
            var events = _dataService.EventRepository.GetAll().Where(e => e.SpeakerId == speakerId);
            var eventsCollection = GetCollection(events);

            return new HttpResponseMessage()
            {
                Content = new CollectionJsonContent(eventsCollection)
            };
        }


        [Route("", Name = Links.GetEventsByDay)]
        public HttpResponseMessage GetEventsByDay(int dayno)
        {
            
            var events = _dataService.EventRepository.GetAll().Where(e => e.Dayno == dayno);
            var eventsCollection = GetCollection(events);

            return new HttpResponseMessage()
            {
                Content = new CollectionJsonContent(eventsCollection)
            };
        }

        [Route("", Name = Links.GetEventsByTopic)]
        public HttpResponseMessage GetEventsByTopic(int topicid)
        {
            var events = _dataService.EventRepository.GetAll().Where(e => true);
            var eventsCollection = GetCollection(events);

            return new HttpResponseMessage()
            {
                Content = new CollectionJsonContent(eventsCollection)
            };
        }


        private Collection GetCollection(IEnumerable<Event> events)
        {
            var eventsCollection = new Collection();

            foreach (var @event in events)
            {
                var item = new Item();

                item.Data.Add(new Data { Name = "Title", Value = @event.Title });
                item.Links.Add(Request.ResolveLink<EventLink>(Links.GetEventById, new { id = @event.SpeakerId }).ToCJLink());
                eventsCollection.Items.Add(item);
            }
            return eventsCollection;
        }
    }
}
