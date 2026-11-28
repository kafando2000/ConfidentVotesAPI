
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
    public interface IVotersHandler
    {   
        public Task<List<VoterResponseDto>> GetVotersAsync();
        public Task<VoterResponseDto?> GetVoterByIdAsync(Guid id);
        public  Task<VoterResponseDto?> addVoterAsync(VoterRequestDto Voter);
        public  Task<bool> RemoveVoterAsync(Guid id);
        public  Task<VoterResponseDto?> UpdateVoterAsync(Guid Id, VoterRequestDto Voter);
        
    }
    public class VotersHandler : IVotersHandler
    {   
        public VotersHandler(ConfidentVotesDbContext dbContext, UserManager<AppUser> userManager, 
              IElectionsHandler electionsHandler )
        {
            _ServiceId = new Random().Next(5000, 6000);
            _DbContext = dbContext;
            _UserManager = userManager;
            _ElectionsHandler = electionsHandler;
        }
        private readonly int _ServiceId;
        private ConfidentVotesDbContext _DbContext;
        private UserManager<AppUser> _UserManager;
        private IElectionsHandler _ElectionsHandler;
        // Voters list
        //private List<Voter> Voters = new List<Voter>();
        public async Task<VoterResponseDto?> GetVoterByIdAsync(Guid id)
        {
            var voter = await _DbContext.Voters
                                                .FirstOrDefaultAsync(p => 
                                                    p.Id == id);

         
            return voter?.ToResponse() ?? null;
        }
        public async Task<VoterResponseDto?> addVoterAsync(VoterRequestDto newVoter)
        {   
            var relatedUser = await _UserManager.Users.FirstOrDefaultAsync(u => u.Id == newVoter.AppUserId);
            var relatedElection = await  _ElectionsHandler.GetElectionByIdAsync(newVoter.ElectionId);
            if(relatedElection == null || relatedUser == null )
            {
                return null;
            }
            var dbVoter = newVoter.ToVoter();
           _DbContext.Add(dbVoter);
           await _DbContext.SaveChangesAsync();
           return dbVoter.ToResponse();
        }
        // error handling hear : specific error handling
        public async Task<bool> RemoveVoterAsync(Guid id)
        {  
            bool isDeleted = false;
            var voter = await _DbContext.FindAsync<Voter>(id);
            if (voter == null)
            {
               return isDeleted;
            }
            _DbContext.Remove(voter);
            await _DbContext.SaveChangesAsync();
            return !isDeleted;
    
        }
        public async Task<VoterResponseDto?> UpdateVoterAsync(Guid id, VoterRequestDto updatedVoter)
        {
            var trackedVoter = await _DbContext.FindAsync<Voter>(id);
            var relatedUser = await _UserManager.Users.FirstOrDefaultAsync(u => u.Id == updatedVoter.AppUserId);
            if (trackedVoter == null || relatedUser == null)
            {
               return null;
            }
            trackedVoter.AppUserId = updatedVoter.AppUserId;
            trackedVoter.Weight = updatedVoter.Weight;
            await _DbContext.SaveChangesAsync();
            return trackedVoter.ToResponse();
        }
        public async Task<List<VoterResponseDto>> GetVotersAsync()
        {
            return _DbContext.Voters.Select(ev => ev.ToResponse()).ToList();
        }

    }
}