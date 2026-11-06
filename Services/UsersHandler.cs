
using System.Text.Json;
using ConfidentVotesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using ConfidentVotesAPI.CustomExceptionHandling;
using Serilog;

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
        public UsersHandler()
        {
            _ServiceId = new Random().Next(201, 2000);
        }
        private readonly int _ServiceId;
        // users list
        private List<Person> Users = new List<Person>();
        public async Task<Person> GetUserById(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if( user == null)
            {
                throw new BadValueException($"{id}");
            }
            return user;
        }
        public async Task addUser(Person user)
        {   
            user.Id = Users.Count + 1;
            if(user == null)
            {
                throw new BadBodyException("Body can't be empty.");
            }
            Users.Add(user);
        }

        // error handling hear : specific error handling
        public async Task RemoveUser(int id)
        {  
            var person = Users.FirstOrDefault(p => p.Id == id);
            if (person == null)
            {
               throw new BadPathException($"{id}");
            }
            Users.Remove(person);
    
        }
        public async Task UpdateUser(int id,Person user)
        {
            var person = Users.FirstOrDefault(p => p.Id == id);
            if (person == null)
            {
               throw new BadPathException($"{id}");
            }
            person.FullName = user.FullName;
            person.Email = user.Email;
            person.UserName = user.UserName;
        }
        public async Task<List<Person>> GetUsers()
        {
            return Users;
        }

    }
}