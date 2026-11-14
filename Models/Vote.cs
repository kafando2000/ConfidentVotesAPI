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
        public VoteResponseDto ToResponse()
        {
            return new VoteResponseDto
            {
                VoteID = VoteID,
                ElectionVoterID = ElectionVoterID,
                ChoiceID = ChoiceID,
                VoteDate = VoteDate
            };
        }
        public VoteRequestDto ToRequest()
        {
            return new VoteRequestDto
            {
                VoteID = VoteID,
                ElectionVoterID = ElectionVoterID,
                ChoiceID = ChoiceID,
                VoteDate = VoteDate
            };
        }
    }
     public class VoteRequestDto
    {
        // foreign keys of person and election
        // they will be unique combination
        public required int VoteID {get; set;}
        public required int ElectionVoterID {get; set;}
        public required int ChoiceID {get; set;}
        public  DateTime VoteDate{get; set;} = DateTime.Now;
        public Vote ToVote()
        {
            return new Vote
            {
                VoteID = VoteID,
                ElectionVoterID = ElectionVoterID,
                ChoiceID = ChoiceID,
                VoteDate = VoteDate
            };
        }
    }
     public class VoteResponseDto
    {
        // foreign keys of person and election
        // they will be unique combination
        public required int VoteID {get; set;}
        public required int ElectionVoterID {get; set;}
        public required int ChoiceID {get; set;}
        public  DateTime VoteDate{get; set;} = DateTime.Now;
        public Vote ToVote()
        {
            return new Vote
            {
                VoteID = VoteID,
                ElectionVoterID = ElectionVoterID,
                ChoiceID = ChoiceID,
                VoteDate = VoteDate
            };
        }
    }
}