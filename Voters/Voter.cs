using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace ConfidentVotesAPI.Models
{
    public class Voter
    {   
        public  Guid Id{get; set;} = Guid.CreateVersion7();
        [Required]
        [Range(0.5, double.MaxValue)]
        public double Weight{get; set;} = 1.0;
        [DeniedValues("00000000-0000-0000-0000-000000000000", ErrorMessage = "VoterId cannot be empty.")]
        public  Guid ElectionId  {get; set;} = Guid.Empty;
        [DeniedValues("00000000-0000-0000-0000-000000000000", ErrorMessage = "VoterId cannot be empty.")]
        public  Guid AppUserId  {get; set;} = Guid.Empty;
        public Election Election {get; set;} = null!;
        public AppUser AppUser {get; set;} = null!;
        public Vote? Vote {get;}
        public VoterResponseDto ToResponse()
        {
            return new VoterResponseDto
            {
                Id = Id,
                Weight = Weight,
                ElectionId = ElectionId,
                AppUserId = AppUserId

            };
        }
         public VoterRequestDto ToRequest()
        {
            return new VoterRequestDto
            {
                Weight = Weight,
                ElectionId = ElectionId,
                AppUserId = AppUserId

            };
        }
    }
    public class VoterRequestDto
    {   
        [Required]
        [Range(0.5, double.MaxValue)]
        public double Weight{get; set;} = 1.0;
        [DeniedValues("00000000-0000-0000-0000-000000000000", ErrorMessage = "VoterId cannot be empty.")]
        public  Guid ElectionId  {get; set;} = Guid.Empty;
        [DeniedValues("00000000-0000-0000-0000-000000000000", ErrorMessage = "VoterId cannot be empty.")]
        public  Guid AppUserId  {get; set;} = Guid.Empty;
         public Voter ToVoter()
        {
            return new Voter
            {
                Weight = Weight,
                ElectionId = ElectionId,
                AppUserId = AppUserId

            };
        }
    }
     public class VoterResponseDto
    {   
        public  Guid Id{get; set;} = Guid.Empty;
        public double Weight{get; set;} = 1.0;
        public  Guid ElectionId  {get; set;} = Guid.Empty;
        public  Guid AppUserId  {get; set;} = Guid.Empty;
        public Voter ToVoter()
        {
            return new Voter
            {
                Id = Id,
                Weight = Weight,
                ElectionId = ElectionId,
                AppUserId = AppUserId

            };
        }
    }
}