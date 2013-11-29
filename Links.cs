using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ndc
{
    public static class Links
    {

        public const string GetAllDays = "GetAllDays";
        public const string GetAllSpeakers = "GetAllSpeakers";
        public const string GetAllTopics = "GetAllTopics";
        public const string GetAllEvents = "GetAllEvents";
        public const string GetAllReviews = "GetAllReviews";

      
        public const string GetSpeakerById = "GetSpeakerById";
        public const string GetTopicById = "GetTopicById";
        public const string GetEventById = "GetEventById";
        public const string GetReviewById = "GetReviewById";

        public const string GetEventsBySpeaker = "GetEventsBySpeaker";
        public const string GetEventsByDay = "GetEventsByDay";
        public const string GetEventsByTopic = "GetEventsByTopic";

        public const string GetSpeakersByTopic = "GetSpeakersByTopic";
        public const string GetSpeakersByDay = "GetSpeakersByDay";

        public const string GetTopicsByEvent = "GetTopicsByEvent";
        public const string GetTopicsByDay = "GetTopicsByDay";
        
        public const string GetReviewsByEvent = "GetReviewsByEvent";
        public const string GetReviewsBySpeaker = "GetReviewsBySpeaker";
        
        public const string DocsByResourceClass = "DocsByResourceClass";
    }
}
