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
                EndDateTime = meetingModel.EndDateTime
            };
            meeting = await _meetingService.AddMeeting(meeting, meetingModel.MeetingParticipantsId);
            return Ok(CreateMeetingModel(meeting));
        }

        // DELETE:Meetings/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<MeetingModel>> Delete(int id)
        {
            var res = await _meetingService.DeleteMeeting(id);
            if (res == int.MinValue)
            {
                return BadRequest();
            }
            return Ok(res);
        }

        // POST: Meetings/5/Participants
        [HttpPost]
        [Route("{id}/Participants")]
        public async Task<ActionResult<MeetingModel>> PostParticipants(int id, [FromBody] MeetingParticipants meetingParticipants)
        {
            if (meetingParticipants.MeetingParticipantsId == null || !meetingParticipants.MeetingParticipantsId.Any()) return BadRequest();
            var res = await _meetingService.AddParticipantsForMeeting(id, meetingParticipants.MeetingParticipantsId);
            return Ok(CreateMeetingModel(res));
        }

        // DELETE:Meetings/5/Participant/1
        [HttpDelete]
        [Route("{meetingId}/Participants/{participantId}")]
        public async Task<ActionResult<MeetingModel>> DeleteParticipant(int meetingId, int participantId)
        {
            var res = await _meetingService.DeleteParticipantFromMeeting(meetingId, participantId);
            return Ok(CreateMeetingModel(res));
        }

        private static MeetingModel CreateMeetingModel(Meeting meeting)
        {
            var participantModelsList = meeting.MeetingParticipants?.Select(meetingParticipant => new ParticipantModel() { Name = meetingParticipant.Participant.Name, Email = meetingParticipant.Participant.Email }).ToList();

            var meetingModel = new MeetingModel()
            {
                Id = meeting.MeetingId,
                Name = meeting.Name,
                StartDateTime = meeting.StartDateTime,
                EndDateTime = meeting.EndDateTime,
                MeetingParticipants = participantModelsList
            };
            return meetingModel;
        }
    }
}
