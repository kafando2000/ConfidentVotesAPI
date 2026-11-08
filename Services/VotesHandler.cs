
using System.Text.Json;
using ConfidentVotesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using ConfidentVotesAPI.CustomExceptionHandling;
using Serilog;
using Microsoft.EntityFrameworkCore;
using ConfidentVotesAPI;

namespace ConfidentVotesAPI.Services
{
    public interface IVotesHandler
    {   
        public Task<List<Vote>> GetVotes();
        public Task<Vote> GetVoteById(int id);
        public  Task addVote(VoteDto Vote);
        public  Task RemoveVote(int id);
        public  Task UpdateVote(int Id, VoteDto Vote);
        
    }
    public class VotesHandler : IVotesHandler
    {   
        public VotesHandler(ConfidentVotesDbContext dbContext)
        {
            _ServiceId = new Random().Next(201, 2000);
            _DbContext = dbContext;
        }
        private readonly int _ServiceId;
        private ConfidentVotesDbContext _DbContext;
        // Votes list
        //private List<Vote> Votes = new List<Vote>();
        public async Task<Vote> GetVoteById(int id)
        {
            var vote = await _DbContext.Votes.AsTracking()
                                             .FirstOrDefaultAsync(p => p.VoteID == id);
            if( vote == null)
            {
                throw new BadValueException($"{id}");
            }
            return vote;
        }
        public async Task addVote(VoteDto newVote)
        {   
            if(newVote == null)
            {
                throw new BadBodyException("Body can't be empty.");
            }
            var dbvote = new Vote
            {
                VoteID = newVote.VoteID,
                ElectionVoterID = newVote.ElectionVoterID,
                ChoiceID = newVote.ChoiceID
            };
           _DbContext.Add(dbvote);
           await _DbContext.SaveChangesAsync();
        }

        // error handling hear : specific error handling
        public async Task RemoveVote(int id)
        {  
            var vote = await _DbContext.FindAsync<Vote>(id);
            if (vote == null)
            {
               throw new BadPathException($"{id}");
            }
            _DbContext.Remove(vote);
            await _DbContext.SaveChangesAsync();
    
        }
        public async Task UpdateVote(int id,VoteDto Vote)
        {
            var trackedVote = await _DbContext.FindAsync<Vote>(id);
            if (trackedVote == null)
            {
               throw new BadPathException($"{id}");
            }
            trackedVote.ChoiceID = Vote.ChoiceID;
            trackedVote.VoteDate = Vote.VoteDate;
            await _DbContext.SaveChangesAsync();
        }
        public async Task<List<Vote>> GetVotes()
        {
            return _DbContext.Votes.ToList();
        }

    }
}