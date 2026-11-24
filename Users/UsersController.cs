

using Microsoft.AspNetCore.Mvc;
using ConfidentVotesAPI.Models;
using System.Reflection;
using System.Text.Json;
using ConfidentVotesAPI.Services;

namespace ConfidentUsersAPI.Controllers
{
    [ApiController]

    //[Route("confidentvotesapi/[controller]")]
    [Route("confidentvotesapi/users")]
    // the class must be public
    public class UsersController : ControllerBase
    {   
     
        public required IUsersHandler  _MyUsersHandler ;

        // when the controller is create the only one instance of 
        // IProductsHandler will be used or reused
        public UsersController(IUsersHandler UsersHandler)
        {
            _MyUsersHandler = UsersHandler;
        }
        /* ############### GET ############*/
        [HttpGet]
        public async Task<ActionResult<List<PersonResponseDto>>> GetAll()
        {
            var users = await _MyUsersHandler.GetUsers();
            return Ok(users);
        }
         [HttpGet("{id:int:min(0)}")]
        public async Task<ActionResult<PersonResponseDto>> GetbyId(int id)
        {
            var user = await _MyUsersHandler.GetUserById(id);
            return Ok(user);
            
        }
        [HttpPost] 
         /* ############### POST ############*/
        public async Task<ActionResult<PersonResponseDto>> Create( [FromBody] PersonRequestDto newUser)
        {
            await _MyUsersHandler.addUser(newUser);
            return AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = newUser.PersonID}, 
                       value:newUser);
        }
        /* ############### PUT ############*/
        [HttpPut("{id:int:min(0)}")] 
        public async Task<ActionResult<PersonResponseDto>> Update(int id, PersonRequestDto updatedPerson)
        {
           
            await _MyUsersHandler.UpdateUser(id, updatedPerson);
            return AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = updatedPerson.PersonID}, 
                       value:updatedPerson);
        }

        /* ############### DELETE ############*/
        [HttpDelete("{id:int:min(1)}")] 
        public async Task<ActionResult<PersonResponseDto>> Delete(int id)
        {
            await _MyUsersHandler.RemoveUser(id);
            return NoContent();
        }
    }
}