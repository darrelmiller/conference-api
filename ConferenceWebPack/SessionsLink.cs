using System.Net.Http;
using Tavis;
using Tavis.Home;
using Tavis.IANA;

namespace ConferenceWebPack
{
    [LinkRelationType("http://tavis.net/rels/sessions")]
    public class SessionsLink : Link
    {
        public void SetDay(int day)
        {
            SetParameter("dayno",day.ToString());
        }
    }
}