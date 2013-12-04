using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ConferenceClientLib.Gen1;
using Xunit;

namespace ConferenceClientConsole
{
    public class Step1
    {
        [Fact]
        public async Task GetAllSpeakers()
        {

            var api = new ConferenceApi();
            var speakers = await api.GetAllSpeakers();

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
                var sessions = await api.GetSessionsBySpeakerName(speakerDto.Name);    // Don't have Id... why not?
            }
            

        }
    }
}
