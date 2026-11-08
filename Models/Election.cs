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
        [ForeignKey("PersonID")]
        public Person Initiator{ get; set;} = null!;
        public required DateTime ElectionStart {get; set;}
        public required DateTime ElectionEnd {get; set;}
        //Collection navigation containing choices and voters
        public List<Choice> Choices {get;} = new List<Choice>();
        public List<Person> Voters {get;} = new List<Person>();
        public List<Vote> Votes {get;} = new List<Vote>();
    }
    public class ElectionDto
    {
        public required int ElectionID  {get; set;}
        public required string  Name {get; set;}
        public required string  Description {get; set;}
        // the id of the user who initiated the voteS
        public required int PersonID {get; set;}
        public required DateTime ElectionStart {get; set;}
        public required DateTime ElectionEnd {get; set;}
    }
}