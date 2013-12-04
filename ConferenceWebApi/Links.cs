namespace ConferenceWebApi
{
    public static class Links
    {

        // SessionsLink
        public const string AllSessions = "AllSessions";
        public const string SessionsBySpeaker = "SessionsBySpeaker";
        public const string SessionsByDay = "SessionsByDay";
        public const string SessionsByKeyword = "SessionsByKeyword";
        public const string SessionsByTopic = "SessionsByTopic";
        public const string SessionsBySpeakerName = "SessionsBySpeakerName";

        // SpeakersLink
        public const string AllSpeakers = "AllSpeakers";
        public const string SpeakersByTopic = "SpeakersByTopic";
        public const string SpeakersByDay = "SpeakersByDay";
        public const string SpeakersByName = "SpeakersByName";
        
        // TopicsLinks
        public const string AllTopics = "AllTopics";
        public const string TopicsBySession = "TopicsBySession";
        public const string TopicsByDay = "TopicsByDay";
        
        // ReviewsLinks
        public const string AllReviews = "AllReviews";
        public const string ReviewsBySession = "ReviewsBySession";
        public const string ReviewsBySpeaker = "ReviewsBySpeaker";
        
        //DaysLinks
        public const string AllDays = "AllDays";

        // Speaker Links
        public const string SpeakerById = "SpeakerById";

        // Topic Links
        public const string TopicById = "TopicById";

        // Session Links
        public const string SessionById = "SessionById";

        // Review Link
        public const string ReviewById = "ReviewById";

        public const string DocsByResourceClass = "DocsByResourceClass";
    }
}
