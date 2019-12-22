namespace DataAccess.Models
{
    public class MeetingParticipant
    {
        public int MId { get; set; }
        public Meeting Meeting { get; set; }
        public int PId { get; set; }
        public Participant Participant { get; set; }
    }
}