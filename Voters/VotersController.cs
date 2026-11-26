

using Microsoft.AspNetCore.Mvc;
using ConfidentVotesAPI.Models;
using System.Reflection;
using System.Text.Json;
using ConfidentVotesAPI.Services;

namespace ConfidentVotesAPI.Controllers
{
    [ApiController]

    //[Route("confidentvotesapi/[controller]")]
    [Route("confidentvotes/api/Voters")]
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
        public async Task<ActionResult<List<VoterResponseDto>>> GetAll()
        {
            var Voters = await _MyVotersHandler.GetVoters();
            return Ok(Voters);
        }
         [HttpGet("{id:int:min(0)}")]
        public async Task<ActionResult<VoterResponseDto>> GetbyId(Guid id)
        {
            var Voter = await _MyVotersHandler.GetVoterById(id);
            return Ok(Voter);
            
        }
        [HttpPost] 
         /* ############### POST ############*/
        public async Task<ActionResult<ElectionResponseDto>> Create( VoterRequestDto newVoter)
        {
            await _MyVotersHandler.addVoter(newVoter);
            return AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = Guid.CreateVersion7()}, 
                       value:newVoter);
        }
        /* ############### PUT ############*/
        [HttpPut("{id:Guid}")] 
        public async Task<ActionResult<ElectionResponseDto>> Update(Guid id, VoterRequestDto updatedVoter)
        {
           
            await _MyVotersHandler.UpdateVoter(id, updatedVoter);
            return AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = Guid.CreateVersion7()}, 
                       value:updatedVoter);
        }

        /* ############### DELETE ############*/
        [HttpDelete("{id:int:min(1)}")] 
        public async Task<ActionResult<VoterResponseDto>> Delete(Guid id)
        {
            await _MyVotersHandler.RemoveVoter(id);
            return NoContent();
        }
    }
}