using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConfidentVotesAPI.Models
{
    public class ElectionVoter
    {   
        public required int ElectionVoterID{get; set;}
        public required int ElectionID  {get; set;}
        public required int PersonID  {get; set;}
        public Election Election {get; set;} = null!;
        public Person Person {get; set;} = null!;
        public Vote? Vote {get;}
    }
    public class ElectionVoterDto
    {   
        public required int ElectionVoterID{get; set;}
        public required int ElectionID  {get; set;}
        public required int PersonID  {get; set;}
    }
}