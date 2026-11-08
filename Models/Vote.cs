using System.ComponentModel.DataAnnotations;
namespace ConfidentVotesAPI.Models
{
    public class Vote
    {
        // foreign keys of person and election
        // they will be unique combination
        public required int VoteID {get; set;}
        public required int ElectionVoterID {get; set;}
        public required int ChoiceID {get; set;}
        public ElectionVoter ElectionVoter {get; set;} = null!;
        public Choice Choice {get; set;} = null!;
        public  DateTime VoteDate{get; set;} = DateTime.Now;
    }
     public class VoteDto
    {
        // foreign keys of person and election
        // they will be unique combination
        public required int VoteID {get; set;}
        public required int ElectionVoterID {get; set;}
        public required int ChoiceID {get; set;}
        public  DateTime VoteDate{get;} = DateTime.Now;
    }
}