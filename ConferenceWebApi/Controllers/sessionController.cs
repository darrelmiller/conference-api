using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using ConferenceWebApi.DataModel;
using ConferenceWebApi.ServerLinks;
using ConferenceWebApi.Tools;
using System.Web.Http.Results;
using System.Threading.Tasks;

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
       public IHttpActionResult Get(int id)
       {
            if (!_dataService.SessionRepository.Exists(id)) return new Tools.NotFoundResult("Session not found");

            var session = _dataService.SessionRepository.Get(id);
            return SessionLinkHelper.CreateResponse(session, Request);
       }

        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            if (!_dataService.SessionRepository.Exists(id)) return new Tools.NotFoundResult("Session not found");

            _dataService.SessionRepository.Delete(id);

            return new OkResult(Request);
        }

       [Route("{id}/topics", Name = TopicsLinkHelper.SessionTopicsRoute)]
       public IHttpActionResult GetTopicsBySession(int id)
       {
            if (!_dataService.SessionRepository.Exists(id)) return new Tools.NotFoundResult("Session not found");

            var topics = _dataService.SessionTopicRepository.GetTopicsBySession(id)
               .Select(st => st.TopicId)
               .Distinct()
               .Select(t => _dataService.TopicRepository.Get(t));

           return TopicsLinkHelper.GetResponse(topics, Request);
       }

        [Route("{id}/rating", Name = "SessionRating")]
        public IHttpActionResult GetRating(int id)
        {
            if (!_dataService.SessionRepository.Exists(id)) return new Tools.NotFoundResult("Session not found");

            var session = _dataService.SessionRepository.Get(id);

            IHttpActionResult response = 
                new OkResult(Request)
                .WithContent(new StringContent(session.Rating.ToString()));

            return response;
        }

        [Route("{id}/rating")]
        public async Task<IHttpActionResult> PostRating(int id, string userId = null, bool isSpeaker = false, string postingDate = null)
        {
            var rating = int.Parse(await Request.Content.ReadAsStringAsync());

            var session = _dataService.SessionRepository.Get(id);
            if (session.ResponseCount == 0)
            {
                session.Rating = rating;
                session.ResponseCount = 1;
            } else
            {
                session.Rating = (session.Rating * session.ResponseCount + rating) / ++session.ResponseCount;
            }

            IHttpActionResult response =
                new OkResult(Request)
                .WithContent(new StringContent(session.Rating.ToString()));

            return response;

        }

    }
}
