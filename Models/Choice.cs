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
        public ChoiceResponseDto ToResponse()
        {
            return new ChoiceResponseDto
            {
                ChoiceID = ChoiceID,
                Title =Title,
                ElectionID = ElectionID,
                Votes = Votes.Select(vote => vote.ToResponse()).ToList()
            };
        }
        public ChoiceRequestDto ToRequest()
        {
            return new ChoiceRequestDto
            {
                ChoiceID = ChoiceID,
                Title =Title,
                ElectionID = ElectionID
            };
        }
    }
    public class ChoiceRequestDto
    {   
        public required int ChoiceID {get; set;}
        public required string  Title {get; set;}
        public required int ElectionID  {get; set;}
        public Choice ToChoice()
        {
            return new Choice
            {
                ChoiceID = ChoiceID,
                Title =Title,
                ElectionID = ElectionID
            };
        }
    }
    public class ChoiceResponseDto
    {   
        public required int ChoiceID {get; set;}
        public required int ElectionID  {get; set;}
        public required string  Title {get; set;}
        public List<VoteResponseDto> Votes {get; set;} = new List<VoteResponseDto>();
         public Choice ToChoice()
        {
            return new Choice
            {
                ChoiceID = ChoiceID,
                Title =Title,
                ElectionID = ElectionID
            };
        }
    }
}