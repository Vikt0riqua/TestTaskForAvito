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
    public class ParticipantsController : ControllerBase
    {
        private readonly ParticipantService _participantService;
        public ParticipantsController(ParticipantService participantService)
        {
            _participantService = participantService;
        }

        // GET: Participants
        [HttpGet]
        public async Task<ActionResult<List<ParticipantModel>>> Get()
        {
            var allParticipants = await _participantService.Get();
            return allParticipants.Select(participant => new ParticipantModel() { Id = participant.ParticipantId, Name = participant.ParticipantName, Email = participant.Email }).ToList();
        }

        // POST:Participants
        [HttpPost]
        public async Task<ActionResult<ParticipantModel>> Post(ParticipantModel participantModel)
        {
            if (participantModel == null)
            {
                return Ok("Вы не задали параметры для участника");
            }
            try
            {
                var participant = await _participantService.AddParticipant(new Participant() { ParticipantName = participantModel.Name, Email = participantModel.Email });
                return Ok(new ParticipantModel() { Id = participant.ParticipantId, Name = participant.ParticipantName, Email = participant.Email });
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
    }
}
