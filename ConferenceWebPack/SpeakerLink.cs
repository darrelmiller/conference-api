using System.Net.Http;
using Tavis;
using Tavis.Home;
using Tavis.IANA;

namespace ConferenceWebPack
{
    [LinkRelationType("http://tavis.net/rels/speaker")]
    public class SpeakerLink : Link
    {
        public SpeakerLink()
        {
            this.AddHint<AllowHint>(h =>
                {
                    h.AddMethod(HttpMethod.Get);
                    h.AddMethod(HttpMethod.Put);
                    h.AddMethod(HttpMethod.Delete);
                });
            this.AddHint<FormatsHint>(h => h.AddMediaType("text/plain"));
           
        }
    }
}