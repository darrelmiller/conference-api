using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ConferenceWebApi.DataModel;
using ConferenceWebApi.ServerLinks;
using ConferenceWebApi.Tools;
using ConferenceWebPack;
using Newtonsoft.Json.Linq;
using Tavis;
using Tavis.IANA;
using System.Web.Http.Results;

namespace ConferenceWebApi.Controllers
{
    [RoutePrefix("topic")]
    public class TopicController : ApiController
    {
        private readonly DataService _dataService;

        public TopicController(DataService dataService)
        {
            _dataService = dataService;
        }

        [Route("{id}", Name = TopicLinkHelper.TopicByIdRoute)]
        public IHttpActionResult GetTopic(int id)
        {
            var topicInfo = _dataService.TopicRepository.Get(id);

            return TopicLinkHelper.CreateResponse(topicInfo, Request);
        }

        [Route("{id}")]
        public async Task<IHttpActionResult> PutTopic(int id)
        {
            var topicInfo = _dataService.TopicRepository.Get(id);

            var newTopic = await Request.Content.ReadAsStringAsync();
            if (Request.Content.Headers.ContentType.MediaType == "application/json")
            {
                newTopic = (string)JValue.Parse(newTopic);
            }

            topicInfo.Name = newTopic;

            return TopicLinkHelper.CreateResponse(topicInfo, Request);
        }


        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            if (!_dataService.TopicRepository.Exists(id)) return new Tools.NotFoundResult("Topic not found");

            _dataService.TopicRepository.Delete(id);

            return new OkResult(Request);
        }


        [Route("{id}/sessions", Name = SessionsLinkHelper.TopicSessionsRoute)]
        public IHttpActionResult GetSessionsByTopic(int id)
        {
            var sessions = _dataService.SessionTopicRepository.GetAll()
                .Where(s => s.TopicId == id)
                .Select(sessiontopic => _dataService.SessionRepository.Get(sessiontopic.SessionId))
                .ToList();

            return SessionsLinkHelper.CreateResponse(sessions, _dataService, Request);
        }

        [Route("{id}/speakers", Name = SpeakersLinkHelper.TopicSpeakersRoute)]
        public IHttpActionResult GetSpeakersByTopic(int id)
        {
            var speakers = _dataService.SessionTopicRepository.GetAll()
                .Where(st => st.TopicId == id)
                .Select(st => _dataService.SessionRepository.Get(st.SessionId))
                .Select(se => se.SpeakerId).Distinct()
                .Where(sp => sp != 0)
                .Select(sp => _dataService.SpeakerRepository.Get(sp))
                .ToList();

            return SpeakersLinkHelper.CreateResponse(speakers, Request);
        }

    }
}
