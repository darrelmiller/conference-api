using System;
using System.Collections.Generic;
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
            // Retreive the Home document
            var linkFactory = new LinkFactory();
            ConferenceWebPack.Config.Register(linkFactory);

            var homeLink = linkFactory.CreateLink<HomeLink>();
            homeLink.Target = new Uri("http://birch:1001/");
            var response = await _client.SendAsync(homeLink.CreateRequest());

            var homeDocument = await homeLink.ParseResponse(response, linkFactory);

            // Get the speakers link
            var speakersLink = homeDocument.GetResource(LinkHelper.GetLinkRelationTypeName<SpeakersLink>()) as SpeakersLink;

            // Get the Speakers
            var speakersResponse = await _client.SendAsync(speakersLink.CreateRequest());

            // Interpret the result
            var collection = speakersLink.ParseResponse(speakersResponse);

        }

        [Fact]
        public async Task GetAllSessions()
        {

            var api = new ConferenceApi();
            var sessions = await api.GetAllSessions();

        }
       
        [Fact]
        public async Task GetSessionsBySpeaker()
        {

            var api = new ConferenceApi();
            var sessions = await api.GetSessionsBySpeaker(34);

        }

        [Fact]
        public async Task MakeMultipleRequests()
        {
           
            var api = new ConferenceApi();
            var speakers = await api.GetAllSpeakers();
            foreach (var speakerDto in speakers)
            {
                var sessions = await api.GetSessionsBySpeakerName(speakerDto.Name);    
            }
            

        }
    }
}
