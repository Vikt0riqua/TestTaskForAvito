using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Services;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MeetingsController : ControllerBase
    {
        private readonly MeetingService _meetingService;
        public MeetingsController(MeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        // GET:Meetings
        [HttpGet]
        public async Task<ActionResult<List<MeetingModel>>> Get()
        {
            var meetings = await _meetingService.Get();
            return meetings.Select(CreateMeetingModel).ToList();
        }

        // POST: Meetings
        [HttpPost]
        public async Task<ActionResult<MeetingModel>> Post(MeetingModelWithParticipantIds meetingModel)
        {
            if (meetingModel == null)
            {
                return BadRequest();
            }
            var meeting = new Meeting()
            {
                Name = meetingModel.Name,
                StartDateTime = meetingModel.StartDateTime,
                Duration = meetingModel.Duration
            };
            meeting = await _meetingService.AddMeeting(meeting, meetingModel.MeetingParticipantsId);
            return Ok(CreateMeetingModel(meeting));
        }

        // DELETE:Meetings/5
        [HttpDelete("id")]
        public async Task<ActionResult<MeetingModel>> Delete(int id)
        {
            var res = await _meetingService.DeleteMeeting(id);
            if (res == int.MinValue)
            {
                return Ok();
            }
            return Ok(res);
        }

        // POST: Meetings/5/Participants
        [HttpPost("id")]
        public async Task<ActionResult<MeetingModel>> PostParticipants(int id, [FromBody] List<int> participantsId)
        {
            var res = await _meetingService.AddParticipantsForMeeting(id, participantsId);
            return Ok(CreateMeetingModel(res));
        }

        // DELETE:Meetings/5/Participant/1
        [HttpDelete("meetingId/{participant}/participantId")]
        public async Task<ActionResult<MeetingModel>> DeleteParticipant(int meetingId, int participantId)
        {
            var res = await _meetingService.DeleteParticipantFromMeeting(meetingId, participantId);
            return Ok(CreateMeetingModel(res));
        }

        private MeetingModel CreateMeetingModel(Meeting meeting)
        {
            var participantModelsList = new List<ParticipantModel>();
            foreach (var meetingParticipant in meeting.MeetingParticipants)
            {
                participantModelsList.Add(new ParticipantModel() { Name = meetingParticipant.Participant.Name, Email = meetingParticipant.Participant.Email });
            }

            var meetingModel = new MeetingModel()
            {
                Name = meeting.Name,
                StartDateTime = meeting.StartDateTime,
                Duration = meeting.Duration,
                MeetingParticipants = participantModelsList
            };
            return meetingModel;
        }
    }
}
