
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
    public interface IVotesHandler
    {   
        public Task<HashSet<VoteResponseDto>> GetVotesAsync();
        public Task<VoteResponseDto?> GetVoteByIdAsync(Guid id);
        public  Task<VoteResponseDto?> addVoteAsync(VoteRequestDto Vote);
        public  Task<bool> RemoveVoteAsync(Guid id);
        public  Task<VoteResponseDto?> UpdateVoteAsync(Guid Id, VoteRequestDto Vote);
        
    }
    public class VotesHandler : IVotesHandler
    {   
        public VotesHandler(ConfidentVotesDbContext dbContext, IOptionsHandler optionsHandler, 
              IVotersHandler votersHandler )
        {
            _ServiceId = new Random().Next(5000, 6000);
            _DbContext = dbContext;
            _OptionsHandler = optionsHandler;
            _VotersHandler = votersHandler;
        }
        private readonly int _ServiceId;
        private ConfidentVotesDbContext _DbContext;
        private IOptionsHandler _OptionsHandler;
        private IVotersHandler _VotersHandler;
        // Votes list
        //private List<Vote> Votes = new List<Vote>();
        public async Task<VoteResponseDto?> GetVoteByIdAsync(Guid id)
        {
            var vote = await _DbContext.Votes
                                       .FirstOrDefaultAsync(p => p.Id == id);
            return vote?.ToResponse();
        }
        public async Task<VoteResponseDto?> addVoteAsync(VoteRequestDto newVote)
        {   
            var relatedVoter = await _VotersHandler.GetVoterByIdAsync(newVote.VoterId);
            var relatedOption = await  _OptionsHandler.GetOptionByIdAsync(newVote.OptionId);
            var existingVote = await _DbContext.Votes.FirstOrDefaultAsync(e => 
                                                                    e.VoterId == newVote.VoterId && 
                                                                    e.OptionId == newVote.OptionId);
            if(relatedVoter == null || relatedOption == null || existingVote != null )
            {
                return null;
            }
            var dbVote = newVote.ToVote();
           _DbContext.Add(dbVote);
           await _DbContext.SaveChangesAsync();
           return dbVote.ToResponse();
        }
        // error handling hear : specific error handling
        public async Task<bool> RemoveVoteAsync(Guid id)
        {  
            bool isDeleted = false;
            var vote = await _DbContext.FindAsync<Vote>(id);
            if (vote == null)
            {
               return isDeleted;
            }
            _DbContext.Remove(vote);
            await _DbContext.SaveChangesAsync();
            return !isDeleted;
    
        }
        public async Task<VoteResponseDto?> UpdateVoteAsync(Guid id, VoteRequestDto updatedVote)
        {
            var trackedVote = await _DbContext.FindAsync<Vote>(id);
            var updatedVoteOption = await  _OptionsHandler.GetOptionByIdAsync(updatedVote.OptionId);
            if (trackedVote == null || trackedVote.VoterId != updatedVote.VoterId || updatedVoteOption == null)
            {
               return null;
            }
            var trackedVoteOption = await  _OptionsHandler.GetOptionByIdAsync(trackedVote.OptionId);
            if( trackedVoteOption == null || trackedVoteOption.ElectionId != updatedVoteOption.ElectionId)
            {
                return null;
            }
            trackedVote.OptionId = updatedVote.OptionId;
            trackedVote.Confidence = updatedVote.Confidence;
            await _DbContext.SaveChangesAsync();
            return trackedVote.ToResponse();
        }
        public async Task<HashSet<VoteResponseDto>> GetVotesAsync()
        {
            return await _DbContext.Votes.Select(ev => ev.ToResponse()).ToHashSetAsync();
        }

    }
}