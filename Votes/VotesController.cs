

using Microsoft.AspNetCore.Mvc;
using ConfidentVotesAPI.Models;
using System.Reflection;
using System.Text.Json;
using ConfidentVotesAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace ConfidentVotesAPI.Controllers
{
    [ApiController]

    //[Route("confidentvotesapi/[controller]")]
    [Route("confidentvotes/api/votes")]
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
        [Authorize] 
        public async Task<ActionResult<HashSet<VoteResponseDto>>> GetAll()
        {
            return Ok(await _MyVotesHandler.GetVotesAsync());
        }
        [HttpGet("{id:Guid}")]
        [Authorize] 
        public async Task<ActionResult<VoteResponseDto>> GetbyId(Guid id)
        {
            var vote = await _MyVotesHandler.GetVoteByIdAsync(id);
            return vote != null ? Ok(vote) : BadRequest(@$"Check your data, something is wrong. 
            The likely cause is that
            the provided id : {id} is not related to any vote");
            
        }
         /* ############### POST ############*/
       
        [HttpPost]
        [Authorize]  
        public async Task<ActionResult<VoteResponseDto>> Create( VoteRequestDto newVote)
        {
            var vote = await _MyVotesHandler.addVoteAsync(newVote);
            if(vote == null)
            {
                return BadRequest($@"Something is wrong :
                The likely causes are :
                    1. Id of option : {newVote.OptionId} is not related to any option
                    2. Id of voter : {newVote.VoterId} is not related to any voter");                
            }
            return AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = Guid.CreateVersion7()}, 
                       value:newVote);
        }
        /* ############### PUT ############*/
        [HttpPut("{id:Guid}")] 
        [Authorize] 
        public async Task<ActionResult<VoteResponseDto>> Update(Guid id, VoteRequestDto updatedVote)
        {
           
            var vote = await _MyVotesHandler.UpdateVoteAsync(id, updatedVote);
            if(vote == null)
            {
                return BadRequest($@"Something is wrong :
                The likely causes are:
                    1. Id : {id} is not related to any vote
                    2. Id of voter : {updatedVote.VoterId} is not related to any voter
                    3. you're trying to change a vote to be part of  unrelated election");  
            }
            return AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = vote.Id}, 
                       value:updatedVote);
        }

        /* ############### DELETE ############*/
        [HttpDelete("{id:Guid}")] 
        [Authorize] 
        public async Task<ActionResult<VoteResponseDto>> Delete(Guid id)
        {
            var isDeleted = await _MyVotesHandler.RemoveVoteAsync(id);
            return isDeleted ? NoContent() : 
                                        BadRequest(@$"Check your data, something is wrong. 
                                        The likely cause is that
                                        the provided id : {id} is not related to any vote");
        }
    }
}