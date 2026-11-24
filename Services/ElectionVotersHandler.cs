
using System.Text.Json;
using ConfidentVotesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using ConfidentVotesAPI.CustomExceptionHandling;
using Serilog;
using Microsoft.EntityFrameworkCore;
using ConfidentVotesAPI;

namespace ConfidentVotesAPI.Services
{
    public interface IElectionVotersHandler
    {   
        public Task<List<ElectionVoterResponseDto>> GetElectionVoters();
        public Task<ElectionVoterResponseDto> GetElectionVoterById(int id);
        public  Task addElectionVoter(ElectionVoterRequestDto electionVoter);
        public  Task RemoveElectionVoter(int id);
        public  Task UpdateElectionVoter(int Id, ElectionVoterRequestDto electionVoter);
        
    }
    public class ElectionVotersHandler : IElectionVotersHandler
    {   
        public ElectionVotersHandler(ConfidentVotesDbContext dbContext)
        {
            _ServiceId = new Random().Next(5000, 6000);
            _DbContext = dbContext;
        }
        private readonly int _ServiceId;
        private ConfidentVotesDbContext _DbContext;
        // ElectionVoters list
        //private List<ElectionVoter> ElectionVoters = new List<ElectionVoter>();
        public async Task<ElectionVoterResponseDto> GetElectionVoterById(int id)
        {
            var electionVoter = await _DbContext.ElectionVoters
                                                .FirstOrDefaultAsync(p => 
                                                    p.ElectionVoterID == id);

            if( electionVoter == null)
            {
                throw new BadValueException($"{id}");
            }
            return electionVoter.ToResponse();
        }
        public async Task addElectionVoter(ElectionVoterRequestDto electionVoter)
        {   
            if(electionVoter == null)
            {
                throw new BadBodyException("Body can't be empty.");
            }
            var dbelectionVoter = electionVoter.ToElectionVoter();
           _DbContext.Add(dbelectionVoter);
           await _DbContext.SaveChangesAsync();
        }

        // error handling hear : specific error handling
        public async Task RemoveElectionVoter(int id)
        {  
            var electionVoter = await _DbContext.FindAsync<ElectionVoter>(id);
            if (electionVoter == null)
            {
               throw new BadPathException($"{id}");
            }
            _DbContext.Remove(electionVoter);
            await _DbContext.SaveChangesAsync();
    
        }
        public async Task UpdateElectionVoter(int id, ElectionVoterRequestDto electionVoter)
        {
            var trackedElectionVoter = await _DbContext.FindAsync<ElectionVoter>(id);
            if (trackedElectionVoter == null)
            {
               throw new BadPathException($"{id}");
            }
            trackedElectionVoter.ElectionID = electionVoter.ElectionID;
            trackedElectionVoter.PersonID = electionVoter.PersonID;
            await _DbContext.SaveChangesAsync();
        }
        public async Task<List<ElectionVoterResponseDto>> GetElectionVoters()
        {
            return _DbContext.ElectionVoters.Select(ev => ev.ToResponse()).ToList();
        }

    }
}