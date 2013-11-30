using ConferenceWebApi.Tools;
using ConferenceWebPack;
using Newtonsoft.Json.Linq;

namespace ConferenceWebApi.DataModel
{
    public class SpeakerRepository: Repository<Speaker>
    {
      
        public SpeakerRepository(JArray jSpeakers)
        {
            foreach (dynamic jspeaker in jSpeakers)
            {
                _Entities.Add((int)jspeaker.id, new Speaker()
                    { 
                        Id = jspeaker.id,
                        Name = jspeaker.name,
                        Bio = jspeaker.bio,
                        ImageUrl = jspeaker.image_url
                    });    
            }
            
        }
        
    }
}