

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
        // get all elections response
        [HttpGet]
        public async Task<ActionResult<List<ElectionResponseDto>>> GetAllAsync()
        {
            var Elections = await _MyElectionsHandler.GetElectionsAsync();
            return Ok(Elections);
        }
        // get election by id response
         [HttpGet("{id:Guid}")]
        public async Task<ActionResult<ElectionResponseDto>> GetbyIdAsync(Guid id)
        {
            var election = await _MyElectionsHandler.GetElectionByIdAsync(id);
            if(election is null)
            {
                return BadRequest($"{id} is not associated with an election.");
            }
            return Ok(election);
            
        }
        // get options related to an election, response
        [HttpGet("{id:Guid}/options")]
        public async Task<ActionResult<OptionResponseDto>> GetOptionsByIdAsync(Guid id)
        {
            var election = await _MyElectionsHandler.GetElectionByIdAsync(id);
            if( election == null)
            {
                return NotFound($"Id : {id} is not related to an election");
            }
            var ElectionOptions = await _MyElectionsHandler.GetElectionOptionsAsync(id);
            return Ok(ElectionOptions);
            
        }
        [HttpPost] 
         /* ############### POST ############*/
        public async Task<ActionResult<ElectionResponseDto>> Create( [FromBody] ElectionRequestDto newElection)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem();
            }
            var election = await _MyElectionsHandler.addElectionAsync(newElection);
            if(election == null)
            {
                return BadRequest($@"check your model data, something is not okay !\n
                 possible causes:\n
                       1. Election User data is not corrected.\n
                       2. Election End datetime and Start datetime are the same.\n"); 
            }
            return AcceptedAtAction(actionName:nameof(GetbyIdAsync), 
                       routeValues: new {id = election.Id}, 
                       value:newElection);
        }
        /* ############### PUT ############*/
        [HttpPut("{id:Guid}")] 
        public async Task<ActionResult<ElectionResponseDto>> Update(Guid id, ElectionRequestDto updatedElection)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem();
            }
            var election = await _MyElectionsHandler.UpdateElectionAsync(id, updatedElection);
            if(election == null)
            {
                return BadRequest($@"check your model data, something is not okay !\n
                 possible causes:\n
                       1. Maybe the id : {id} is not related to any election.\n
                       2. Election User data is not corrected.\n
                       3. Election End datetime and Start datetime are the same.\n");   
            }   
            return AcceptedAtAction(actionName:nameof(GetbyIdAsync), 
                       routeValues: new {id = election.Id}, 
                       value:updatedElection);
        }

        /* ############### DELETE ############*/
        [HttpDelete("{id:Guid}")] 
        public async Task<ActionResult<ElectionResponseDto>> Delete(Guid id)
        {
            var isRemoved = await _MyElectionsHandler.RemoveElectionAsync(id);
        
            return isRemoved ? NoContent() : BadRequest($"Id : {id} might not be related to an election anymore or try again."); 
        }
    }
}