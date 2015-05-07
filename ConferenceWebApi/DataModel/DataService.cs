using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace ConferenceWebApi.DataModel
{
    public class DataService
    {
        public DateTime ConferenceStart { get; set; }
        public DateTime ConferenceEnd { get; set; }
        public SessionRepository SessionRepository { get; private set; }
        public SpeakerRepository SpeakerRepository { get; private set; }
        public TopicRepository TopicRepository { get; private set; }
        public SessionTopicRepository SessionTopicRepository { get; private set; }
        public DataService()
        {
            ConferenceStart = DateTime.Parse("2013/11/04");
            ConferenceEnd = DateTime.Parse("2013/11/06");

            TopicRepository = new TopicRepository(LoadJsonArray("topics.json"));
            SessionRepository = new SessionRepository(LoadJsonArray("sessions.json"));
            SpeakerRepository = new SpeakerRepository(LoadJsonArray("speakers.json"));
            SessionTopicRepository = new SessionTopicRepository(LoadJsonArray("sessiontopics.json"));
        }

        public int TotalDays
        {
            get
            {
                return (int)(ConferenceEnd - ConferenceStart).TotalDays + 1;
            }
        }
        private JArray LoadJsonArray(string jsonDataFile)
        {
            var resourceStream = this.GetType().Assembly.GetManifestResourceStream(this.GetType(), jsonDataFile);
            var jsonString  = new StreamReader(resourceStream).ReadToEnd();
            return JArray.Parse(jsonString);
        }
    }
}