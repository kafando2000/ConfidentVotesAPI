using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConfidentVotesAPI.Models
{
    public class ElectionVoter
    {   
        public required int ElectionVoterID{get; set;}
        public required int ElectionID  {get; set;}
        public required int PersonID  {get; set;}
        public Election Election {get; set;} = null!;
        public Person Person {get; set;} = null!;
        public Vote? Vote {get;}
        public ElectionVoterResponseDto ToResponse()
        {
            return new ElectionVoterResponseDto
            {
                ElectionVoterID = ElectionVoterID,
                ElectionID = ElectionID,
                PersonID = PersonID

            };
        }
         public ElectionVoterRequestDto ToRequest()
        {
            return new ElectionVoterRequestDto
            {
                ElectionVoterID = ElectionVoterID,
                ElectionID = ElectionID,
                PersonID = PersonID

            };
        }
    }
    public class ElectionVoterRequestDto
    {   
        public required int ElectionVoterID{get; set;}
        public required int ElectionID  {get; set;}
        public required int PersonID  {get; set;}
         public ElectionVoter ToElectionVoter()
        {
            return new ElectionVoter
            {
                ElectionVoterID = ElectionVoterID,
                ElectionID = ElectionID,
                PersonID = PersonID

            };
        }
    }
     public class ElectionVoterResponseDto
    {   
        public required int ElectionVoterID{get; set;}
        public required int ElectionID  {get; set;}
        public required int PersonID  {get; set;}
        public ElectionVoter ToElectionVoter()
        {
            return new ElectionVoter
            {
                ElectionVoterID = ElectionVoterID,
                ElectionID = ElectionID,
                PersonID = PersonID

            };
        }
    }
}