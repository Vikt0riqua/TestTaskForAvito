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
            return allParticipants.Select(participant => new ParticipantModel() { Id = participant.ParticipantId, Name = participant.Name, Email = participant.Email }).ToList();
        }
        // GET: Participants/id
        [HttpGet]
        [Route("{id}")]
        public ActionResult<DateTime> GetDate(int id)
        {
            return DateTime.Now;
        }

        // POST:Participants
        [HttpPost]
        public async Task<ActionResult<ParticipantModel>> Post(ParticipantModel participantModel)
        {
            if (participantModel == null)
            {
                return BadRequest();
            }
            var participant = await _participantService.AddParticipant(new Participant() { Name = participantModel.Name, Email = participantModel.Email });
            return Ok(participant);
        }
    }
}
