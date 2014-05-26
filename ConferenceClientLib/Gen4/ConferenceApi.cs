using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ConferenceClientLib.DTOs;
using ConferenceWebPack;
using Tavis;
using Tavis.Home;
using WebApiContrib;
using WebApiContrib.CollectionJson;
using WebApiContrib.Formatting.CollectionJson.Client;
using ConferenceClientLib;

namespace ConferenceClientLib.Gen4
{
    public class ConferenceApi
    {

        private readonly HttpClient _HttpClient;
        private HomeDocument _homeDocument;

        public SpeakersLink SpeakersLink { get; private set; }
        public SessionsLink SessionsLink { get; private set; }
        public SessionLink SessionLink { get; private set; }
        

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
            SpeakersLink = _homeDocument.GetResource<SpeakersLink>();
            SessionsLink = _homeDocument.GetResource<SessionsLink>();
        }

        public async Task<List<SpeakerDTO>> GetAllSpeakers()
        {
            
            var speakersResponse = await _HttpClient.SendAsync(SpeakersLink.BuildRequestMessage());
            speakersResponse.EnsureSuccessStatusCode();
            WebApiContrib.CollectionJson.Collection collection = await SpeakersLink.ParseResponseAsync(speakersResponse);
            return SpeakersLink.ParseSpeakers(collection);
        }

        public async Task<List<SessionDTO>> GetAllSessions()
        {
            //SessionsLink.SetDay(1);
            var sessionsResponse = await _HttpClient.SendAsync(SessionsLink.BuildRequestMessage());
            sessionsResponse.EnsureSuccessStatusCode();
            var collection = await SessionsLink.ParseResponseAsync(sessionsResponse);
            return SessionsLink.ParseSessions(collection);
        }

        public async Task<List<SessionDTO>> GetSessionsBySpeaker(int speakerid)
        {
            var sessionsResponse = await _HttpClient.SendAsync(SessionLink.BuildRequestMessage(new Dictionary<string, object> {{"speakerid",speakerid}}));
            sessionsResponse.EnsureSuccessStatusCode();
            var collection = await SessionsLink.ParseResponseAsync(sessionsResponse);
            return SessionsLink.ParseSessions(collection);

        }

        public async Task<List<SessionDTO>> GetSessionsBySpeakerName(string speakerName)
        {
            var response = await _HttpClient.SendAsync(SessionsLink.BuildRequestMessage(new Dictionary<string, object> { { "{speakername}", speakerName } }));
            response.EnsureSuccessStatusCode();
            
            var collection = await SessionsLink.ParseResponseAsync(response);
            return SessionsLink.ParseSessions(collection);

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
