using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ConferenceWebApi.Tools;
using ConferenceWebPack;
using Tavis.IANA;
using Tavis.Home;

namespace ConferenceWebApi.ServerLinks
{
    public static class DaysLinkHelper
    {
        public const string AllDaysRoute = "AllDays";

        public static DaysLink CreateLink(HttpRequestMessage request)
        {
            return request.ResolveLink<DaysLink>(AllDaysRoute);
        }

        public static DaysLink WithHints(this DaysLink link)
        {
            link.AddHint<AllowHint>(h => h.AddMethod(HttpMethod.Get));
            link.AddHint<FormatsHint>(h => h.AddMediaType("application/vnd.collection+json"));
            return link;
        }
    }
}
