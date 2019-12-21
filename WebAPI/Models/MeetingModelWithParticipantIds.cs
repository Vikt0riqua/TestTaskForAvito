using System;
using System.Collections.Generic;

namespace WebAPI.Models
{
    public class MeetingModelWithParticipantIds : MeetingModel
    {
        public List<int> MeetingParticipantsId { get; set; }
    }
}