using System.Net.Http;
using Tavis;
using Tavis.Home;
using Tavis.IANA;

namespace ConferenceWebPack
{
    [LinkRelationType("http://tavis.net/rels/session")]
    public class SessionLink : Link
    {
        public SessionLink() 
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