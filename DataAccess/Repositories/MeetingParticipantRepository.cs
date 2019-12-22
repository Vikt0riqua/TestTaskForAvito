using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Contexts;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class MeetingParticipantRepository
    {
        private readonly MeetingParticipantsContext _db;
        public MeetingParticipantRepository(MeetingParticipantsContext context)
        {
            _db = context;
        }
        public async Task<List<Participant>> GetAllParticipants()
        {
            return await _db.Participants.Include(m => m.MeetingParticipants).ThenInclude(mp => mp.Meeting).ToListAsync();
        }
        public Participant GetParticipant(int id)
        {
            return _db.Participants.Include(m => m.MeetingParticipants).ThenInclude(mp => mp.Meeting).FirstOrDefault(x => x.ParticipantId == id);
        }
        public async Task<Participant> AddParticipant(Participant participant)
        {
            _db.Participants.Add(participant);
            var success = await _db.SaveChangesAsync() == 1;
            if (!success)
            {
                throw new Exception("Произошла ошибка при добавление участника в базу");
            }
            return participant;
        }

        public async Task<List<Meeting>> GetAllMeetings()
        {
            return await _db.Meetings.Include(m => m.MeetingParticipants).ThenInclude(mp => mp.Participant).ToListAsync();
        }
        public Meeting GetMeeting(int id)
        {
            return _db.Meetings.Include(m => m.MeetingParticipants).ThenInclude(mp => mp.Participant).FirstOrDefault(x => x.MeetingId == id);
        }

        public async Task<Meeting> AddMeeting(Meeting meeting)
        {
            _db.Meetings.Add(meeting);
            var success = await _db.SaveChangesAsync() == 1;
            if (!success)
            {
                throw new Exception("Произошла ошибка при добавление встречи");
            }
            return meeting;
        }

        public async Task DeleteMeeting(int id)
        {
            var meeting = _db.Meetings.FirstOrDefault(x => x.MeetingId == id);
            if (meeting == null)
            {
                throw new Exception("Встреча с данным id не найдена");
            }
            _db.Meetings.Remove(meeting);
            await _db.SaveChangesAsync();
        }
        public async Task<Meeting> AddParticipantForMeeting(Meeting meeting, Participant participant)
        {
            var meetingParticipant = new MeetingParticipant() { MId = meeting.MeetingId, PId = participant.ParticipantId, Participant = participant };
            meeting.MeetingParticipants.Add(meetingParticipant);
            _db.Meetings.Update(meeting);
            var success = await _db.SaveChangesAsync() == 2;
            if (!success)
            {
                throw new Exception("Произошла ошибка при добавление участника с id = " + participant.ParticipantId);
            }
            return meeting;
        }

        public async Task DeleteParticipantFromMeeting(int meetingId, int participantId)
        {
            var meeting = _db.Meetings.Include(m => m.MeetingParticipants).ThenInclude(mp => mp.Participant).FirstOrDefault(x => x.MeetingId == meetingId);
            if(meeting == null) throw new Exception("Вчтреча с данным id не найдена");
            var meetingParticipant = meeting.MeetingParticipants.FirstOrDefault(x => x.PId == participantId);
            if (meetingParticipant == null) throw new Exception("Человек не является участником данной встречи");
            meeting.MeetingParticipants.Remove(meetingParticipant);
            var success = await _db.SaveChangesAsync() == 1;
            if (!success)
            {
                throw new Exception("Произошла ошибка при удалении участника со встречи");
            }
        }

        public bool CheckParticipantTime(Participant participant, Meeting meeting)
        {
            var meetings = participant.MeetingParticipants?.Select(mp => new { mp.Meeting.StartDateTime, mp.Meeting.EndDateTime});
            if (meetings == null) return true;
            var haveMeeting = meetings.Any(m => m.StartDateTime <= meeting.EndDateTime && m.EndDateTime >= meeting.StartDateTime);
            return !haveMeeting;
        }
    }
}