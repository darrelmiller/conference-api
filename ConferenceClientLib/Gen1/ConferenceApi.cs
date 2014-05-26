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
    /// <summary>
    /// The Classic Wrapper API
    /// </summary>
    public class ConferenceApi
    {

        public async Task<List<SpeakerDTO>> GetAllSpeakers()
        {
            using (var httpClient = new HttpClient {BaseAddress = new Uri("http://127.0.0.1:1001")})
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
            using (var httpClient = new HttpClient {BaseAddress = new Uri("http://127.0.0.1:1001")})
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
            using (var httpClient = new HttpClient {BaseAddress = new Uri("http://127.0.0.1:1001")})
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
            using (var httpClient = new HttpClient {BaseAddress = new Uri("http://127.0.0.1:1001")})
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
    }
}


// Can't change the BaseAddress
// Redundantly does setup of HttpClient on every request.
// Error handling is duplicated on each method
// Response interpretation  is duplicated on each method.
// Hardcoded URIs