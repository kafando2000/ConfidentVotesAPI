

using Microsoft.AspNetCore.Mvc;
using ConfidentVotesAPI.Models;
using System.Reflection;
using System.Text.Json;
using ConfidentVotesAPI.Services;

namespace ConfidentVoteAPI.Controllers
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
        public async Task<ActionResult<List<Election>>> GetAll()
        {
            var Elections = await _MyElectionsHandler.GetElections();
            return Ok(Elections);
        }
         [HttpGet("{id:int:min(0)}")]
        public async Task<ActionResult<Election>> GetbyId(int id)
        {
            var Election = await _MyElectionsHandler.GetElectionById(id);
            return Ok(Election);
            
        }
        [HttpGet("{id:int:min(0)}/choices")]
        public async Task<ActionResult<Election>> GetChoicesByElectionId(int id)
        {
            var Election = await _MyElectionsHandler.GetElectionChoices(id);
            return Ok(Election);
            
        }
        [HttpPost] 
         /* ############### POST ############*/
        public async Task<ActionResult<Election>> Create( [FromBody] ElectionDto newElection)
        {
            await _MyElectionsHandler.addElection(newElection);
            return AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = newElection.ElectionID}, 
                       value:newElection);
        }
        /* ############### PUT ############*/
        [HttpPut("{id:int:min(0)}")] 
        public async Task<ActionResult<Election>> Update(int id, ElectionDto updatedElection)
        {
           
            await _MyElectionsHandler.UpdateElection(id, updatedElection);
            return Ok(updatedElection);
        }

        /* ############### DELETE ############*/
        [HttpDelete("{id:int:min(1)}")] 
        public async Task<ActionResult<Election>> Delete(int id)
        {
            await _MyElectionsHandler.RemoveElection(id);
            return NoContent();
        }
    }
}