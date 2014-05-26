using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ConferenceClientLib.DTOs;
using ConferenceWebPack;
using Tavis;
using Tavis.Home;
using WebApiContrib.CollectionJson;
using WebApiContrib.Formatting.CollectionJson.Client;

namespace ConferenceClientLib
{
    public static class LinkExtensions
    {
        public async static Task<Collection> ParseResponseAsync(this SpeakersLink link, HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK) return null;
            if (response.Content == null) return null;
            if (response.Content.Headers.ContentType == null || response.Content.Headers.ContentType.MediaType != "application/vnd.collection+json") return null;

            var readDocument = await response.Content.ReadAsAsync<ReadDocument>(new[] { new CollectionJsonFormatter() });
            return readDocument.Collection;
        }

        public async static Task<Collection> ParseResponseAsync(this SessionsLink link, HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK) return null;
            if (response.Content == null) return null;
            if (response.Content.Headers.ContentType == null || response.Content.Headers.ContentType.MediaType != "application/vnd.collection+json") return null;

            var readDocument = await response.Content.ReadAsAsync<ReadDocument>(new[] { new CollectionJsonFormatter() });
            return readDocument.Collection;
        }



        public  static List<SpeakerDTO> ParseSpeakers(this SpeakersLink link, Collection collection)
        {
          
            var speakers = collection.Items.Select(item =>
                new SpeakerDTO
                {
                    Name = item.Data[0].Value
                }).ToList();

            return speakers;
        }

        public static List<SessionDTO> ParseSessions(this SessionsLink link, Collection collection)
        {
            var sessions = collection.Items.Select(item =>
            {
                var session = new SessionDTO();
                foreach (var data in item.Data)
                {
                    switch (data.Name)
                    {
                        case "Title": session.Title = data.Value; break;
                        case "SpeakerName": session.SpeakerName = data.Value; break;
                    }
                }
                return session;
            }
                ).ToList();
            return sessions;
        }
    }

    public static class HomeExtensions
    {
        public static T GetResource<T>(this HomeDocument homeDocument) where T : Tavis.Link
        {
            return (T)homeDocument.GetResource(LinkHelper.GetLinkRelationTypeName(typeof (T)));
        }
    }
}
