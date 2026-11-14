using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConfidentVotesAPI.Models
{
    public class Election
    {
        public required int ElectionID  {get; set;}
        public required string  Name {get; set;}
        public required string  Description {get; set;}
        // the id of the user who initiated the voteS
        public required int PersonID {get; set;}
        public Person Initiator{ get; set;} = null!;
        public required DateTime ElectionStart {get; set;}
        public required DateTime ElectionEnd {get; set;}
        //Collection navigation containing choices and voters
        public List<Choice> Choices {get;} = new List<Choice>();
        public List<ElectionVoter> Voters {get;} = new List<ElectionVoter>();
        public ElectionResponseDto ToResponse()
        {
            return new ElectionResponseDto
            {
                ElectionID = ElectionID,
                ElectionEnd = ElectionEnd,
                ElectionStart = ElectionStart,
                Name = Name,
                Description = Description,
                PersonID = PersonID,
                Choices = Choices.Select(c => c.ToResponse()).ToList(),
                Voters = Voters.Select( v => v.ToResponse()).ToList()
            };
        }
        public ElectionRequestDto ToRequest()
        {
            return new ElectionRequestDto
            {
                ElectionID = ElectionID,
                ElectionEnd = ElectionEnd,
                ElectionStart = ElectionStart,
                Name = Name,
                Description = Description,
                PersonID = PersonID,
            };
        }
    }
    public class ElectionRequestDto
    {
        public required int ElectionID  {get; set;}
        public required string  Name {get; set;}
        public required string  Description {get; set;}
        // the id of the user who initiated the voteS
        public required int PersonID {get; set;}
        public required DateTime ElectionStart {get; set;}
        public required DateTime ElectionEnd {get; set;}
        public Election ToElection()
        {
            return new Election
            {
                ElectionID = ElectionID,
                ElectionEnd = ElectionEnd,
                ElectionStart = ElectionStart,
                Name = Name,
                Description = Description,
                PersonID = PersonID
            };
        }
    }
    
     public class ElectionResponseDto
    {
        public required int ElectionID  {get; set;}
        public required string  Name {get; set;}
        public required string  Description {get; set;}
        // the id of the user who initiated the voteS
        public required int PersonID {get; set;}
        public required DateTime ElectionStart {get; set;}
        public required DateTime ElectionEnd {get; set;}
        public List<ChoiceResponseDto> Choices {get; set;} = new List<ChoiceResponseDto>();
        public List<ElectionVoterResponseDto> Voters {get; set;} = new List<ElectionVoterResponseDto>();
        public Election ToElection()
        {
            return new Election
            {
                ElectionID = ElectionID,
                ElectionEnd = ElectionEnd,
                ElectionStart = ElectionStart,
                Name = Name,
                Description = Description,
                PersonID = PersonID
            };
        }
    }
}