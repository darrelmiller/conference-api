using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ConferenceClientLib.Gen3;
using Xunit;

namespace ConferenceClientConsole
{
    public class Step3
    {
        private HttpClient _httpClient;

        public Step3()
        {
            _httpClient = new HttpClient() {BaseAddress = new Uri("http://birch:1001")};
        }

        [Fact]
        public async Task GetAllSpeakers()
        {
            var api = await ConferenceApi.CreateConferenceApi(_httpClient);
            var speakers = await api.GetAllSpeakers();

        }

        [Fact]
        public async Task GetAllSessions()
        {

            var api = new ConferenceApi(_httpClient);
            var sessions = await api.GetAllSessions();

        }
       
        [Fact]
        public async Task GetSessionsBySpeaker()
        {

            var api = new ConferenceApi(_httpClient);
            var sessions = await api.GetSessionsBySpeaker(34);

        }

        [Fact]
        public async Task MakeMultipleRequests()
        {

            var api = new ConferenceApi(_httpClient);
            var speakers = await api.GetAllSpeakers();
            foreach (var speakerDto in speakers)
            {
                var sessions = await api.GetSessionsBySpeakerName(speakerDto.Name);    
            }
            

        }
    }
}
