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
    [RoutePrefix("topics")]
    public class TopicsController : ApiController
    {

        private readonly DataService _dataService;

        public TopicsController(DataService dataService)
        {
            _dataService = dataService;
        }

        [Route("", Name = Links.AllTopics)]
        public HttpResponseMessage Get()
        {

            var topics = _dataService.TopicRepository.GetAll();
            var topicsCollection = GetCollection(topics);

            return new HttpResponseMessage()
            {
                Content = new CollectionJsonContent(topicsCollection)
            };
        }


        //[Route("", Name = Links.TopicsByDay)]
        //public HttpResponseMessage Get(int dayno)
        //{

        //    var topics = _dataService.SessionRepository.GetSessionsByDay(dayno); //.Select(s => s.TopicId).Distinct().Select(s => _dataService.TopicRepository.Get(s));
        //    var topicsCollection = GetCollection(topics);

        //    return new HttpResponseMessage()
        //    {
        //        Content = new CollectionJsonContent(topicsCollection)
        //    };
        //}



        private Collection GetCollection(IEnumerable<Topic> topics)
        {
            var eventsCollection = new Collection();

            foreach (var topic in topics)
            {
                var item = new Item();

                item.Data.Add(new Data { Name = "Title", Value = topic.Name });
                item.Links.Add(Request.ResolveLink<TopicLink>(Links.TopicById, new { topic.Id }).ToCJLink());
                eventsCollection.Items.Add(item);
            }
            return eventsCollection;
        }



    }
}
