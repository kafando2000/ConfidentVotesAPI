

using Microsoft.AspNetCore.Mvc;
using ConfidentVotesAPI.Models;
using System.Reflection;
using System.Text.Json;
using ConfidentVotesAPI.Services;

namespace ConfidentVotesAPI.Controllers
{
    [ApiController]

    //[Route("confidentvotesapi/[controller]")]
    [Route("confidentvotesapi/electionvoters")]
    // the class must be public
    public class ElectionVotersController : ControllerBase
    {   
     
        public required IElectionVotersHandler  _MyElectionVotersHandler ;

        // when the controller is create the only one instance of 
        // IProductsHandler will be used or reused
        public ElectionVotersController(IElectionVotersHandler ElectionVotersHandler)
        {
            _MyElectionVotersHandler = ElectionVotersHandler;
        }
        /* ############### GET ############*/
        [HttpGet]
        public async Task<ActionResult<List<ElectionVoterResponseDto>>> GetAll()
        {
            var ElectionVoters = await _MyElectionVotersHandler.GetElectionVoters();
            return Ok(ElectionVoters);
        }
         [HttpGet("{id:int:min(0)}")]
        public async Task<ActionResult<ElectionVoterResponseDto>> GetbyId(int id)
        {
            var ElectionVoter = await _MyElectionVotersHandler.GetElectionVoterById(id);
            return Ok(ElectionVoter);
            
        }
        [HttpPost] 
         /* ############### POST ############*/
        public async Task<ActionResult<ElectionResponseDto>> Create( ElectionVoterRequestDto newElectionVoter)
        {
            await _MyElectionVotersHandler.addElectionVoter(newElectionVoter);
            return AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = newElectionVoter.ElectionVoterID}, 
                       value:newElectionVoter);
        }
        /* ############### PUT ############*/
        [HttpPut("{id:int:min(0)}")] 
        public async Task<ActionResult<ElectionResponseDto>> Update(int id, ElectionVoterRequestDto updatedElectionVoter)
        {
           
            await _MyElectionVotersHandler.UpdateElectionVoter(id, updatedElectionVoter);
            return AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = updatedElectionVoter.ElectionVoterID}, 
                       value:updatedElectionVoter);
        }

        /* ############### DELETE ############*/
        [HttpDelete("{id:int:min(1)}")] 
        public async Task<ActionResult<ElectionResponseDto>> Delete(int id)
        {
            await _MyElectionVotersHandler.RemoveElectionVoter(id);
            return NoContent();
        }
    }
}