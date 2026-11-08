
using System.Text.Json;
using ConfidentVotesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using ConfidentVotesAPI.CustomExceptionHandling;
using Serilog;
using Microsoft.EntityFrameworkCore;
using ConfidentVotesAPI;

namespace ConfidentVotesAPI.Services
{
    public interface IUsersHandler
    {   
        public Task<List<Person>> GetUsers();
        public Task<Person> GetUserById(int id);
        public  Task addUser(Person user);
        public  Task RemoveUser(int id);
        public  Task UpdateUser(int Id, Person user);
        
    }
    public class UsersHandler : IUsersHandler
    {   
        public UsersHandler(ConfidentVotesDbContext dbContext)
        {
            _ServiceId = new Random().Next(201, 2000);
            _DbContext = dbContext;
        }
        private readonly int _ServiceId;
        private ConfidentVotesDbContext _DbContext;
        // users list
        //private List<Person> Users = new List<Person>();
        public async Task<Person> GetUserById(int id)
        {
            var user = await _DbContext.Users.AsTracking()
                                             .FirstOrDefaultAsync<Person>(p => p.PersonID == id);
            if( user == null)
            {
                throw new BadValueException($"{id}");
            }
            return user;
        }
        public async Task addUser(Person user)
        {   
            if(user == null)
            {
                throw new BadBodyException("Body can't be empty.");
            }
           _DbContext.Add(user);
           await _DbContext.SaveChangesAsync();
        }

        // error handling hear : specific error handling
        public async Task RemoveUser(int id)
        {  
            var user = await _DbContext.FindAsync<Person>(id);
            if (user == null)
            {
               throw new BadPathException($"{id}");
            }
            _DbContext.Remove(user);
            await _DbContext.SaveChangesAsync();
    
        }
        public async Task UpdateUser(int id,Person user)
        {
            var trackedUser = await _DbContext.FindAsync<Person>(id);
            if (trackedUser == null)
            {
               throw new BadPathException($"{id}");
            }
            trackedUser.FirstName = user.FirstName;
            trackedUser.LastName = user.LastName;
            trackedUser.Email = user.Email;
            trackedUser.UserName = user.UserName;
            await _DbContext.SaveChangesAsync();
        }
        public async Task<List<Person>> GetUsers()
        {
            return _DbContext.Users.ToList();
        }

    }
}