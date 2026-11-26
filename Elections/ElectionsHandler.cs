
using System.Text.Json;
using ConfidentVotesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using ConfidentVotesAPI.CustomExceptionHandling;
using Serilog;
using Microsoft.EntityFrameworkCore;
using ConfidentVotesAPI;
using Microsoft.AspNetCore.Identity;

namespace ConfidentVotesAPI.Services
{
    public interface IElectionsHandler
    {   
        public Task<HashSet<ElectionResponseDto>> GetElectionsAsync();
        public Task<ElectionResponseDto?> GetElectionByIdAsync(Guid id);
        public  Task<ElectionResponseDto?> addElectionAsync(ElectionRequestDto election);
        public  Task<bool> RemoveElectionAsync(Guid id);
        public  Task<ElectionResponseDto?> UpdateElectionAsync(Guid id, ElectionRequestDto election);
        public Task<HashSet<OptionResponseDto>> GetElectionOptionsAsync(Guid electionId);
        
    }
    public class ElectionsHandler : IElectionsHandler
    {   
        public ElectionsHandler(ConfidentVotesDbContext dbContext, UserManager<AppUser> userManager)
        {
            _ServiceId = new Random().Next(201, 2000);
            _DbContext = dbContext;
            _UserManager = userManager;
        }
        private readonly int _ServiceId;
        private ConfidentVotesDbContext _DbContext;
        private UserManager<AppUser> _UserManager;
        // get an election by id
        public async Task<ElectionResponseDto?> GetElectionByIdAsync(Guid id)
        {
            var election = await _DbContext.Elections.Include(e => e.Options)
                                             .ThenInclude(c => c.Votes)
                                             .FirstOrDefaultAsync(p => p.Id == id);
            if( election == null)
            {
                return null;
            }
          
            return  election.ToResponse();
        }
        // add a new election
        public async Task<ElectionResponseDto?> addElectionAsync(ElectionRequestDto newElection)
        {   
            if(newElection.AppUserId == Guid.Empty || 
                await _UserManager.FindByIdAsync(newElection.AppUserId.ToString()) == null ||
                newElection.ElectionEnd == newElection.ElectionStart
                )
            {
                return null;
            }
            var dbelection = newElection.ToElection();
           _DbContext.Add(dbelection);
           await _DbContext.SaveChangesAsync();
           return dbelection.ToResponse();
        }
        // delete an election  
        public async Task<bool> RemoveElectionAsync(Guid id)
        {  
            bool isRemoved = false;
            var election = await _DbContext.FindAsync<Election>(id);
            if (election == null)
            {
               return isRemoved;
            }
            _DbContext.Remove(election);

            await _DbContext.SaveChangesAsync();
            isRemoved = true;
            return isRemoved;
    
        }
        // update an election  
        public async Task<ElectionResponseDto?> UpdateElectionAsync(Guid id, ElectionRequestDto updatedElection)
        {
            var trackedElection = await _DbContext.FindAsync<Election>(id);
            if (trackedElection == null ||
             updatedElection.ElectionEnd == updatedElection.ElectionStart
             )
            {
               return null;
            }
            trackedElection.ElectionEnd = updatedElection.ElectionEnd;
            trackedElection.ElectionStart = updatedElection.ElectionStart;
            trackedElection.Name = updatedElection.Name;
            trackedElection.Description = updatedElection.Description;
            await _DbContext.SaveChangesAsync();
            return trackedElection.ToResponse();
        }
        // get all election 
        public async Task<HashSet<ElectionResponseDto>> GetElectionsAsync()
        {
            return await _DbContext.Elections.Select(election => election.ToResponse())
                                             .ToHashSetAsync();
                
        }
        // get all options related to an election, with their votes
        public async Task<HashSet<OptionResponseDto>> GetElectionOptionsAsync(Guid electionId)
        {
            
            var options = await _DbContext.Options.Include(c => c.Votes)
                .Where(Option => Option.ElectionId == electionId)
                .Select(Option => Option.ToResponse())
                .ToHashSetAsync();
            return options;
        }

    }
}