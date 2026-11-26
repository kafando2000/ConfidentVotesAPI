

using Microsoft.AspNetCore.Mvc;
using ConfidentVotesAPI.Models;
using System.Reflection;
using System.Text.Json;
using ConfidentVotesAPI.Services;

namespace ConfidentVotesAPI.Controllers
{
    [ApiController]

    //[Route("confidentvotesapi/[controller]")]
    [Route("confidentvotesapi/Options")]
    // the class must be public
    public class OptionsController : ControllerBase
    {   
     
        public required IOptionsHandler  _MyOptionsHandler ;

        // when the controller is create the only one instance of 
        // IProductsHandler will be used or reused
        public OptionsController(IOptionsHandler OptionsHandler)
        {
            _MyOptionsHandler = OptionsHandler;
        }
        /* ############### GET ############*/
        [HttpGet]
        public async Task<ActionResult<List<OptionResponseDto>>> GetAll()
        {
            var Options = await _MyOptionsHandler.GetOptions();
            return Ok(Options);
        }
         [HttpGet("{id:Guid}")]
        public async Task<ActionResult<OptionResponseDto>> GetbyId(Guid id)
        {
            var Option = await _MyOptionsHandler.GetOptionById(id);
            return Ok(Option);
            
        }
        [HttpPost] 
         /* ############### POST ############*/
        public async Task<ActionResult<OptionResponseDto>> Create( [FromBody] OptionRequestDto newOption)
        {
            await _MyOptionsHandler.addOption(newOption);
            return AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = newOption.Id}, 
                       value:newOption);
        }
        /* ############### PUT ############*/
        [HttpPut("{id:Guid:min(0)}")] 
        public async Task<ActionResult<OptionResponseDto>> Update(Guid id, OptionRequestDto updatedOption)
        {
           var newOption = updatedOption;
            await _MyOptionsHandler.UpdateOption(id, newOption);
            return AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = updatedOption.Id}, 
                       value:updatedOption);
        }

        /* ############### DELETE ############*/
        [HttpDelete("{id:Guid:min(1)}")] 
        public async Task<ActionResult<OptionResponseDto>> Delete(Guid id)
        {
            await _MyOptionsHandler.RemoveOption(id);
            return NoContent();
        }
    }
}