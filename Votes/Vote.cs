using System.ComponentModel.DataAnnotations;
namespace ConfidentVotesAPI.Models
{
    public class Vote
    {
        // foreign keys of person and election
        // they will be unique combination
        public  Guid Id {get; set;} = Guid.CreateVersion7();
        [Range(1.0, 10.0)]
        public int Confidence {get; set;} = 10;
        [Required]
        public  Guid? VoterID {get; set;}
        [Required]
        public  Guid? OptionID {get; set;}
        public Voter Voter {get;} = null!;
        public Option Option {get;} = null!;
        public  DateTimeOffset VotedOn{get; set;} = DateTimeOffset.UtcNow;
        public VoteResponseDto ToResponse()
        {
            return new VoteResponseDto
            {
                Id = Id,
                Confidence = Confidence,
                VoterID = VoterID,
                OptionID = OptionID,
                VotedOn = VotedOn
            };
        }
    }
     public class VoteRequestDto
    {
        // foreign keys of person and election
        // they will be unique combination
        [Range(1.0, 10.0)]
        public int Confidence {get; set;} = 10;
        [Required]
        public  Guid? VoterID {get; set;}
        [Required]
        public  Guid? OptionID {get; set;}
    }
     public class VoteResponseDto
    {
        // foreign keys of person and election
        // they will be unique combination
        public  Guid Id {get; set;}
        [Range(1.0, 10.0)]
        public int Confidence {get; set;} = 10;
        public  Guid? VoterID {get; set;}
        public  Guid? OptionID {get; set;}
        public  DateTimeOffset VotedOn{get; set;} = DateTimeOffset.UtcNow;
        public Vote ToVote()
        {
            return new Vote
            {
                Id = Id,
                Confidence = Confidence,
                VoterID = VoterID,
                OptionID = OptionID,
                VotedOn = VotedOn
            };
        }
    }
}