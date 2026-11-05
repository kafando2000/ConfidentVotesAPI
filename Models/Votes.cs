using System.ComponentModel.DataAnnotations;

namespace ConfidentVotesAPI.Models
{
    public class Vote
    {   [Required]
        public required int Id  {get; set;}
        [Required]
        public required string  Name {get; set;}
        [Required]
        public required string  Description {get; set;}
        // the id of the user who initiated the vote
        [Required]
        public required int Initiator_id {get; set;}
        
    }
}