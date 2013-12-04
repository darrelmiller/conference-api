using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ConferenceClientLib.DTOs;
using WebApiContrib.CollectionJson;
using WebApiContrib.Formatting.CollectionJson.Client;

namespace ConferenceClientLib.Gen2
{
    // Centralize and decouple URIs
    // Reuse semantic parsing code
    // Use lenient parsing
    // Share HTTPClient

    // Variants on this could be 
    //   derived HttpClient
    //   Extension methods


    /// <summary>
    /// The smart wrapper
    /// </summary>
    public class ConferenceApi
    {
        private const string AllSessions = "sessions";
        private const string AllSpeakers = "speakers";
        private const string SessionsBySpeaker = "sessions?speakerId={speakerid}";
        private const string SessionsBySpeakerName = "sessions?speakerName={speakername}";

        private readonly HttpClient _HttpClient;

        public ConferenceApi()
        {
            _HttpClient = CreateHttpClient();
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
