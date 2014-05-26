using System.Collections.Generic;
using System.Linq;
using ConferenceWebApi.Tools;
using Newtonsoft.Json.Linq;

namespace ConferenceWebApi.DataModel
{
    public class SessionTopicRepository : Repository<SessionTopic>
    {

        public SessionTopicRepository(JArray jarray)
        {
            int i = 1;
            foreach (dynamic jObject in jarray)
            {
                _Entities.Add(i++, new SessionTopic()
                {
                    SessionId = jObject.sessionId,
                    TopicId = jObject.topicId

                });
            }



        }

        public List<SessionTopic> GetTopicsBySession(int sessionId)
        {
            return _Entities.Values.Where(e => e.SessionId == sessionId).ToList();
        }
        public List<SessionTopic> GetSessionsByTopic(int topicId)
        {
            return _Entities.Values.Where(e => e.TopicId == topicId).ToList();
        }
    }
}