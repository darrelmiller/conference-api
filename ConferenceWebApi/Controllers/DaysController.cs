using System;
using System.Net.Http;
using System.Web.Http;
using ConferenceWebApi.DataModel;
using ConferenceWebApi.ServerLinks;
using ConferenceWebApi.Tools;
using ConferenceWebPack;
using CollectionJson;

namespace ConferenceWebApi.Controllers
{

    // Controllers don't have to map to domain model entities

    [RoutePrefix("days")]
    public class DaysController : ApiController
    {
        private readonly DataService _dataService;

        public DaysController(DataService dataService)
        {
            _dataService = dataService;
        }


        // For simple resources, everything can be done inline in the controller method.  
        // As code becomes more complex, you can introduce model and view classes to refactor out the code

        [Route("", Name = DaysLinkHelper.AllDaysRoute)]
        public HttpResponseMessage Get()
        {
            var duration = _dataService.ConferenceEnd - _dataService.ConferenceStart;

            var daysCollection = CreateCollection(duration);

            return new HttpResponseMessage()
            {
                Content = new CollectionJsonContent(daysCollection)
            };
        }


        private Collection CreateCollection(TimeSpan duration)
        {
            var daysCollection = new Collection();
            for (int i = 1; i < duration.TotalDays; i++)
            {
                var item = new Item();

                item.Data.Add(new Data {Name = "Day", Value = i.ToString()});
                item.Links.Add(SessionsLinkHelper.CreateLink(Request, dayno: i).ToCJLink());
                item.Links.Add(SpeakersLinkHelper.CreateLink(Request, dayno: i).ToCJLink());
                item.Links.Add(TopicsLinkHelper.CreateLink(Request, dayno: i).ToCJLink());
                daysCollection.Items.Add(item);
            }
            return daysCollection;
        }
    }
}
