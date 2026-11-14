

using Microsoft.AspNetCore.Mvc;
using ConfidentVotesAPI.Models;
using System.Reflection;
using System.Text.Json;
using ConfidentVotesAPI.Services;

namespace ConfidentVotesAPI.Controllers
{
    [ApiController]

    //[Route("confidentvotesapi/[controller]")]
    [Route("confidentvotesapi/elections")]
    // the class must be public
    public class ElectionsController : ControllerBase
    {   
     
        public required IElectionsHandler  _MyElectionsHandler ;

        // when the controller is create the only one instance of 
        // IProductsHandler will be used or reused
        public ElectionsController(IElectionsHandler electionsHandler)
        {
            _MyElectionsHandler = electionsHandler;
        }
        /* ############### GET ############*/
        [HttpGet]
        public async Task<ActionResult<List<ElectionResponseDto>>> GetAll()
        {
            var Elections = await _MyElectionsHandler.GetElections();
            return Ok(Elections);
        }
         [HttpGet("{id:int:min(0)}")]
        public async Task<ActionResult<ElectionResponseDto>> GetbyId(int id)
        {
            var Election = await _MyElectionsHandler.GetElectionById(id);
            return Ok(Election);
            
        }
        [HttpGet("{id:int:min(0)}/choices")]
        public async Task<ActionResult<ChoiceResponseDto>> GetChoicesByElectionId(int id)
        {
            var ElectionChoices = await _MyElectionsHandler.GetElectionChoices(id);
            return Ok(ElectionChoices);
            
        }
        [HttpPost] 
         /* ############### POST ############*/
        public async Task<ActionResult<ElectionResponseDto>> Create( [FromBody] ElectionRequestDto newElection)
        {
            await _MyElectionsHandler.addElection(newElection);
            return AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = newElection.ElectionID}, 
                       value:newElection);
        }
        /* ############### PUT ############*/
        [HttpPut("{id:int:min(0)}")] 
        public async Task<ActionResult<ElectionResponseDto>> Update(int id, ElectionRequestDto updatedElection)
        {
           
            await _MyElectionsHandler.UpdateElection(id, updatedElection);
            return AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = updatedElection.ElectionID}, 
                       value:updatedElection);
        }

        /* ############### DELETE ############*/
        [HttpDelete("{id:int:min(1)}")] 
        public async Task<ActionResult<ElectionResponseDto>> Delete(int id)
        {
            await _MyElectionsHandler.RemoveElection(id);
            return NoContent();
        }
    }
}