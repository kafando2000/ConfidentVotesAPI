using System.ComponentModel.DataAnnotations;
namespace ConfidentVotesAPI.Models
{
    public class Vote
    {
        // foreign keys of person and election
        // they will be unique combination
        public  Guid Id {get; set;} = Guid.CreateVersion7();
        [Range(0.01, 1.0)]
        [Required]
        public double Confidence {get; set;} = 1.0; // 100% confident

        [DeniedValues("00000000-0000-0000-0000-000000000000", ErrorMessage = "VoterId cannot be empty.")]
        public  Guid VoterId {get; set;} = Guid.Empty;

        [DeniedValues("00000000-0000-0000-0000-000000000000", ErrorMessage = "OptionId cannot be empty.")]
        public  Guid OptionId {get; set;} = Guid.Empty;
        public Voter Voter {get;} = null!;
        public Option Option {get;} = null!;
        [Required]
        public  DateTimeOffset VotedOn{get; set;} = DateTimeOffset.UtcNow;
        public VoteResponseDto ToResponse()
        {
            return new VoteResponseDto
            {
                Id = Id,
                Confidence = Confidence,
                VoterId = VoterId,
                OptionId = OptionId,
                VotedOn = VotedOn
            };
        }
    }
     public class VoteRequestDto
    {
        // foreign keys of person and election
        // they will be unique combination
        [Range(1.0, 10.0)]
        public double Confidence {get; set;} = 1.0; // 100% confident

        [DeniedValues("00000000-0000-0000-0000-000000000000", ErrorMessage = "VoterId cannot be empty.")]
        public  Guid VoterId {get; set;} = Guid.Empty;
        [DeniedValues("00000000-0000-0000-0000-000000000000", ErrorMessage = "OptionId cannot be empty.")]
        public  Guid OptionId {get; set;} = Guid.Empty;
        public Vote ToVote()
        {
            return new Vote
            {
                Confidence = Confidence,
                VoterId = VoterId,
                OptionId = OptionId
            };
        }
    }
     public class VoteResponseDto
    {
        // foreign keys of person and election
        // they will be unique combination
        public  Guid Id {get; set;}
        [Range(1.0, 10.0)]
        public double Confidence {get; set;} = 1.0; // 100% confident
        public  Guid VoterId {get; set;} = Guid.Empty;
        public  Guid OptionId {get; set;} = Guid.Empty;
        public  DateTimeOffset VotedOn{get; set;} = DateTimeOffset.UtcNow;
        public Vote ToVote()
        {
            return new Vote
            {
                Id = Id,
                Confidence = Confidence,
                VoterId = VoterId,
                OptionId = OptionId,
                VotedOn = VotedOn
            };
        }
    }
}