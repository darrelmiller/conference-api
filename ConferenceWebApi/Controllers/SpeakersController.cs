using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
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
            
            return Request.RespondOk(new CollectionJsonContent(speakersCollection));
        }


        [Route("", Name = Links.SpeakersByDay)]
        public HttpResponseMessage Get(int dayno)
        {

            var speakers = _dataService.SessionRepository.GetSessionsByDay(dayno).Select(s => s.SpeakerId).Distinct().Select(s => _dataService.SpeakerRepository.Get(s));
            var speakersCollection = GetCollection(speakers);

            return Request.RespondOk(new CollectionJsonContent(speakersCollection));
        }

        [Route("", Name = Links.SpeakersByName)]
        public HttpResponseMessage Get(string name)
        {

            var speakers = _dataService.SpeakerRepository.GetAll().Where(s => s.Name.Contains(name)).ToList();
            var count = speakers.Count();
            if (count > 1)
            {
                var speakersCollection = GetCollection(speakers);
                return Request.RespondOk(new CollectionJsonContent(speakersCollection));

            } else if (count == 1)
            {
                var speaker = speakers.First();
                var speakerLink = Request.ResolveLink<SpeakerLink>(Links.SpeakerById, new {id = speaker.Id});
                return Request.RespondSeeOther(speakerLink);
                
            }
            else
            {
                return Request.RespondNotFound("Speaker not found " + name);
            }
           
        }


        private  Collection GetCollection(IEnumerable<Speaker> speakers)
        {
            var collection = new Collection();

            foreach (var speaker in speakers)
            {
                var item = new Item();

                item.Data.Add(new Data {Name = "Name", Value = speaker.Name});
                item.Links.Add(Request.ResolveLink<SpeakerLink>(Links.SpeakerById,new {speaker.Id}).ToCJLink());
                collection.Items.Add(item);
            }
            collection.Href = Request.RequestUri;
            return collection;
        }

       

    }
}
