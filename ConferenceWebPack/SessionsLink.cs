using System.Net.Http;
using Tavis;
using Tavis.Home;
using Tavis.IANA;

namespace ConferenceWebPack
{
    [LinkRelationType("http://tavis.net/rels/sessions")]
    public class SessionsLink : Link
    {
        public SessionsLink()
        {
            this.AddHint<AllowHint>(h =>
                {
                    h.AddMethod(HttpMethod.Get);
                 });
            this.AddHint<FormatsHint>(h => h.AddMediaType("application/collection+json"));
        }
    }
}