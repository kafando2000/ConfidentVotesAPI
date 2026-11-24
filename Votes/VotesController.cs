

using Microsoft.AspNetCore.Mvc;
using ConfidentVotesAPI.Models;
using System.Reflection;
using ConfidentVotesAPI.Services;

namespace ConfidentVotesAPI.Controllers
{
    [ApiController]

    [Route("confidentvotesapi/votes")]
    // the class must be public
    public class VotesController : ControllerBase
    {   
     
        public required IVotesHandler  _MyVotesHandler ;

        // when the controller is create the only one instance of 
        // IProductsHandler will be used or reused
        public VotesController(IVotesHandler votesHandler)
        {
            _MyVotesHandler = votesHandler;
        }
        /* ############### GET ############*/
        [HttpGet]
        public async Task<ActionResult<List<VoteResponseDto>>> GetAll()
        {
            var votes = await _MyVotesHandler.GetVotes();
            return Ok(votes);
        }
         [HttpGet("{id:int:min(0)}")]
        public async Task<ActionResult<VoteResponseDto>> GetbyId(int id)
        {
            var vote = await _MyVotesHandler.GetVoteById(id);
            return Ok(vote);
            
        }
        [HttpPost] 
         /* ############### POST ############*/
        public async Task<ActionResult<VoteResponseDto>> Create(VoteRequestDto newVote)
        {
            await _MyVotesHandler.addVote(newVote);
            return AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = newVote.VoteID}, 
                       value:newVote);
        }
        /* ############### PUT ############*/
        [HttpPut("{id:int:min(0)}")] 
        public async Task<ActionResult<VoteResponseDto>> Update(int id, VoteRequestDto updatedVote)
        {
           
            await _MyVotesHandler.UpdateVote(id, updatedVote);
            return AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = updatedVote.VoteID}, 
                       value:updatedVote);
        }

        /* ############### DELETE ############*/
        [HttpDelete("{id:int:min(1)}")] 
        public async Task<ActionResult<VoteResponseDto>> Delete(int id)
        {
            await _MyVotesHandler.RemoveVote(id);
            return NoContent();
        }
    }
}