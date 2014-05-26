using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ConferenceClientLib.DTOs;
using ConferenceWebPack;
using Tavis;
using Tavis.Home;
using WebApiContrib.CollectionJson;
using WebApiContrib.Formatting.CollectionJson.Client;

namespace ConferenceClientLib.Gen3
{
    // Externalize HttpClient
    // Discover URIs from home document


    public class ConferenceApi
    {
        private string AllSessions = "sessions";
        private string AllSpeakers = "speakers";
        private string SessionsBySpeaker = "sessions?speakerId={speakerid}";
        private string SessionsBySpeakerName = "sessions?speakerName={speakername}";

        private readonly HttpClient _HttpClient;
        private HomeDocument _homeDocument;
        public ConferenceApi(HttpClient client)
        {
            _HttpClient = ConfigureHttpClient(client);
        }

        public async static Task<ConferenceApi> CreateConferenceApi(HttpClient client)
        {
            var conferenceApi = new ConferenceApi(client);
            await conferenceApi.DiscoverResources();
            return conferenceApi;
        }

        private async Task DiscoverResources()
        {
            var response = await _HttpClient.GetAsync("/");
            _homeDocument = HomeDocument.Parse(await response.Content.ReadAsStreamAsync());
            AllSpeakers = _homeDocument.GetResource(LinkHelper.GetLinkRelationTypeName<SpeakersLink>()).Target.OriginalString;
            AllSessions = _homeDocument.GetResource(LinkHelper.GetLinkRelationTypeName<SessionsLink>()).Target.OriginalString;

        } 

        public async Task<List<SpeakerDTO>> GetAllSpeakers()
        {
            var response = await _HttpClient.GetAsync(AllSpeakers);
            response.EnsureSuccessStatusCode();

            var readDocument = await response.Content.ReadAsAsync<ReadDocument>(new[] { new CollectionJsonFormatter() });
            return ParseSpeakers(readDocument);
        }

        private static List<SpeakerDTO> ParseSpeakers(ReadDocument readDocument)
        {
            var speakers = readDocument.Collection.Items.Select(item =>
                new SpeakerDTO
                {
                    Name = item.Data[0].Value
                }).ToList();

            return speakers;
        }

        public async Task<List<SessionDTO>> GetAllSessions()
        {
            var response = await _HttpClient.GetAsync(AllSessions);
            response.EnsureSuccessStatusCode();

            var readDocument = await response.Content.ReadAsAsync<ReadDocument>(new[] { new CollectionJsonFormatter() });
            return ParseSessions(readDocument);
        }

        public async Task<List<SessionDTO>> GetSessionsBySpeaker(int speakerid)
        {

            var response = await _HttpClient.GetAsync(SessionsBySpeaker.Replace("{speakerid}",speakerid.ToString()) );
            response.EnsureSuccessStatusCode();

            var readDocument = await response.Content.ReadAsAsync<ReadDocument>(new[] { new CollectionJsonFormatter() });

            return ParseSessions(readDocument);
            
        }

        public async Task<List<SessionDTO>> GetSessionsBySpeakerName(string speakerName)
        {
            var response = await _HttpClient.GetAsync(SessionsBySpeakerName.Replace("{speakername}", speakerName));
            response.EnsureSuccessStatusCode();

            var readDocument = await response.Content.ReadAsAsync<ReadDocument>(new[] { new CollectionJsonFormatter() });

            return ParseSessions(readDocument);
         
        }

        private static List<SessionDTO> ParseSessions(ReadDocument readDocument)
        {
            var sessions = readDocument.Collection.Items.Select(item =>
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

        private HttpClient ConfigureHttpClient(HttpClient client)
        {
            
            // do all the setup stuff
            client.BaseAddress = new Uri("http://birch:1001/");
            // Setup default headers,
            // Setup security credentials
            return client;
        }
    }
}
