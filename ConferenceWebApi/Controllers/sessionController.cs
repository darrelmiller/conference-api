using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using ConferenceWebApi.DataModel;
using ConferenceWebApi.Tools;
using ConferenceWebPack;
using Newtonsoft.Json.Linq;
using Tavis;

namespace ConferenceWebApi.Controllers
{
   [Route("session/{id}", Name = Links.SessionById)]
    public class SessionController : ApiController
    {
        private readonly DataService _dataService;

       public SessionController(DataService dataService)
       {
           _dataService = dataService;
       }

       public HttpResponseMessage Get(int id)
        {
            var session = _dataService.SessionRepository.Get(id);

            var response = Request.RespondOk();

            if (Request.Headers.Accept.Contains(new MediaTypeWithQualityHeaderValue("application/hal+json")))
            {
                response.Content = CreateHalContent(session);
            }
            else
            {
                response.Content = new StringContent(session.Title + Environment.NewLine + session.Description);
                response.Headers.AddLinkHeader(Request.ResolveLink<SpeakerLink>(Links.SpeakerById, new { id = session.SpeakerId }));
            }
            return response;
        }

       private HttpContent CreateHalContent(Session session)
       {
           dynamic jspeaker = new JObject();
           jspeaker.name = session.Title;
           jspeaker.bio = session.Description;

           dynamic links = new JObject();

           dynamic speakerLink = new JObject();
           speakerLink.href = Request.ResolveLink<SpeakerLink>(Links.SpeakerById, new { id = session.SpeakerId }).Target;
           links[LinkHelper.GetLinkRelationTypeName<SpeakerLink>()] = speakerLink;

           jspeaker["_links"] = links;

           return new DynamicHalContent(jspeaker);
       }

       

    }
}
