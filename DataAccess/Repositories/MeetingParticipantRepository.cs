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
            return await _db.Participants.ToListAsync();
        }
        public Participant GetParticipant(int id)
        {
            return _db.Participants.FirstOrDefault(x => x.ParticipantId == id);
        }
        public async Task<Participant> AddParticipant(Participant participant)
        {
            _db.Participants.Add(participant);
            await _db.SaveChangesAsync();
            return participant;
        }

        public async Task<List<Meeting>> GetAllMeetings()
        {
            return await _db.Meetings.ToListAsync();
        }
        public Meeting GetMeeting(int id)
        {
            return _db.Meetings.FirstOrDefault(x => x.MeetingId == id);
        }

        public async Task<Meeting> AddMeeting(Meeting meeting)
        {
            _db.Meetings.Add(meeting);
            await _db.SaveChangesAsync();
            return meeting;
        }

        public async Task<int> DeleteMeeting(int id)
        {
            var meeting = _db.Meetings.FirstOrDefault(x => x.MeetingId == id);
            if (meeting == null)
            {
                return int.MinValue;
            }
            _db.Meetings.Remove(meeting);
            return await _db.SaveChangesAsync();
        }
        public async Task<Meeting> AddParticipantForMeeting(Meeting meeting, int participantId)
        {
            var meetingParticipant = new MeetingParticipant() { MeetingId = meeting.MeetingId, ParticipantId = participantId };
            if(meeting.MeetingParticipants == null) meeting.MeetingParticipants = new List<MeetingParticipant>();
            meeting.MeetingParticipants.Add(meetingParticipant);
            _db.Meetings.Update(meeting);
            await _db.SaveChangesAsync();
            return meeting;
        }

        public async Task<Meeting> DeleteParticipantFromMeeting(int meetingId, int participantId)
        {
            var meeting = _db.Meetings.FirstOrDefault(x => x.MeetingId == meetingId);
            var meetingParticipant = meeting?.MeetingParticipants.FirstOrDefault(x => x.ParticipantId == participantId);
            if (meetingParticipant == null) return meeting;
            meeting.MeetingParticipants.Remove(meetingParticipant);
            await _db.SaveChangesAsync();
            return meeting;
        }

        public bool CheckParticipantTime(int participantId, Meeting meeting)
        {
            bool result = true;
            //проверка времени пользователя
            return result;
        }
    }
}