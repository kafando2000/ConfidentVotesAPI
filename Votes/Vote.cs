using System.ComponentModel.DataAnnotations;
namespace ConfidentVotesAPI.Models
{
    public class Vote
    {
        // foreign keys of person and election
        // they will be unique combination
        public  Guid Id {get; set;} = Guid.CreateVersion7();
        public  Guid? ElectionVoterID {get; set;}
        public  Guid? ElectionOptionID {get; set;}
        public ElectionVoter ElectionVoter {get;} = null!;
        public ElectionOption ElectionOption {get;} = null!;
        public  DateTimeOffset VotedOn{get; set;} = DateTimeOffset.UtcNow;
        public VoteResponseDto ToResponse()
        {
            return new VoteResponseDto
            {
                Id = Id,
                ElectionVoterID = ElectionVoterID,
                ElectionOptionID = ElectionOptionID,
                VotedOn = VotedOn
            };
        }
        public VoteRequestDto ToRequest()
        {
            return new VoteRequestDto
            {
                ElectionVoterID = ElectionVoterID,
                ElectionOptionID = ElectionOptionID,
            };
        }
    }
     public class VoteRequestDto
    {
        // foreign keys of person and election
        // they will be unique combination
        public  Guid? ElectionVoterID {get; set;}
        public  Guid? ElectionOptionID {get; set;}
        public Vote ToVote()
        {
            return new Vote
            {
                ElectionVoterID = ElectionVoterID,
                ElectionOptionID = ElectionOptionID,
            };
        }
    }
     public class VoteResponseDto
    {
        // foreign keys of person and election
        // they will be unique combination
        public  Guid Id {get; set;}
        public  Guid? ElectionVoterID {get; set;}
        public  Guid? ElectionOptionID {get; set;}
        public  DateTimeOffset VotedOn{get; set;} = DateTimeOffset.UtcNow;
        public Vote ToVote()
        {
            return new Vote
            {
                Id = Id,
                ElectionVoterID = ElectionVoterID,
                ElectionOptionID = ElectionOptionID,
                VotedOn = VotedOn
            };
        }
    }
}