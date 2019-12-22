using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class Meeting
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MeetingId { get; set; }
        [Required]
        public string MeetingName { get; set; }
        [Required]
        public DateTime StartDateTime { get; set; }
        [Required]
        public DateTime EndDateTime { get; set; }

        public IList<MeetingParticipant> MeetingParticipants { get; set; }

        public Meeting()
        {
            MeetingParticipants = new List<MeetingParticipant>();
        }
    }
}