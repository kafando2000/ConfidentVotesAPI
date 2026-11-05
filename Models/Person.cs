

using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ConfidentVotesAPI.Models
{
    public class Person
    {   
        [Required]
        public required int Id {get; set;}
        [Required]
        public  required string FullName {get; set;}
        [Required]
        [EmailAddress]
        public required string Email {get; set;}
        [Required]
        public required string UserName {get; set;}
        
    }
}