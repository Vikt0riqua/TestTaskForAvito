using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataAccess.Models;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class ParticipantService
    {
        private readonly MeetingParticipantRepository _meetingParticipantRepository;
        public ParticipantService(MeetingParticipantRepository meetingParticipantRepository)
        {
            _meetingParticipantRepository = meetingParticipantRepository;
        }
        public async Task<List<Participant>> Get()
        {
            return await _meetingParticipantRepository.GetAllParticipants();
        }

        public async Task<Participant> AddParticipant(Participant participant)
        {
            if (!IsValidEmail(participant.Email)) throw new Exception("Email задан не верно");
            participant = await _meetingParticipantRepository.AddParticipant(participant);
            return participant;
        }

        private bool IsValidEmail(string email)
        {
            string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            Match isMatch = Regex.Match(email, pattern, RegexOptions.IgnoreCase);
            return isMatch.Success;
        }
    }
}
