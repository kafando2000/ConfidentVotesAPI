
using System.Text.Json;
using ConfidentVotesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using ConfidentVotesAPI.CustomExceptionHandling;
using Serilog;
using Microsoft.EntityFrameworkCore;
using ConfidentVotesAPI;

namespace ConfidentVotesAPI.Services
{
    public interface IChoicesHandler
    {   
        public Task<List<Choice>> GetChoices();
        public Task<Choice> GetChoiceById(int id);
        public  Task addChoice(Choice Choice);
        public  Task RemoveChoice(int id);
        public  Task UpdateChoice(int Id, Choice Choice);
        
    }
    public class ChoicesHandler : IChoicesHandler
    {   
        public ChoicesHandler(ConfidentVotesDbContext dbContext)
        {
            _ServiceId = new Random().Next(201, 2000);
            _DbContext = dbContext;
        }
        private readonly int _ServiceId;
        private ConfidentVotesDbContext _DbContext;
        // Choices list
        //private List<Choice> Choices = new List<Choice>();
        public async Task<Choice> GetChoiceById(int id)
        {
            var choice = await _DbContext.Choices.AsTracking()
                                             .FirstOrDefaultAsync<Choice>(p => p.ChoiceID == id);
            if( choice == null)
            {
                throw new BadValueException($"{id}");
            }
            return choice;
        }
        public async Task addChoice(Choice choice)
        {   
            if(choice == null)
            {
                throw new BadBodyException("Body can't be empty.");
            }
           _DbContext.Add(choice);
           await _DbContext.SaveChangesAsync();
        }

        // error handling hear : specific error handling
        public async Task RemoveChoice(int id)
        {  
            var Choice = await _DbContext.FindAsync<Choice>(id);
            if (Choice == null)
            {
               throw new BadPathException($"{id}");
            }
            _DbContext.Remove(Choice);
            await _DbContext.SaveChangesAsync();
    
        }
        public async Task UpdateChoice(int id, Choice choice)
        {
            var trackedChoice = await _DbContext.FindAsync<Choice>(id);
            if (trackedChoice == null)
            {
               throw new BadPathException($"{id}");
            }
            trackedChoice.Title = choice.Title;
           
            await _DbContext.SaveChangesAsync();
        }
        public async Task<List<Choice>> GetChoices()
        {
            return _DbContext.Choices.ToList();
        }

    }
}