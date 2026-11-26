using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConfidentVotesAPI.Models
{
    public class Voter
    {   
        public  Guid Id{get; set;} = Guid.CreateVersion7();

        public double Weight{get; set;} = 1.0;
        public  Guid ElectionId  {get; set;}
        public  Guid AppUserId  {get; set;}
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
    }
    public class VoterRequestDto
    {   
        public double Weight{get; set;} = 1.0;
        public  Guid ElectionId  {get; set;}
        public  Guid AppUserId  {get; set;}
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
        public required Guid Id{get; set;}
        public double Weight{get; set;}
        public required Guid ElectionId  {get; set;}
        public required Guid AppUserId  {get; set;}
        public Voter ToVoter()
        {
            return new Voter
            {
                Id = Id,
                ElectionId = ElectionId,
                AppUserId = AppUserId

            };
        }
    }
}