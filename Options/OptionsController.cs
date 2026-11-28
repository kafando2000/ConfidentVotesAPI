

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ConfidentVotesAPI.Models;
using System.Reflection;
using System.Text.Json;
using ConfidentVotesAPI.Services;

namespace ConfidentVotesAPI.Controllers
{
    [ApiController]

    //[Route("confidentvotesapi/[controller]")]
    [Route("confidentvotes/api/options")]
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
        [Authorize]
        public async Task<ActionResult<HashSet<OptionResponseDto>>> GetAll()
        {
            return Ok(await _MyOptionsHandler.GetOptionsAsync());
        }
        [HttpGet("{id:Guid}")]
        [Authorize]
        public async Task<ActionResult<OptionResponseDto>> GetbyId(Guid id)
        {
            var option = await _MyOptionsHandler.GetOptionByIdAsync(id);
            return option != null ? Ok(option) : BadRequest(@$"Check your data, something is wrong. 
            The likely cause is that
            the provided id : {id} is not related to any option");
            
        } 
         /* ############### POST ############*/
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OptionResponseDto>> Create( [FromBody] OptionRequestDto newOption)
        {
            var createdOption = await _MyOptionsHandler.addOptionAsync(newOption);
            if(createdOption == null)
            {
                return BadRequest($@"Something is wrong :
                The likely causes are :
                    1. Id of Election: {newOption.ElectionId} is not related to any election
                    2. A conflict of naming has surged if you tried to have two options with same title in the same election");
            }
            return AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = createdOption.Id}, 
                       value:newOption);
        }
        /* ############### PUT ############*/
        [HttpPut("{id:Guid}")] 
        [Authorize]
        public async Task<ActionResult<OptionResponseDto>> Update(Guid id, OptionRequestDto updatedOption)
        {
            var option = await _MyOptionsHandler.UpdateOptionAsync(id, updatedOption);
            if( option == null)
            {
                return BadRequest($@"Something is wrong :
                The likely causes are :
                    1. Id :{id} is not related to any option
                    2. Id of Election: {updatedOption.ElectionId} is not related to any election
                    3. A conflict of naming has surged if you tried to have two options with same title in the same election
                    4. You are not the owner of the election");
            }
            return AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = option.Id}, 
                       value:option);
        }

        /* ############### DELETE ############*/
        [HttpDelete("{id:Guid}")] 
        [Authorize]
        public async Task<ActionResult<OptionResponseDto>> Delete(Guid id)
        {
            var isDeleted = await _MyOptionsHandler.RemoveOptionAsync(id);
            
            return isDeleted ? NoContent() : 
                                        BadRequest(@$"Check your data, something is wrong. 
                                        The likely cause is that
                                        the provided id : {id} is not related to any option");
        }
    }
}