namespace ConferenceWebApi.DataModel
{
    public class Session
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int SpeakerId { get; set; }
        public int Dayno { get; set; }
        public int Timeslot { get; set; }
        public string TimeslotDescription { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public int ResponseCount { get; set; }
    }
}