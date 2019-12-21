using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class MeetingParticipant
    {
        [Key, Column(Order = 0)]
        public int MeetingId { get; set; }
        public Meeting Meeting { get; set; }

        [Key, Column(Order = 1)]
        public int ParticipantId { get; set; }
        public Participant Participant { get; set; }
    }
}