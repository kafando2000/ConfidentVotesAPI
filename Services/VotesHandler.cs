
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
        public  Task addVote(Vote Vote);
        public  Task RemoveVote(int id);
        public  Task UpdateVote(int Id, Vote Vote);
        
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
                                             .FirstOrDefaultAsync<Vote>(p => p.VoteID == id);
            if( vote == null)
            {
                throw new BadValueException($"{id}");
            }
            return vote;
        }
        public async Task addVote(Vote newVote)
        {   
            if(newVote == null)
            {
                throw new BadBodyException("Body can't be empty.");
            }
           _DbContext.Add(newVote);
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
        public async Task UpdateVote(int id,Vote Vote)
        {
            var trackedVote = await _DbContext.FindAsync<Vote>(id);
            if (trackedVote == null)
            {
               throw new BadPathException($"{id}");
            }
            trackedVote.ChoiceID = Vote.ChoiceID;
            await _DbContext.SaveChangesAsync();
        }
        public async Task<List<Vote>> GetVotes()
        {
            return _DbContext.Votes.ToList();
        }

    }
}