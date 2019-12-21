namespace DataAccess.Models
{
    public class MeetingParticipant
    {
        public int MeetingId { get; set; }
        public Meeting Meeting { get; set; }

        public int ParticipantId { get; set; }
        public Participant Participant { get; set; }
    }
}