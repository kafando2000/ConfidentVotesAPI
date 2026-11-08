using System.ComponentModel.DataAnnotations;

namespace ConfidentVotesAPI.Models
{
    public class Choice
    {   
        public required int ChoiceID {get; set;}
        public required string  Title {get; set;}
        public required int ElectionID  {get; set;}
        public Election Election {get; set;} = null!;
        public List<Vote> Votes {get;} = new List<Vote>();
    }
}