using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConfidentVotesAPI.Models
{
    public class Election
    {
        public  Guid Id  {get; set;} = Guid.CreateVersion7();
        [Required]
        [MaxLength(300)]
        public  string  Name {get; set;} = string.Empty;
        [MaxLength(10000)]
        public  string?  Description {get; set;}
        // the id of the user who initiated the voteS
        [Required]
        public  Guid AppUserId {get; set;}
        public AppUser Initiator{ get; } = null!;
        [Required]
        public  DateTimeOffset ElectionStart {get; set;} = DateTimeOffset.UtcNow;
        [Required]
        public  DateTimeOffset ElectionEnd {get; set;} =  DateTimeOffset.UtcNow;
        [Required]
        //Collection navigation containing ElectionOptions and voters
        public ICollection<ElectionOption> ElectionOptions {get;} = new HashSet<ElectionOption>();
        public ICollection<ElectionVoter> Voters {get;} = new HashSet<ElectionVoter>();
        public ElectionResponseDto ToResponse()
        {
            return new ElectionResponseDto
            {
                Id = Id,
                ElectionEnd = ElectionEnd,
                ElectionStart = ElectionStart,
                Name = Name,
                Description = Description ?? string.Empty,
                AppUserId = AppUserId,
                ElectionOptions = ElectionOptions.Select(c => c.ToResponse()).ToHashSet(),
                Voters = Voters.Select( v => v.ToResponse()).ToHashSet()
            };
        }
        public ElectionRequestDto ToRequest()
        {
            return new ElectionRequestDto
            {
                ElectionEnd = ElectionEnd,
                ElectionStart = ElectionStart,
                Name = Name,
                Description = Description,
                AppUserId = AppUserId,
            };
        }
    }
    public class ElectionRequestDto
    {
        [Required]
        public  string  Name {get; set;} = string.Empty;
        public  string?  Description {get; set;}
        // the id of the user who initiated the voteS
        public  Guid? AppUserId {get; set;}
        public  DateTimeOffset ElectionStart {get; set;}
        public  DateTimeOffset ElectionEnd {get; set;}
        public Election ToElection()
        {
            return new Election
            {
                ElectionEnd = ElectionEnd,
                ElectionStart = ElectionStart,
                Name = Name,
                Description = Description,
            };
        }
    }
    
     public class ElectionResponseDto
    {
        public  Guid? Id  {get; set;}
        public  string  Name {get; set;} = string.Empty;
        public  string  Description {get; set;} = string.Empty;
        public Guid AppUserId { get; set; }
        // the id of the user who initiated the voteS
        public required DateTimeOffset ElectionStart {get; set;}
        public required DateTimeOffset ElectionEnd {get; set;}
        public ICollection<ElectionOptionResponseDto> ElectionOptions {get; set;} = new HashSet<ElectionOptionResponseDto>();
        public ICollection<ElectionVoterResponseDto> Voters {get; set;} = new HashSet<ElectionVoterResponseDto>();
        public Election ToElection()
        {
            return new Election
            {
                ElectionEnd = ElectionEnd,
                ElectionStart = ElectionStart,
                Name = Name,
                Description = Description,
                AppUserId = AppUserId
            };
        }
    }
}