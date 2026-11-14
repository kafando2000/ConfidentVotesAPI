using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ConfidentVotesAPI.Models
{
    public class Person
    {   
        public required int PersonID {get; set;}
        [Required]
        [StringLength(30,MinimumLength =1)]
        public  required string FirstName {get; set;}
         [Required]
        [StringLength(45,MinimumLength =1)]
        public  required string LastName {get; set;}
        
        [Required]
        [EmailAddress]
        public required string Email {get; set;}
        [Required]
        [StringLength(35,MinimumLength =1)]
        public required string UserName {get; set;}
        [PasswordPropertyText]
        public required string Password {get; set;}
        public  readonly DateTime SignupDate = DateTime.Now;
        public List<Election> InitiatedElections { get;} = new List<Election>();
        public List<ElectionVoter> ElectionsVoters { get;} = new List<ElectionVoter>();
        public PersonRequestDto ToRequest()
        {   
            return new PersonRequestDto
            {
                PersonID = PersonID,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                UserName = UserName,
                Password = Password
            };
        }
         public PersonResponseDto ToResponse()
        {   
            return new PersonResponseDto
            {
                PersonID = PersonID,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                UserName = UserName,
                InitiatedElections = InitiatedElections.Select(el => el.ToResponse()).ToList(),
                AllowedElections = ElectionsVoters.Select(elv => elv.Election.ToResponse()).ToList()
            };
        }
    }
     public class PersonRequestDto
    {   
        public required int PersonID {get; set;}
        public  required string FirstName {get; set;}
        public  required string LastName {get; set;}
        [EmailAddress]
        public required string Email {get; set;}
        public required string UserName {get; set;}
        public required string Password {get; set;}
         public Person ToPerson()
        {   
            return new Person
            {
                PersonID = PersonID,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                UserName = UserName,
                Password = Password
            };
        }
    }
     public class PersonResponseDto
    {   
        public required int PersonID {get; set;}
        public  required string FirstName {get; set;}
        public  required string LastName {get; set;}
        [EmailAddress]
        public required string Email {get; set;}
        public required string UserName {get; set;}
        public List<ElectionResponseDto> InitiatedElections { get; set;} = new List<ElectionResponseDto>();
        public List<ElectionResponseDto> AllowedElections { get; set;} = new List<ElectionResponseDto>();
    }
}