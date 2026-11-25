using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.AspNetCore.Identity;

namespace ConfidentVotesAPI.Models
{
    public class AppUser : IdentityUser<Guid>
    {
      
        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        [MaxLength(2048)]
        public string? ProfilePictureUrl { get; set; }
        public bool IsActive { get; set; } = false;
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? LastLogIn { get; set; } 
        public ICollection<Election> InitiatedElections { get;} = new HashSet<Election>();
        public ICollection<ElectionVoter> ElectionsVoters { get;} = new HashSet<ElectionVoter>();
        public AppUser()
        {
          Id = Guid.CreateVersion7(); 
        }
    }
    public class AppUserResponseDto
    {  
        public Guid? Id{get; set;}
        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        [MaxLength(2048)]
        public string? ProfilePictureUrl { get; set; }
        public bool IsActive { get; set; } = false;
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? LastLogIn { get; set; } 
        public ICollection<ElectionResponseDto> InitiatedElections { get; set; } = new HashSet<ElectionResponseDto>();
        public ICollection<ElectionVoterResponseDto> ElectionsVoters { get; set;} = new HashSet<ElectionVoterResponseDto>();
    }
     public class RegisterRequest
    {   
        [MaxLength(100)]
        public   string FirstName {get; set;} = string.Empty;

        [MaxLength(100)]
        public   string LastName {get; set;} = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public  string Email {get; set;} = string.Empty;

        [MaxLength(100)]
        [Required]
        public  string UserName {get; set;} = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(
                @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
                ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character."
        )]
        [DataType(DataType.Password)]
        public   string Password {get; set;} = string.Empty;

        [Required(ErrorMessage = "Please confirm your password.")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public  string PasswordConfirm {get; set;} = string.Empty;
    }
     public class LoginRequest
    {   
        [Required]
        public required string Login {get; set;}
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        public   string Password {get; set;} = string.Empty;

    }
}