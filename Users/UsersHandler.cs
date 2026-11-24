
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
        public Task<List<PersonResponseDto>> GetUsers();
        public Task<PersonResponseDto> GetUserById(int id);
        public  Task addUser(PersonRequestDto user);
        public  Task RemoveUser(int id);
        public  Task UpdateUser(int Id, PersonRequestDto user);
        
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
        public async Task<PersonResponseDto> GetUserById(int id)
        {
            var user = await _DbContext.Users
            .Include(u => u.ElectionsVoters)
            .Include(u => u.InitiatedElections)
            .FirstOrDefaultAsync();
            if(user == null)
            {
                throw new BadBodyException("Body can't be empty.");
            }
            return user.ToResponse();
        }
        public async Task addUser(PersonRequestDto user)
        {   
            if(user == null)
            {
                throw new BadBodyException("Body can't be empty.");
            }
           _DbContext.Add(user.ToPerson());
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
        public async Task UpdateUser(int id,PersonRequestDto user)
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
            trackedUser.Password = user.Password;
            await _DbContext.SaveChangesAsync();
        }
        public async Task<List<PersonResponseDto>> GetUsers()
        {
            return _DbContext.Users
                                   .Select(e => e.ToResponse()).ToList();
        }

    }
}