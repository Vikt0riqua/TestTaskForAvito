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
        public string Name { get; set; }
        [Required]
        public DateTime StartDateTime { get; set; }
        public TimeSpan Duration { get; set; }

        public List<MeetingParticipant> MeetingParticipants { get; set; }
    }
}