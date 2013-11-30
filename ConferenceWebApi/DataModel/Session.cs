namespace ConferenceWebPack
{
    public class Session
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int SpeakerId { get; set; }
        public int Dayno { get; set; }
        public int Timeslot { get; set; }
    }
}