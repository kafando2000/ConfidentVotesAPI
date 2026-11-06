

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
        public async Task<ActionResult<List<Person>>> GetAll()
        {
            var users = await _MyUsersHandler.GetUsers();
            return Ok(users);
        }
         [HttpGet("{id:int:min(0)}")]
        public async Task<ActionResult<Person>> GetbyId(int id)
        {
            var user = await _MyUsersHandler.GetUserById(id);
            return Ok(user);
            
        }
        [HttpPost] 
         /* ############### POST ############*/
        public async Task<ActionResult<Person>> Create( [FromBody] Person newUser)
        {
            var users = await  _MyUsersHandler.GetUsers();
            var userId = users.Count + 1;
            newUser.Id = userId;
            await _MyUsersHandler.addUser(newUser);
            return AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = newUser.Id}, 
                       value:newUser);
        }
        /* ############### PUT ############*/
        [HttpPut("{id:int:min(0)}")] 
        public async Task<ActionResult<Person>> Update(int id, Person updatedVote)
        {
           
            await _MyUsersHandler.UpdateUser(id, updatedVote);
            return Ok(updatedVote);
        }

        /* ############### DELETE ############*/
        [HttpDelete("{id:int:min(1)}")] 
        public async Task<ActionResult<Person>> Delete(int id)
        {
            await _MyUsersHandler.RemoveUser(id);
            return NoContent();
        }
    }
}