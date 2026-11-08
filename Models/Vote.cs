using System.ComponentModel.DataAnnotations;
namespace ConfidentVotesAPI.Models
{
    public class Vote
    {
        // foreign keys of person and election
        // they will be unique combination
        public required int  VoteID{get; set;}
        public required int ElectionID {get; set;}
        public required int ChoiceID {get; set;}
        public required int PersonID {get; set;}
        public Person Voter {get; set;} = null!;
        public Choice Choice {get; set;} = null!;
        public Election Election {get; set;} = null!;
        public readonly DateTime VoteDate = DateTime.Now;
    }
}