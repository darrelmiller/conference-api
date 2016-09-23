using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using ConferenceWebApi.DataModel;
using ConferenceWebApi.ServerLinks;
using ConferenceWebApi.Tools;
using ConferenceWebPack;
using CollectionJson;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Newtonsoft.Json.Linq;

namespace ConferenceWebApi.Controllers
{
    [RoutePrefix("topics")]
    public class TopicsController : ApiController
    {

        private readonly DataService _dataService;

        public TopicsController(DataService dataService)
        {
            _dataService = dataService;
        }

        [Route("", Name = TopicsLinkHelper.TopicsSearchRoute)]
        public IHttpActionResult Get()
        {
            var topics = _dataService.TopicRepository.GetAll();
            return TopicsLinkHelper.GetResponse(topics, Request);
        }

        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            if (!_dataService.TopicRepository.Exists(id)) return new Tools.NotFoundResult("Topic not found");

            _dataService.TopicRepository.Delete(id);

            return new OkResult(Request);
        }

        [Route("")]
        public async Task<IHttpActionResult> Post()
        {

            var newTopic = await Request.Content.ReadAsStringAsync();
            if (Request.Content.Headers.ContentType.MediaType == "application/json")
            {
                newTopic = (string)JValue.Parse(newTopic);
            }

            var topic = _dataService.TopicRepository.Create(new Topic() { Name = newTopic });
            
            return TopicLinkHelper.CreateResponse(topic, Request)
                .WithCreatedLocation(TopicLinkHelper.CreateLink(Request, topic).Target);
        }


        [Route("byday")]
        public IHttpActionResult Get(int dayno)
        {

            var topics = _dataService.SessionRepository.GetSessionsByDay(dayno)
                .SelectMany(s => _dataService.SessionTopicRepository.GetTopicsBySession(s.Id))
                .Select(st => st.TopicId).Distinct().Select(s => _dataService.TopicRepository.Get(s));

            return TopicsLinkHelper.GetResponse(topics, Request);
        }






    }
}
