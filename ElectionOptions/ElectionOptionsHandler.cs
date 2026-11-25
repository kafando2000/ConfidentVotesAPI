
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
    public interface IElectionOptionsHandler
    {   
        public Task<List<ElectionOptionResponseDto>> GetElectionOptions();
        public Task<ElectionOptionResponseDto> GetElectionOptionById(int id);
        public  Task addElectionOption(ElectionOptionRequestDto ElectionOption);
        public  Task RemoveElectionOption(int id);
        public  Task UpdateElectionOption(int Id, ElectionOptionRequestDto ElectionOption);
        
    }
    public class ElectionOptionsHandler : IElectionOptionsHandler
    {   
        public ElectionOptionsHandler(ConfidentVotesDbContext dbContext)
        {
            _ServiceId = new Random().Next(201, 2000);
            _DbContext = dbContext;
        }
        private readonly int _ServiceId;
        private ConfidentVotesDbContext _DbContext;
        // ElectionOptions list
        //private List<ElectionOption> ElectionOptions = new List<ElectionOption>();
        public async Task<ElectionOptionResponseDto> GetElectionOptionById(int id)
        {
            var ElectionOption = await _DbContext.ElectionOptions.Include(e => e.Votes)
            .FirstOrDefaultAsync(e => e.Id == id);
    
            if( ElectionOption == null)
            {
                throw new BadValueException($"{id}");
            }
            var ElectionOptionDto = ElectionOption.ToResponse();
            return ElectionOptionDto;
        }
        public async Task addElectionOption(ElectionOptionRequestDto ElectionOption)
        {   
            if(ElectionOption == null)
            {
                throw new BadBodyException("Body can't be empty.");
            }
            var dbElectionOption = ElectionOption.ToElectionOption();
           _DbContext.Add(dbElectionOption);
           await _DbContext.SaveChangesAsync();
        }

        // error handling hear : specific error handling
        public async Task RemoveElectionOption(int id)
        {  
            var ElectionOption = await _DbContext.FindAsync<ElectionOption>(id);
            if (ElectionOption == null)
            {
               throw new BadPathException($"{id}");
            }
            _DbContext.Remove(ElectionOption);
            await _DbContext.SaveChangesAsync();
    
        }
        public async Task UpdateElectionOption(int id, ElectionOptionRequestDto ElectionOption)
        {
            var trackedElectionOption = await _DbContext.FindAsync<ElectionOption>(id);
            if (trackedElectionOption == null)
            {
               throw new BadPathException($"{id}");
            }
            trackedElectionOption.Title = ElectionOption.Title;
           
            await _DbContext.SaveChangesAsync();
        }
        public async Task<List<ElectionOptionResponseDto>> GetElectionOptions()
        {
            return _DbContext.ElectionOptions.Select( ElectionOption => ElectionOption.ToResponse()).ToList();

       }
    }
}