using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using ConferenceWebApi.DataModel;
using ConferenceWebApi.Tools;
using ConferenceWebPack;
using WebApiContrib.CollectionJson;
using System.Linq;

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

        [Route("", Name = Links.AllSpeakers)]
        public HttpResponseMessage Get()
        {

            var speakers = _dataService.SpeakerRepository.GetAll();
            var speakersCollection = GetCollection(speakers);

            return new HttpResponseMessage()
                {
                    Content = new CollectionJsonContent(speakersCollection)
                };
        }


        [Route("", Name = Links.SpeakersByDay)]
        public HttpResponseMessage Get(int dayno)
        {

            var speakers = _dataService.SessionRepository.GetSessionsByDay(dayno).Select(s => s.SpeakerId).Distinct().Select(s => _dataService.SpeakerRepository.Get(s));
            var speakersCollection = GetCollection(speakers);

            return new HttpResponseMessage()
            {
                Content = new CollectionJsonContent(speakersCollection)
            };
        }



        private  Collection GetCollection(IEnumerable<Speaker> speakers)
        {
            var eventsCollection = new Collection();

            foreach (var speaker in speakers)
            {
                var item = new Item();

                item.Data.Add(new Data {Name = "Name", Value = speaker.Name});
                item.Links.Add(Request.ResolveLink<SpeakerLink>(Links.SpeakerById,new {speaker.Id}).ToCJLink());
                eventsCollection.Items.Add(item);
            }
            return eventsCollection;
        }

       

    }
}
