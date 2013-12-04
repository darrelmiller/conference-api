using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tavis;
using Tavis.Home;

namespace ConferenceWebPack
{

    [LinkRelationType("home")]
    public class HomeLink : Link
    {
        public  async Task<HomeDocument> ParseResponse( HttpResponseMessage response, LinkFactory linkFactory)
        {
            if (response.StatusCode != HttpStatusCode.OK) return null;
            if (response.Content == null) return null;
            if (response.Content.Headers.ContentType == null || response.Content.Headers.ContentType.MediaType != "application/home+json") return null;

            Stream stream = await response.Content.ReadAsStreamAsync();
            return HomeDocument.Parse(stream, linkFactory);


        }
    }
}
