
using System.Text.Json;
using ConfidentVotesAPI.Models;
using ConfidentVotesAPI.CustomExceptionHandling;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ConfidentVotesAPI.Services
{
    public interface IVotesHandler
    {
        public  Task addVote(Vote vote);
        public Task<Vote> GetVoteById(int id);
        public  Task RemoveVote(int id);
        public  Task UpdateVote(int id,Vote vote);
        public Task<List<Vote>> GetVotes();
    }
    public class VotesHandler : IVotesHandler
    {   
        public VotesHandler()
        {
            _ServiceId = new Random().Next(10, 200);
        }
        private readonly int _ServiceId;
        private List<Vote> Votes = new List<Vote>();
         public async Task<Vote> GetVoteById(int id)
        {
            var vote = Votes.FirstOrDefault(v => v.Id == id);
            if( vote == null)
            {
                throw new BadValueException($"{id}");
            }
            return vote;
        }
        public async Task addVote(Vote vote)
        {   
             if(vote == null)
            {
                throw new BadBodyException("Body can't be empty.");
            }
            vote.Id = Votes.Count +1;
            Votes.Add(vote);
        }

        // error handling hear : specific error handling
        public async Task RemoveVote(int id)

        {   
           
            var vote = Votes.FirstOrDefault(p => p.Id == id);
            if (vote == null)
            {
               throw new BadPathException($"{id}");
            }
            Votes.Remove(vote);
    
        }
        public async Task UpdateVote(int id, Vote newVote)
        {
            var vote = Votes.FirstOrDefault(p => p.Id == id);
            if( vote == null)
            {
               throw new BadPathException($"{id}");
            }
            vote.Name = newVote.Name;
            vote.Description = newVote.Description;
            
        }
        public async Task<List<Vote>> GetVotes()
        {
            return Votes;
        }

    }
}