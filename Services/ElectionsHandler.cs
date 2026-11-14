
using System.Text.Json;
using ConfidentVotesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using ConfidentVotesAPI.CustomExceptionHandling;
using Serilog;
using Microsoft.EntityFrameworkCore;
using ConfidentVotesAPI;

namespace ConfidentVotesAPI.Services
{
    public interface IElectionsHandler
    {   
        public Task<List<ElectionResponseDto>> GetElections();
        public Task<ElectionResponseDto> GetElectionById(int id);
        public  Task addElection(ElectionRequestDto election);
        public  Task RemoveElection(int id);
        public  Task UpdateElection(int Id, ElectionRequestDto election);
        public Task<List<ChoiceResponseDto>> GetElectionChoices(int electionId);
        
    }
    public class ElectionsHandler : IElectionsHandler
    {   
        public ElectionsHandler(ConfidentVotesDbContext dbContext)
        {
            _ServiceId = new Random().Next(201, 2000);
            _DbContext = dbContext;
        }
        private readonly int _ServiceId;
        private ConfidentVotesDbContext _DbContext;
        // Elections list
        //private List<Election> Elections = new List<Election>();
        public async Task<ElectionResponseDto> GetElectionById(int id)
        {
            var election = await _DbContext.Elections.Include(e => e.Choices)
                                             .ThenInclude(c => c.Votes)
                                             .FirstOrDefaultAsync(p => p.ElectionID == id);
            if( election == null)
            {
                throw new BadValueException($"{id}");
            }
          
            return  election.ToResponse();
        }
        public async Task addElection(ElectionRequestDto election)
        {   
            if(election == null)
            {
                throw new BadBodyException("Body can't be empty.");
            }
            var dbelection = election.ToElection();
           _DbContext.Add(dbelection);
           await _DbContext.SaveChangesAsync();
        }

        public async Task RemoveElection(int id)
        {  
            var election = await _DbContext.FindAsync<Election>(id);
            if (election == null)
            {
               throw new BadPathException($"{id}");
            }
            _DbContext.Remove(election);
            await _DbContext.SaveChangesAsync();
    
        }
        public async Task UpdateElection(int id, ElectionRequestDto election)
        {
            var trackedElection = await _DbContext.FindAsync<Election>(id);
            if (trackedElection == null)
            {
               throw new BadPathException($"{id}");
            }
            trackedElection.ElectionEnd = election.ElectionEnd;
            trackedElection.ElectionStart = election.ElectionStart;
            trackedElection.Name = election.Name;
            trackedElection.Description = election.Description;
            await _DbContext.SaveChangesAsync();
        }
        public async Task<List<ElectionResponseDto>> GetElections()
        {
            return await _DbContext.Elections.Select(election => election.ToResponse()).ToListAsync<ElectionResponseDto>();
                
        }
        public async Task<List<ChoiceResponseDto>> GetElectionChoices(int electionId)
        {
            var choices = await _DbContext.Choices.Include(c => c.Votes)
                .Where(choice => choice.ElectionID == electionId)
                .Select(choice => choice.ToResponse())
                .ToListAsync();

            if (choices is null)
            {
                throw new BadPathException($"{electionId}");
            }

            return choices;
        }

    }
}