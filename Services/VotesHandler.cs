
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
        public Task<List<VoteResponseDto>> GetVotes();
        public Task<VoteResponseDto> GetVoteById(int id);
        public  Task addVote(VoteRequestDto Vote);
        public  Task RemoveVote(int id);
        public  Task UpdateVote(int Id, VoteRequestDto Vote);
        
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
        public async Task<VoteResponseDto> GetVoteById(int id)
        {
            var vote = await _DbContext.Votes.AsNoTracking()
                                            .Select(vote => vote.ToResponse())
                                             .FirstOrDefaultAsync(p => p.VoteID == id);
            
            
            if( vote == null)
            {
                throw new BadValueException($"{id}");
            }
            return vote;
        }
        public async Task addVote(VoteRequestDto newVote)
        {   
            Console.WriteLine("entered addvote");
            if(newVote == null)
            {
                throw new BadBodyException("Body can't be empty.");
            }
            var dbvote = newVote.ToVote();
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
        public async Task UpdateVote(int id,VoteRequestDto Vote)
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
        public async Task<List<VoteResponseDto>> GetVotes()
        {
            return await _DbContext.Votes.Select(vote => vote.ToResponse())
            .ToListAsync();
        }

    }
}