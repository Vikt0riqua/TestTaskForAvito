using System;
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
        public async Task<ActionResult<MeetingModelForResponse>> Post(MeetingModelWithParticipantIds meetingModel)
        {
            if (meetingModel == null)
            {
                return Ok("Вы не задали параметры для встречи");
            }
            var meeting = new Meeting()
            {
                MeetingName = meetingModel.Name,
                StartDateTime = meetingModel.StartDateTime,
                EndDateTime = meetingModel.EndDateTime
            };
            try
            {
                var (meetingResult, errorList) = await _meetingService.AddMeeting(meeting, meetingModel.MeetingParticipantsId);
                return Ok(new MeetingModelForResponse { Meeting = CreateMeetingModel(meetingResult), Errors = errorList });
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        // DELETE:Meetings/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _meetingService.DeleteMeeting(id);
                return Ok("Встреча удалена");
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        // POST: Meetings/5/Participants
        [HttpPost]
        [Route("{id}/Participants")]
        public async Task<ActionResult<MeetingModelForResponse>> PostParticipants(int id, [FromBody] MeetingParticipants meetingParticipants)
        {
            if (meetingParticipants.MeetingParticipantsId == null || !meetingParticipants.MeetingParticipantsId.Any()) return Ok("Вы не указали участников для добавления.");
            try
            {
                var (meeting, errorList) = await _meetingService.AddParticipantsForMeeting(id, meetingParticipants.MeetingParticipantsId);
                return Ok(new MeetingModelForResponse { Meeting = CreateMeetingModel(meeting), Errors = errorList });
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        // DELETE:Meetings/5/Participant/1
        [HttpDelete]
        [Route("{meetingId}/Participants/{participantId}")]
        public async Task<ActionResult> DeleteParticipant(int meetingId, int participantId)
        {
            try
            {
                await _meetingService.DeleteParticipantFromMeeting(meetingId, participantId);
                return Ok("Участник удален из митинга");
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        private static MeetingModel CreateMeetingModel(Meeting meeting)
        {
            var participantModelsList = new List<ParticipantModel>();
            if (meeting.MeetingParticipants != null)
            {
                foreach (var participant in meeting.MeetingParticipants)
                {
                    if (participant.Participant == null) continue;
                    participantModelsList.Add(new ParticipantModel()
                    {
                        Id = participant.PId,
                        Name = participant.Participant.ParticipantName,
                        Email = participant.Participant.Email
                    });
                }
            }
            var meetingModel = new MeetingModel()
            {
                Id = meeting.MeetingId,
                Name = meeting.MeetingName,
                StartDateTime = meeting.StartDateTime,
                EndDateTime = meeting.EndDateTime,
                MeetingParticipants = participantModelsList
            };
            return meetingModel;
        }
    }
}
