using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class Participant
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParticipantId { get; set; }
        [Required]
        public string ParticipantName { get; set; }
        [Required]
        public string Email { get; set; }

        public IList<MeetingParticipant> MeetingParticipants { get; set; }

        public Participant()
        {
            MeetingParticipants = new List<MeetingParticipant>();
        }
    }
}