

using Microsoft.AspNetCore.Mvc;
using ConfidentVotesAPI.Models;
using System.Reflection;
using System.Text.Json;
using ConfidentVotesAPI.Services;

namespace ConfidentVotesAPI.Controllers
{
    [ApiController]

    //[Route("confidentvotesapi/[controller]")]
    [Route("confidentvotesapi/choices")]
    // the class must be public
    public class ChoicesController : ControllerBase
    {   
     
        public required IChoicesHandler  _MyChoicesHandler ;

        // when the controller is create the only one instance of 
        // IProductsHandler will be used or reused
        public ChoicesController(IChoicesHandler choicesHandler)
        {
            _MyChoicesHandler = choicesHandler;
        }
        /* ############### GET ############*/
        [HttpGet]
        public async Task<ActionResult<List<ChoiceResponseDto>>> GetAll()
        {
            var Choices = await _MyChoicesHandler.GetChoices();
            return Ok(Choices);
        }
         [HttpGet("{id:int:min(0)}")]
        public async Task<ActionResult<ChoiceResponseDto>> GetbyId(int id)
        {
            var choice = await _MyChoicesHandler.GetChoiceById(id);
            return Ok(choice);
            
        }
        [HttpPost] 
         /* ############### POST ############*/
        public async Task<ActionResult<ChoiceResponseDto>> Create( [FromBody] ChoiceRequestDto newChoice)
        {
            await _MyChoicesHandler.addChoice(newChoice);
            return AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = newChoice.ChoiceID}, 
                       value:newChoice);
        }
        /* ############### PUT ############*/
        [HttpPut("{id:int:min(0)}")] 
        public async Task<ActionResult<ChoiceResponseDto>> Update(int id, ChoiceRequestDto updatedChoice)
        {
           var newchoice = updatedChoice;
            await _MyChoicesHandler.UpdateChoice(id, newchoice);
            return AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = updatedChoice.ChoiceID}, 
                       value:updatedChoice);
        }

        /* ############### DELETE ############*/
        [HttpDelete("{id:int:min(1)}")] 
        public async Task<ActionResult<ChoiceResponseDto>> Delete(int id)
        {
            await _MyChoicesHandler.RemoveChoice(id);
            return NoContent();
        }
    }
}