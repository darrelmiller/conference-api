using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using ConferenceWebApi.DataModel;
using ConferenceWebApi.ServerLinks;
using ConferenceWebApi.Tools;
using ConferenceWebPack;
using CollectionJson;
using System.Linq;

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



        [Route("")]
        public IHttpActionResult Get(int dayno)
        {

            var topics = _dataService.SessionRepository.GetSessionsByDay(dayno)
                .SelectMany(s => _dataService.SessionTopicRepository.GetTopicsBySession(s.Id))
                .Select(st => st.TopicId).Distinct().Select(s => _dataService.TopicRepository.Get(s));

            return TopicsLinkHelper.GetResponse(topics, Request);
        }






    }
}
