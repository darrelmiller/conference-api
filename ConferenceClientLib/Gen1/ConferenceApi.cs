using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConferenceClientLib.DTOs;
using Newtonsoft.Json.Linq;
using WebApiContrib.CollectionJson;
using WebApiContrib.Formatting.CollectionJson.Client;


namespace ConferenceClientLib.Gen1
{
    public class ConferenceApi
    {

        public async Task<List<SpeakerDTO>> GetAllSpeakers()
        {
            using (var httpClient = CreateHttpClient())
            {
                var response = await httpClient.GetAsync("speakers");
                response.EnsureSuccessStatusCode();
                
                var readDocument = await response.Content.ReadAsAsync<ReadDocument>(new []{new CollectionJsonFormatter() });
                var speakers = readDocument.Collection.Items.Select(item => 
                    new SpeakerDTO
                    {
                        Name = item.Data[0].Value
                    }).ToList();

                return speakers;
            }
        }

        public async Task<List<SessionDTO>> GetAllSessions()
        {
            using (var httpClient = CreateHttpClient())
            {
                var response = await httpClient.GetAsync("sessions");
                response.EnsureSuccessStatusCode();

                var readDocument = await response.Content.ReadAsAsync<ReadDocument>(new[] { new CollectionJsonFormatter() });
                
                var sessions = readDocument.Collection.Items.Select(item =>
                    new SessionDTO
                    {
                        Title = item.Data[0].Value
                    }).ToList();

                return sessions;
            }
        }

        public async Task<List<SessionDTO>> GetSessionsBySpeaker(int speakerid)
        {
            using (var httpClient = CreateHttpClient())
            {
                var response = await httpClient.GetAsync("sessions?speakerId=" + speakerid);
                response.EnsureSuccessStatusCode();

                var readDocument = await response.Content.ReadAsAsync<ReadDocument>(new[] { new CollectionJsonFormatter() });

                var sessions = readDocument.Collection.Items.Select(item =>
                    new SessionDTO
                    {
                        Title = item.Data[0].Value,
                        SpeakerName = item.Data[1].Value
                    }).ToList();

                return sessions;
            }
        }


        public async Task<List<SessionDTO>> GetSessionsBySpeakerName(string speakerName)
        {
            using (var httpClient = CreateHttpClient())
            {
                var response = await httpClient.GetAsync("sessions?speakerName=" + speakerName);
                response.EnsureSuccessStatusCode();

                var readDocument = await response.Content.ReadAsAsync<ReadDocument>(new[] { new CollectionJsonFormatter() });

                var sessions = readDocument.Collection.Items.Select(item =>
                    new SessionDTO
                    {
                        Title = item.Data[0].Value,
                        SpeakerName = item.Data[1].Value
                    }).ToList();

                return sessions;
            }
        }
        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            // do all the setup stuff
            client.BaseAddress = new Uri("http://birch:1001/");
            // Setup default headers,
            // Setup security credentials
            return client;
        }

       }
}
