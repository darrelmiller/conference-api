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
    [RoutePrefix("speakers")]
    public class SpeakersController : ApiController
    {

        private readonly DataService _dataService = new DataService();


        [Route("", Name = Links.GetAllSpeakers)]
        public HttpResponseMessage Get()
        {

            var speakers = _dataService.SpeakerRepository.GetAll();
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

                item.Data.Add(new Data {Name = "Title", Value = speaker.Name});
                item.Links.Add(Request.ResolveLink<SpeakerLink>(Links.GetSpeakerById,new {speaker.Id}).ToCJLink());
                eventsCollection.Items.Add(item);
            }
            return eventsCollection;
        }

       

    }
}
