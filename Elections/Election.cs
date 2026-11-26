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
        //Collection navigation containing Options and voters
        public ICollection<Option> Options {get;} = new HashSet<Option>();
        public ICollection<Voter> Voters {get;} = new HashSet<Voter>();
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
                Options = Options.Select(c => c.ToResponse()).ToHashSet(),
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
        [Required]
        public  Guid AppUserId {get; set;} = Guid.Empty;
        [Required]
        public  DateTimeOffset ElectionStart {get; set;}
        [Required]
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
        public ICollection<OptionResponseDto> Options {get; set;} = new HashSet<OptionResponseDto>();
        public ICollection<VoterResponseDto> Voters {get; set;} = new HashSet<VoterResponseDto>();
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