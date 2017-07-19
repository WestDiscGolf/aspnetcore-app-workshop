using System.Collections.Generic;

namespace BackEnd.Data
{
    public class Attendee : ConferenceDTO.Attendee
    {
        public virtual ICollection<ConferenceAttendee> ConferenceAttendees { get; set; }

        //public virtual ICollection<Session> Sessions { get; set; }

        public virtual ICollection<SessionAttendee> SessionsAttendees { get; set; }
    }


    public class SessionAttendee
    {
        public int SessionID { get; set; }

        public Session Session { get; set; }

        public int AttendeeID { get; set; }

        public Attendee Attendee { get; set; }
    }
}