using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace ConfidentVotesAPI.Models
{
    public class Option
    {   
        public Guid Id {get; set;} = Guid.CreateVersion7();
        [Required]
        [MaxLength(200)]
        public string  Title {get; set;} = string.Empty;
        [Required]
        public Guid ElectionId  {get; set;}
        public Election Election {get; set;} = null!;
        public ICollection<Vote> Votes {get;} = new HashSet<Vote>();
        public OptionResponseDto ToResponse()
        {
            return new OptionResponseDto
            {
                Id = Id,
                Title =Title,
                ElectionId = ElectionId,
                Votes = Votes.Select(vote => vote.ToResponse()).ToHashSet()
            };
        }
    }
    public class OptionRequestDto
    {   
        [Required]
        public  string  Title {get; set;} = string.Empty;
        [Required]
        public  Guid ElectionId  {get; set;} = Guid.Empty;
        public Option ToOption()
        {
            return new Option
            {
                Title = Title,
                ElectionId = ElectionId 
            };
        }
    }
    public class OptionResponseDto
    {   
        public Guid Id {get; set;}        
        public  Guid ElectionId  {get; set;}
        public required string  Title {get; set;}
        public ICollection<VoteResponseDto> Votes {get; set;} = new HashSet<VoteResponseDto>();
    }
}