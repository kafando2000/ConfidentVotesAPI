using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ConfidentVotesAPI.Models
{
    public class Person
    {   
        public required int PersonID {get; set;}
        public  required string FirstName {get; set;}
        public  required string LastName {get; set;}
        [EmailAddress]
        public required string Email {get; set;}
        public required string UserName {get; set;}
        public  readonly DateTime SignupDate = DateTime.Now;
        public List<Election> AllowedElections { get;} = new List<Election>();
        public List<Election> InitiatedElections { get;} = new List<Election>();
        public List<Vote> Votes { get;} = new List<Vote>();
    }
}