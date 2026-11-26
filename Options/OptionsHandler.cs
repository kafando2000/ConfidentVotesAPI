
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
    public interface IOptionsHandler
    {   
        public Task<List<OptionResponseDto>> GetOptions();
        public Task<OptionResponseDto?> GetOptionById(Guid id);
        public  Task<OptionResponseDto?> addOption(OptionRequestDto Option);
        public  Task<bool> RemoveOption(Guid id);
        public  Task<OptionResponseDto?> UpdateOption(Guid Id, OptionRequestDto Option);
        
    }
    public class OptionsHandler : IOptionsHandler
    {   
        public OptionsHandler(ConfidentVotesDbContext dbContext)
        {
            _ServiceId = new Random().Next(201, 2000);
            _DbContext = dbContext;
        }
        private readonly int _ServiceId;
        private ConfidentVotesDbContext _DbContext;
        // Options list
        //private List<Option> Options = new List<Option>();
        public async Task<OptionResponseDto?> GetOptionById(Guid id)
        {
            var option = await _DbContext.Options.Include(e => e.Votes)
            .FirstOrDefaultAsync(e => e.Id == id);
    
            return option == null ? null : option.ToResponse();
        }
        public async Task<OptionResponseDto?>  addOption(OptionRequestDto Option)
        {   
            if(Option == null)
            {
                throw new BadBodyException("Body can't be empty.");
            }
            var dbOption = Option.ToOption();
           _DbContext.Add(dbOption);
           await _DbContext.SaveChangesAsync();
        }

        // error handling hear : specific error handling
        public async Task RemoveOption(Guid id)
        {  
            var Option = await _DbContext.FindAsync<Option>(id);
            if (Option == null)
            {
               throw new BadPathException($"{id}");
            }
            _DbContext.Remove(Option);
            await _DbContext.SaveChangesAsync();
    
        }
        public async Task UpdateOption(Guid id, OptionRequestDto Option)
        {
            var trackedOption = await _DbContext.FindAsync<Option>(id);
            if (trackedOption == null)
            {
               throw new BadPathException($"{id}");
            }
            trackedOption.Title = Option.Title;
           
            await _DbContext.SaveChangesAsync();
        }
        public async Task<List<OptionResponseDto>> GetOptions()
        {
            return _DbContext.Options.Select( Option => Option.ToResponse()).ToList();

       }
    }
}