using System.Net.Http;
using System.Web.Http;
using ConferenceWebApi.DataModel;
using ConferenceWebApi.Tools;
using ConferenceWebPack;
using WebApiContrib.CollectionJson;

namespace ConferenceWebApi.Controllers
{
    [RoutePrefix("days")]
    public class DaysController : ApiController
    {
        private readonly DataService _dataService;

        public DaysController(DataService dataService)
        {
            _dataService = dataService;
        }


        [Route("", Name = Links.AllDays)]
        public HttpResponseMessage Get()
        {

            var duration = _dataService.ConferenceEnd - _dataService.ConferenceStart;

            var daysCollection = new Collection();
            for (int i = 1; i < duration.TotalDays; i++)
            {
               
                    var item = new Item();

                    item.Data.Add(new Data { Name = "Day", Value = i.ToString() });
                    item.Links.Add(Request.ResolveLink<SessionsLink>(Links.SessionsByDay, new { dayid = i }).ToCJLink());
                    item.Links.Add(Request.ResolveLink<SessionsLink>(Links.SpeakersByDay, new { dayid = i }).ToCJLink());
                //    item.Links.Add(Request.ResolveLink<TopicsLink>(Links.TopicsByDay, new { dayid = i }).ToCJLink());
                    daysCollection.Items.Add(item);
                  
            }
          

            return new HttpResponseMessage()
            {
                Content = new CollectionJsonContent(daysCollection)
            };
        }
    }
}
