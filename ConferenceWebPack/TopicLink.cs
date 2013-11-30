using System.Net.Http;
using Tavis;
using Tavis.Home;
using Tavis.IANA;

namespace ConferenceWebPack
{
    [LinkRelationType("http://tavis.net/rels/topic")]
    public class TopicLink : Link
    {
        public TopicLink()
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