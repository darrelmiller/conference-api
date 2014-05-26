using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using ConferenceWebApi.DataModel;
using ConferenceWebApi.ServerLinks;


namespace ConferenceWebApi.Controllers
{
   [RoutePrefix("session")]
    public class SessionController : ApiController
    {
        private readonly DataService _dataService;

       public SessionController(DataService dataService)
       {
           _dataService = dataService;
       }

       [Route("{id}", Name = SessionLinkHelper.SessionByIdRoute)]
       public HttpResponseMessage Get(int id)
       {
           var session = _dataService.SessionRepository.Get(id);

           return SessionLinkHelper.CreateResponse(session,Request);
       }

       [Route("{id}/topics", Name = TopicsLinkHelper.SessionTopicsRoute)]
       public IHttpActionResult GetTopicsBySession(int id)
       {
           var topics = _dataService.SessionTopicRepository.GetTopicsBySession(id)
               .Select(st => st.TopicId)
               .Distinct()
               .Select(t => _dataService.TopicRepository.Get(t));

           return TopicsLinkHelper.GetResponse(topics, Request);
       }

       

    }
}
