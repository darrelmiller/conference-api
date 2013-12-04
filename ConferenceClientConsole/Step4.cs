using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ConferenceClientLib.Gen2;
using ConferenceWebPack;
using Tavis;
using Tavis.Home;
using Xunit;


namespace ConferenceClientConsole
{
    public class Step4
    {
        private HttpClient _client;
        
        public Step4()
        {
            _client = new HttpClient();
           
        }
        [Fact]
        public async Task GetAllSpeakers()
        {
            var homeDocument = await GetHomeDocument();

            // Get the speakers link
            var speakersLink = homeDocument.GetResource<SpeakersLink>();

            // Get the Speakers
            var speakersResponse = await _client.SendAsync(speakersLink.CreateRequest());

            // Interpret the result
            var collection = await speakersLink.ParseResponse(speakersResponse);

            foreach (var item in collection.Items)
            {
                foreach (var data in item.Data)
                {
                    Debug.WriteLine(data.Value);
                }
             
            }
        }

        private async Task<HomeDocument> GetHomeDocument()
        {
            var linkFactory = new LinkFactory();
            ConferenceWebPack.Config.Register(linkFactory);

            var homeLink = linkFactory.CreateLink<HomeLink>();
            homeLink.Target = new Uri("http://birch:1001/");
            var response = await _client.SendAsync(homeLink.CreateRequest());

            var homeDocument = await homeLink.ParseResponse(response, linkFactory);
            return homeDocument;
        }

        [Fact]
        public async Task GetAllSessions()
        {

            var homeDocument = await GetHomeDocument();

            var sessionsLink = homeDocument.GetResource<SessionsLink>();

            sessionsLink.SetDay(1);
            var request = sessionsLink.CreateRequest();
            var sessionsResponse = await _client.SendAsync(request);

            // Interpret the result
            var collection = await sessionsLink.ParseResponse(sessionsResponse);

        }
       
        [Fact]
        public async Task GetSessionsBySpeaker()
        {

            var api = new ConferenceApi();
            var sessions = await api.GetSessionsBySpeaker(34);

        }

     
    }
}
