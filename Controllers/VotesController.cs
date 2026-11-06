

using Microsoft.AspNetCore.Mvc;
using ConfidentVotesAPI.Models;
using System.Reflection;
using ConfidentVotesAPI.Services;

namespace ConfidentVotesAPI.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
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
        public async Task<ActionResult<List<Vote>>> GetAll()
        {
            var votes = await _MyVotesHandler.GetVotes();
            return Ok(votes);
        }
         [HttpGet("{id:int:min(0)}")]
        public async Task<ActionResult<Vote>> GetbyId(int id)
        {
            var vote = await _MyVotesHandler.GetVoteById(id);
            return Ok(vote);
            
        }
        [HttpPost] 
         /* ############### POST ############*/
        public async Task<ActionResult<Vote>> Create(Vote newVote)
        {
            var votes = await  _MyVotesHandler.GetVotes();
            var voteId = votes.Count + 1;
            newVote.Id = voteId;
            await _MyVotesHandler.addVote(newVote);
            return AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = newVote.Id}, 
                       value:newVote);
        }
        /* ############### PUT ############*/
        [HttpPut("{id:int:min(0)}")] 
        public async Task<ActionResult<Vote>> Update(int id, Vote updatedVote)
        {
           
            await _MyVotesHandler.UpdateVote(id, updatedVote);
            return Ok(updatedVote);
        }

        /* ############### DELETE ############*/
        [HttpDelete("{id:int:min(1)}")] 
        public async Task<ActionResult<Vote>> Delete(int id)
        {
            await _MyVotesHandler.RemoveVote(id);
            return NoContent();
        }
    }
}