using ConferenceWebApi.Tools;
using ConferenceWebPack;
using Newtonsoft.Json.Linq;

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

    }
}