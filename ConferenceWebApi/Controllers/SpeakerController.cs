using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using ConferenceWebApi.DataModel;
using ConferenceWebApi.Tools;
using ConferenceWebPack;
using Newtonsoft.Json.Linq;
using Tavis;
using Tavis.IANA;

namespace ConferenceWebApi.Controllers
{
     [RoutePrefix("speaker")]
    public class SpeakerController : ApiController
    {
         private readonly DataService _dataService;


         public SpeakerController(DataService dataService)
         {
             _dataService = dataService;
         }

         [Route("{id}",Name = Links.SpeakerById)]
        public HttpResponseMessage GetSpeaker(int id)
        {

            var speakerInfo = _dataService.SpeakerRepository.Get(id);

            var response = Request.RespondOk();

             if (Request.Headers.Accept.Contains(new MediaTypeWithQualityHeaderValue("application/hal+json")))
             {
                 response.Content = CreateHalContent(speakerInfo);
             }
             else
             {
                 response.Content = new StringContent(speakerInfo.Name + Environment.NewLine + speakerInfo.Bio);
                 response.Headers.AddLinkHeader(Request.ResolveLink<SessionsLink>(Links.SessionsBySpeaker, new { speakerId = speakerInfo.Id }));
                 response.Headers.AddLinkHeader(new IconLink() { Target = new Uri(speakerInfo.ImageUrl) });
             }

            return response;
        }

         private HttpContent CreateHalContent(Speaker speakerInfo)
         {
             dynamic jspeaker = new JObject();
             jspeaker.name = speakerInfo.Name;
             jspeaker.bio = speakerInfo.Bio;
             
             dynamic links = new JObject();
             
             dynamic iconLink = new JObject();
             iconLink.href = speakerInfo.ImageUrl;
             links.icon = iconLink;

             dynamic sessionsLink = new JObject();
             sessionsLink.href = Request.ResolveLink<SessionsLink>(Links.SessionsBySpeaker, new { speakerId = speakerInfo.Id }).Target;
             links[LinkHelper.GetLinkRelationTypeName<SessionsLink>()] = sessionsLink;

             jspeaker["_links"] = links;
             
            return new DynamicHalContent(jspeaker);
         }
    }
}
