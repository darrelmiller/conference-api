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
            var homeLink = new HomeLink() { Target = new Uri("http://birch:1001/") };
            var response = await _client.SendAsync(homeLink.CreateRequest());
            var homeDocument = await homeLink.ParseResponse(response);
 
            var speakersLink = homeDocument.GetResource(LinkHelper.GetLinkRelationTypeName<SpeakersLink>()) as SpeakersLink;
            
            var speakersResponse = await _client.SendAsync(speakersLink.CreateRequest());

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
