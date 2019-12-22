using System.Collections.Generic;

namespace WebAPI.Models
{
    public class MeetingModelForResponse
    {
        public MeetingModel Meeting { get; set; }
        public List<string> Errors { get; set; }
    }
}