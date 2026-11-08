
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
        public Task<List<Election>> GetElections();
        public Task<Election> GetElectionById(int id);
        public  Task addElection(Election election);
        public  Task RemoveElection(int id);
        public  Task UpdateElection(int Id, Election election);
        
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
        public async Task<Election> GetElectionById(int id)
        {
            var election = await _DbContext.Elections.AsTracking()
                                             .FirstOrDefaultAsync<Election>(p => p.ElectionID == id);
            if( election == null)
            {
                throw new BadValueException($"{id}");
            }
            return election;
        }
        public async Task addElection(Election election)
        {   
            if(election == null)
            {
                throw new BadBodyException("Body can't be empty.");
            }
           _DbContext.Add(election);
           await _DbContext.SaveChangesAsync();
        }

        // error handling hear : specific error handling
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
        public async Task UpdateElection(int id, Election election)
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
        public async Task<List<Election>> GetElections()
        {
            return _DbContext.Elections.ToList();
        }

    }
}