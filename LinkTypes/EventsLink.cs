using System.Net.Http;
using Tavis;
using Tavis.Home;
using Tavis.IANA;

namespace ndc.Controllers
{
    [LinkRelationType("http://tavis.net/rels/events")]
    public class EventsLink : Link
    {
        public EventsLink()
        {
            this.AddHint<AllowHint>(h =>
                {
                    h.AddMethod(HttpMethod.Get);
                 });
            this.AddHint<FormatsHint>(h => h.AddMediaType("application/collection+json"));
        }
    }
}