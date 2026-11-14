
using System.Text.Json;
using ConfidentVotesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using ConfidentVotesAPI.CustomExceptionHandling;
using Serilog;
using Microsoft.EntityFrameworkCore;
using ConfidentVotesAPI;
using Microsoft.VisualBasic;

namespace ConfidentVotesAPI.Services
{
    public interface IChoicesHandler
    {   
        public Task<List<ChoiceResponseDto>> GetChoices();
        public Task<ChoiceResponseDto> GetChoiceById(int id);
        public  Task addChoice(ChoiceRequestDto Choice);
        public  Task RemoveChoice(int id);
        public  Task UpdateChoice(int Id, ChoiceRequestDto Choice);
        
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
        public async Task<ChoiceResponseDto> GetChoiceById(int id)
        {
            var choice = await _DbContext.Choices.Include(e => e.Votes)
            .FirstOrDefaultAsync(e => e.ChoiceID == id);
    
            if( choice == null)
            {
                throw new BadValueException($"{id}");
            }
            var choiceDto = choice.ToResponse();
            return choiceDto;
        }
        public async Task addChoice(ChoiceRequestDto choice)
        {   
            if(choice == null)
            {
                throw new BadBodyException("Body can't be empty.");
            }
            var dbchoice = choice.ToChoice();
           _DbContext.Add(dbchoice);
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
        public async Task UpdateChoice(int id, ChoiceRequestDto choice)
        {
            var trackedChoice = await _DbContext.FindAsync<Choice>(id);
            if (trackedChoice == null)
            {
               throw new BadPathException($"{id}");
            }
            trackedChoice.Title = choice.Title;
           
            await _DbContext.SaveChangesAsync();
        }
        public async Task<List<ChoiceResponseDto>> GetChoices()
        {
            return _DbContext.Choices.Select( choice => choice.ToResponse()).ToList();

       }
    }
}