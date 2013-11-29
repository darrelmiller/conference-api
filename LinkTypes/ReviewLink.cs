using System.Net.Http;
using Tavis;
using Tavis.Home;
using Tavis.IANA;

namespace ndc.LinkTypes
{
    [LinkRelationType("http://tavis.net/rels/review")]
    public class ReviewLink : Link
    {
        public ReviewLink()
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