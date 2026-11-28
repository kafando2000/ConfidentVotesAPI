

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
    [Route("confidentvotes/api/voters")]
    // the class must be public
    public class VotersController : ControllerBase
    {   
        public required IVotersHandler  _MyVotersHandler ;
        // when the controller is create the only one instance of 
        // IProductsHandler will be used or reused
        public VotersController(IVotersHandler VotersHandler)
        {
            _MyVotersHandler = VotersHandler;
        }
        /* ############### GET ############*/
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<HashSet<VoterResponseDto>>> GetAll()
        {
            return Ok(await _MyVotersHandler.GetVotersAsync());
        }
        [HttpGet("{id:Guid}")]
        [Authorize] 
        public async Task<ActionResult<VoterResponseDto>> GetbyId(Guid id)
        {
            var voter = await _MyVotersHandler.GetVoterByIdAsync(id);
            return voter != null ? Ok(voter) : BadRequest(@$"Check your data, something is wrong. 
            The likely cause is that
            the provided id : {id} is not related to any voter");
            
        }
        
         /* ############### POST ############*/
        [HttpPost] 
        [Authorize]
        public async Task<ActionResult<VoterResponseDto>> Create( VoterRequestDto newVoter)
        {

            var voter = await _MyVotersHandler.addVoterAsync(newVoter);
            if(voter == null)
            {
                return BadRequest($@"Something is wrong :
                The likely causes are :
                    1. Id of Election: {newVoter.ElectionId} is not related to any election
                    2. Id of User : {newVoter.AppUserId} is not related to any user");                
            }
            return AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = Guid.CreateVersion7()}, 
                       value:newVoter);
        }
        /* ############### PUT ############*/
        [HttpPut("{id:Guid}")] 
        [Authorize]
        public async Task<ActionResult<VoterResponseDto>> Update(Guid id, VoterRequestDto updatedVoter)
        {
           
            var voter = await _MyVotersHandler.UpdateVoterAsync(id, updatedVoter);
            if(voter == null)
            {
                return BadRequest($@"Something is wrong :
                The likely causes are:
                    1. Id : {id} is not related to any voter
                    2. Id of User : {updatedVoter.AppUserId} is not related to any user");  
            }
            return AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = voter.Id}, 
                       value:updatedVoter);
        }

        /* ############### DELETE ############*/
        [HttpDelete("{id:Guid}")] 
        [Authorize]
        public async Task<ActionResult<VoterResponseDto>> Delete(Guid id)
        {
            var isDeleted = await _MyVotersHandler.RemoveVoterAsync(id);
            return isDeleted ? NoContent() : 
                                        BadRequest(@$"Check your data, something is wrong. 
                                        The likely cause is that
                                        the provided id : {id} is not related to any voter");
        }
    }
}