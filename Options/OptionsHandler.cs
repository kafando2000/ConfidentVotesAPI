
using System.Text.Json;
using ConfidentVotesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using ConfidentVotesAPI.CustomExceptionHandling;
using Serilog;
using Microsoft.EntityFrameworkCore;
using ConfidentVotesAPI;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace ConfidentVotesAPI.Services
{
    public interface IOptionsHandler
    {   
        public Task<HashSet<OptionResponseDto>> GetOptionsAsync();
        public Task<OptionResponseDto?> GetOptionByIdAsync(Guid id);
        public  Task<OptionResponseDto?> addOptionAsync(OptionRequestDto Option);
        public  Task<bool> RemoveOptionAsync(Guid id);
        public  Task<OptionResponseDto?> UpdateOptionAsync(Guid Id, OptionRequestDto Option);
        
    }
    public class OptionsHandler : IOptionsHandler
    {   
        public OptionsHandler(ConfidentVotesDbContext dbContext, IElectionsHandler electionsHandler)
        {
            _ServiceId = new Random().Next(201, 2000);
            _DbContext = dbContext;
            _ElectionsHandler = electionsHandler;

        }
        private readonly int _ServiceId;
        private ConfidentVotesDbContext _DbContext;
        private IElectionsHandler _ElectionsHandler;
        // Options list
        //private List<Option> Options = new List<Option>();
        public async Task<OptionResponseDto?> GetOptionByIdAsync(Guid id)
        {
            var option = await _DbContext.Options.Include(e => e.Votes)
            .FirstOrDefaultAsync(e => e.Id == id);
    
            return option?.ToResponse();
        }
        public async Task<OptionResponseDto?>  addOptionAsync(OptionRequestDto newOption)
        {   
            if(string.IsNullOrWhiteSpace(newOption.Title) || 
                await _ElectionsHandler.GetElectionByIdAsync(newOption.ElectionId) == null 
               
            )
            {
                 return null;
            }
            // check if this option is alread registered for the same election
            var existingOption =  _DbContext.Options.Where(o => 
                                        o.ElectionId == newOption.ElectionId 
                                        && o.Title == newOption.Title);
            if (existingOption != null)
            {
                return null;
            }
            var dbOption =  newOption.ToOption();
           _DbContext.Add(dbOption);
            await _DbContext.SaveChangesAsync();
            return dbOption.ToResponse();
        }

        public async Task<bool> RemoveOptionAsync(Guid id)
        {  
            bool isDeleted = false;
            var Option = await _DbContext.FindAsync<Option>(id);
            if (Option == null)
            {
               return isDeleted;
            }
            _DbContext.Remove(Option);
            await _DbContext.SaveChangesAsync();

            return !isDeleted;
    
        }
        public async Task<OptionResponseDto?> UpdateOptionAsync(Guid id, OptionRequestDto updatedOption)
        {
            var relatedElection = await _ElectionsHandler.GetElectionByIdAsync(updatedOption.ElectionId);
            // check if related election exists
            if(string.IsNullOrWhiteSpace(updatedOption.Title) || 
               relatedElection == null 
            )
            {
                 return null;
            }
            var trackedOption = await _DbContext.FindAsync<Option>(id);
            // check if there is an option or if found option is related to the updateOptionAsync election
            if (trackedOption == null || trackedOption.ElectionId != relatedElection.Id)
            {
               return null;
            }
            // check if an option of the same election already have this title
            var existingOption =  _DbContext.Options.Where(o => 
                                        o.ElectionId == trackedOption.ElectionId 
                                        && o.Title == updatedOption.Title);
            if (existingOption != null)
            {
                return null;
            }
            trackedOption.Title = updatedOption.Title;
           
            await _DbContext.SaveChangesAsync();
            return trackedOption.ToResponse();
        }
        public async Task<HashSet<OptionResponseDto>> GetOptionsAsync()
        {
            return await _DbContext.Options.Select( Option => Option.ToResponse()).ToHashSetAsync();

       }
    }
}