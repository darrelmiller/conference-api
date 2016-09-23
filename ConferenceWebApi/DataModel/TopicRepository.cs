using System;
using ConferenceWebApi.Tools;
using ConferenceWebPack;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace ConferenceWebApi.DataModel
{
    public class TopicRepository : Repository<Topic>
    {

        public TopicRepository(JArray jarray)
        {
            foreach (dynamic jObject in jarray)
            {
                _Entities.Add((int)jObject.id, new Topic()
                    {
                        Id = jObject.id,
                        Name = jObject.name
                  
                    });
            }
        }

        internal Topic Create(Topic topic)
        {
            if (topic.Id == 0)
            {
                topic.Id = _Entities.Keys.Max() + 1;  // Please don't ever do this in prod code.
            }
            _Entities.Add(topic.Id, topic);
            return topic;
        }
    }
}