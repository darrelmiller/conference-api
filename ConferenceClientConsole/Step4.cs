using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ConferenceClientLib.Gen2;
using ConferenceWebPack;
using Xunit;

namespace ConferenceClientConsole
{
    public class Step4
    {
        [Fact]
        public async Task GetAllSpeakers()
        {

            var client = new HttpClient();
            var link = new SpeakersLink()
            {
                
            };


            var request =  link.CreateRequest();

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
