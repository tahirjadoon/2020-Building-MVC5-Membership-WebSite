using System.ComponentModel.DataAnnotations;

namespace Web.Memberships.Models
{
    public class RegisterUserModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {1} characters long.", MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {1} characters long.", MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        [Compare("Password", ErrorMessage = "The password and confirm password do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public bool AcceptUserAgreement { get; set; }
    }
}