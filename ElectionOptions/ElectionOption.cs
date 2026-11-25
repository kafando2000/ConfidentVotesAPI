using System.ComponentModel.DataAnnotations;

namespace ConfidentVotesAPI.Models
{
    public class ElectionOption
    {   
        public Guid Id {get; set;} = Guid.CreateVersion7();
        [Required]
        [MaxLength(200)]
        public string  Title {get; set;} = string.Empty;
        public Guid ElectionID  {get; set;}
        public Election Election {get; set;} = null!;
        public ICollection<Vote> Votes {get;} = new HashSet<Vote>();
        public ElectionOptionResponseDto ToResponse()
        {
            return new ElectionOptionResponseDto
            {
                Id = Id,
                Title =Title,
                ElectionID = ElectionID,
                Votes = Votes.Select(vote => vote.ToResponse()).ToHashSet()
            };
        }
        public ElectionOptionRequestDto ToRequest()
        {
            return new ElectionOptionRequestDto
            {
                Title =Title,
                ElectionID = ElectionID
            };
        }
    }
    public class ElectionOptionRequestDto
    {   
        public required string  Title {get; set;}
        public required Guid ElectionID  {get; set;}
        public ElectionOption ToElectionOption()
        {
            return new ElectionOption
            {
                Title =Title,
                ElectionID = ElectionID
            };
        }
    }
    public class ElectionOptionResponseDto
    {   
        public Guid Id {get; set;}        
        public  Guid ElectionID  {get; set;}
        public required string  Title {get; set;}
        public ICollection<VoteResponseDto> Votes {get; set;} = new HashSet<VoteResponseDto>();
        public ElectionOption ToElectionOption()
        {
            return new ElectionOption
            {
                Id = Id,
                Title =Title,
                ElectionID = ElectionID
            };
        }
    }
}