using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Contexts;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class MeetingService
    {
        private readonly MeetingParticipantRepository _meetingParticipantRepository;
        private readonly MessageService _messageService;
        public MeetingService(MeetingParticipantRepository meetingParticipantRepository, MessageService messageService)
        {
            _meetingParticipantRepository = meetingParticipantRepository;
            _messageService = messageService;
        }

        public async Task<List<Meeting>> Get()
        {
            return await _meetingParticipantRepository.GetAllMeetings();
        }

        public async Task<Meeting> AddMeeting(Meeting meeting, List<int> participantsId)
        {
            var meetingResult = await _meetingParticipantRepository.AddMeeting(meeting);
            if (meetingResult == null) return null;
            return await AddParticipantsForMeeting(meeting.MeetingId, participantsId);
        }
        public async Task<Meeting> AddParticipantsForMeeting(int meetingId, List<int> participantsId)
        {
            var meeting = _meetingParticipantRepository.GetMeeting(meetingId);
            if (meeting == null) return null;
            var participantsIdInMeeting = meeting.MeetingParticipants?.Select(x => x.ParticipantId);
            if (participantsIdInMeeting != null) participantsId = participantsId.Except(participantsIdInMeeting).ToList();
            foreach (var partId in participantsId)
            {
                bool freeTime = _meetingParticipantRepository.CheckParticipantTime(partId, meeting);
                if (!freeTime) continue;
                meeting = await _meetingParticipantRepository.AddParticipantForMeeting(meeting, partId);
                //SendEmailToParticipant(_meetingParticipantRepository.GetParticipant(partId).Email, meeting);
            }
            return meeting;
        }

        public async Task<int> DeleteMeeting(int id)
        {
            return await _meetingParticipantRepository.DeleteMeeting(id);
        }
        public async Task<Meeting> DeleteParticipantFromMeeting(int meetingId, int participantId)
        {
            return await _meetingParticipantRepository.DeleteParticipantFromMeeting(meetingId, participantId);
        }

        private void SendEmailToParticipant(string email, Meeting meeting)
        {
            _messageService.SendEmail(email, meeting.Name, meeting.StartDateTime);
        }
    }
}