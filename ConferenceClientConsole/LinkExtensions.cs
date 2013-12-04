using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ConferenceWebPack;

using WebApiContrib.CollectionJson;
using WebApiContrib.Formatting.CollectionJson.Client;

namespace ConferenceClientConsole
{
    public static class LinkExtensions
    {
        public async static Task<Collection> ParseResponse(this SpeakersLink link, HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK) return null;
            if (response.Content == null) return null;
            if (response.Content.Headers.ContentType == null || response.Content.Headers.ContentType.MediaType != "application/vnd.collection+json") return null;

            var readDocument = await response.Content.ReadAsAsync<ReadDocument>(new[] { new CollectionJsonFormatter() });
            return readDocument.Collection;
        }
    }
}
