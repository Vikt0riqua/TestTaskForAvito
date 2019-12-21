using System;
using System.Collections.Generic;

namespace WebAPI.Models
{
    public class MeetingModelWithParticipantIds
    {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime StartDateTime { get; set; }
            public TimeSpan Duration { get; set; }
            public List<int> MeetingParticipantsId { get; set; }
    }
}