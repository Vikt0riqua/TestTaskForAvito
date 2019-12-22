using System;
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

        public async Task<(Meeting, List<string>)> AddMeeting(Meeting meeting, List<int> participantsId)
        {
            meeting = await _meetingParticipantRepository.AddMeeting(meeting);
            var result = await AddParticipantsForMeeting(meeting.MeetingId, participantsId);
            return result;
        }
        public async Task<(Meeting, List<string>)> AddParticipantsForMeeting(int meetingId, List<int> participantsId)
        {
            var meeting = _meetingParticipantRepository.GetMeeting(meetingId);
            if (meeting == null) throw new Exception("Встреча с данным id не найдена");
            var participantsIdInMeeting = meeting.MeetingParticipants?.Select(x => x.PId);
            if (participantsIdInMeeting != null) participantsId = participantsId.Except(participantsIdInMeeting).ToList();
            List<string> errors = new List<string>();
            foreach (var participantId in participantsId)
            {
                var res = await TryAddParticipant(meeting, participantId);
                if(res != null) errors.Add(res); 
            }
            meeting = _meetingParticipantRepository.GetMeeting(meetingId);
            return (meeting, errors);
        }

        private async Task<string> TryAddParticipant(Meeting meeting, int participantId)
        {
            try
            {
                await AddParticipantForMeeting(meeting, participantId);
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        private async Task AddParticipantForMeeting(Meeting meeting, int participantId)
        {
            var participant = _meetingParticipantRepository.GetParticipant(participantId);
            if (participant == null) { throw new Exception("Участник с id = " + participantId + " не найден"); }
            if (!_meetingParticipantRepository.CheckParticipantTime(participant, meeting))
            {
                throw new Exception("Участник с id = " + participantId + " занят на данное время");
            }
            await _meetingParticipantRepository.AddParticipantForMeeting(meeting, participant);
            SendEmailToParticipant(participant, meeting);
        }
        public Task DeleteMeeting(int id)
        {
            return _meetingParticipantRepository.DeleteMeeting(id);
        }
        public Task DeleteParticipantFromMeeting(int meetingId, int participantId)
        {
            return _meetingParticipantRepository.DeleteParticipantFromMeeting(meetingId, participantId);
        }

        private void SendEmailToParticipant(Participant participant, Meeting meeting)
        {
            try
            {
                _messageService.SendEmail(participant.Email, meeting.MeetingName, meeting.StartDateTime);
            }
            catch (Exception)
            {
                throw new Exception("Сообщение для участника с id = " + participant.ParticipantId + " не отправлено");
            }
        }
    }
}