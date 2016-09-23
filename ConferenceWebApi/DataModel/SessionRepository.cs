using System;
using System.Collections.Generic;
using System.Linq;
using ConferenceWebApi.Tools;
using ConferenceWebPack;
using Newtonsoft.Json.Linq;

namespace ConferenceWebApi.DataModel
{
    public class SessionRepository : Repository<Session>
    {
        public SessionRepository(JArray sessions)
        {

            foreach (dynamic session in sessions)
            {
                _Entities.Add((int)session.id, new Session()
                    {
                        Id = session.id,
                        Dayno = session.dayno,
                        Title = session.title,
                        SpeakerId = session.speakerId,
                        Description = session.description,
                        TimeslotDescription = session.timeslots
                    });
            }
            
        }

        public List<Session> GetSessionsByDay(int  dayno)
        {
            return _Entities.Values.Where(e => e.Dayno == dayno).ToList();
        }

    }
}