using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ndc.Model
{
    public class DataService
    {
        public DateTime ConferenceStart { get; set; }
        public DateTime ConferenceEnd { get; set; }
        public EventRepository EventRepository { get; private set; }
        public SpeakerRepository SpeakerRepository { get; private set; }

        public DataService()
        {
            EventRepository = new EventRepository();
            SpeakerRepository = new SpeakerRepository();
        }
    }
    
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int SpeakerId { get; set; }
        public int Dayno { get; set; }
        public int Timeslot { get; set; }
    }

    public class EventTopic
    {
        public int EventId { get; set; }
        public int TopicId { get; set; }
    }

    public class Speaker
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Topic
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Review
    {
        public string Id { get; set; }
        public string Attendee { get; set; }
        public string Comments { get; set; }
        public int Score { get; set; }

    }
    public class EventRepository : Repository<Event>
    {
        public EventRepository()
        {
            _Entities.Add(1, new Event(){Title="ScriptCS thing", SpeakerId = 1});
        }

        public List<Event> GetEventsByDay(int  dayno)
        {
            return _Entities.Values.Where(e => e.Dayno == dayno).ToList();
        }
    }

    public class SpeakerRepository: Repository<Speaker>
    {
      
        public SpeakerRepository()
        {
            _Entities.Add(1, new Speaker() {Name = "Glenn Block"});
        }
        
    }

    public class Repository<T>
    {
        protected Dictionary<int, T> _Entities = new Dictionary<int, T>();
        

        public IEnumerable<T> GetAll()
        {
            return _Entities.Values;
        }
        public T Get(int id)
        {
            return _Entities[id];
        }
    }
}
