using System.Net.Http;
using System.Web.Http;
using ConferenceWebApi.DataModel;
using ConferenceWebApi.Tools;
using ConferenceWebPack;
using Tavis;

namespace ConferenceWebApi.Controllers
{
   [Route("session/{id}", Name = Links.GetSessionById)]
    public class SessionController : ApiController
    {
        private readonly DataService _dataService;

       public SessionController(DataService dataService)
       {
           _dataService = dataService;
       }

       public HttpResponseMessage Get(int id)
        {
            var eventInfo = _dataService.SessionRepository.Get(id);


            var response = new HttpResponseMessage()
                {
                    Content = new StringContent(eventInfo.Title)
                };
            response.Headers.AddLinkHeader(Request.ResolveLink<SpeakerLink>(Links.GetSpeakerById, new { id = eventInfo.SpeakerId}));
            return response;
        }

       

    }
}
