using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WebApiContrib.CollectionJson;
using ndc.LinkTypes;
using ndc.Model;
using ndc.Tools;

namespace ndc.Controllers
{
    [RoutePrefix("days")]
    public class DaysController : ApiController
    {
        private readonly DataService _dataService = new DataService();

        [Route("", Name = Links.GetAllDays)]
        public HttpResponseMessage Get()
        {

            var duration = _dataService.ConferenceEnd - _dataService.ConferenceStart;

            var daysCollection = new Collection();
            for (int i = 1; i < duration.TotalDays; i++)
            {
               
                    var item = new Item();

                    item.Data.Add(new Data { Name = "Day", Value = i.ToString() });
                    item.Links.Add(Request.ResolveLink<EventsLink>(Links.GetEventsByDay, new { dayid = i }).ToCJLink());
                    item.Links.Add(Request.ResolveLink<EventsLink>(Links.GetSpeakersByDay, new { dayid = i }).ToCJLink());
                    item.Links.Add(Request.ResolveLink<TopicsLink>(Links.GetTopicsByDay, new { dayid = i }).ToCJLink());
                    daysCollection.Items.Add(item);
                  
            }
          

            return new HttpResponseMessage()
            {
                Content = new CollectionJsonContent(daysCollection)
            };
        }
    }
}
