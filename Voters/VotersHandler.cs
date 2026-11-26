
using System.Text.Json;
using ConfidentVotesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using ConfidentVotesAPI.CustomExceptionHandling;
using Serilog;
using Microsoft.EntityFrameworkCore;
using ConfidentVotesAPI;

namespace ConfidentVotesAPI.Services
{
    public interface IVotersHandler
    {   
        public Task<List<VoterResponseDto>> GetVoters();
        public Task<VoterResponseDto> GetVoterById(int id);
        public  Task addVoter(VoterRequestDto Voter);
        public  Task RemoveVoter(int id);
        public  Task UpdateVoter(int Id, VoterRequestDto Voter);
        
    }
    public class VotersHandler : IVotersHandler
    {   
        public VotersHandler(ConfidentVotesDbContext dbContext)
        {
            _ServiceId = new Random().Next(5000, 6000);
            _DbContext = dbContext;
        }
        private readonly int _ServiceId;
        private ConfidentVotesDbContext _DbContext;
        // Voters list
        //private List<Voter> Voters = new List<Voter>();
        public async Task<VoterResponseDto> GetVoterById(int id)
        {
            var Voter = await _DbContext.Voters
                                                .FirstOrDefaultAsync(p => 
                                                    p.VoterID == id);

            if( Voter == null)
            {
                throw new BadValueException($"{id}");
            }
            return Voter.ToResponse();
        }
        public async Task addVoter(VoterRequestDto Voter)
        {   
            if(Voter == null)
            {
                throw new BadBodyException("Body can't be empty.");
            }
            var dbVoter = Voter.ToVoter();
           _DbContext.Add(dbVoter);
           await _DbContext.SaveChangesAsync();
        }

        // error handling hear : specific error handling
        public async Task RemoveVoter(int id)
        {  
            var Voter = await _DbContext.FindAsync<Voter>(id);
            if (Voter == null)
            {
               throw new BadPathException($"{id}");
            }
            _DbContext.Remove(Voter);
            await _DbContext.SaveChangesAsync();
    
        }
        public async Task UpdateVoter(int id, VoterRequestDto Voter)
        {
            var trackedVoter = await _DbContext.FindAsync<Voter>(id);
            if (trackedVoter == null)
            {
               throw new BadPathException($"{id}");
            }
            trackedVoter.ElectionID = Voter.ElectionID;
            trackedVoter.PersonID = Voter.PersonID;
            await _DbContext.SaveChangesAsync();
        }
        public async Task<List<VoterResponseDto>> GetVoters()
        {
            return _DbContext.Voters.Select(ev => ev.ToResponse()).ToList();
        }

    }
}