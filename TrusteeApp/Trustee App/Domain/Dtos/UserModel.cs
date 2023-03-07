using System;
using System.ComponentModel.DataAnnotations;
using Trustee_App.Validation;

namespace TrusteeApp.Domain.Dtos
{
    public class UserModel
    {
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        [Display(Name = "I agree to the terms of service")]
        public bool TermsAgreed { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }


        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        [Display(Name = "User Name")]
        public string? UserName { get; set; }

        public string? Title { get; set; }

        public string? Otp { get; set; }

        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "State of Origin")]
        public string? StateOfOrigin { get; set; }

        [Display(Name = "Permanent Address")]
        public string? PermanentAddress { get; set; }

        [Display(Name = "Old Password")]
        [DataType(DataType.Password)]
        public string? OldPassword { get; set; }

        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Display(Name = "Phone No.")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }

        //public string? Code { get; set; }

        public string? StatusMessage { get; set; }

        [Display(Name = "Change Password")]
        [DataType(DataType.Password)]
        public string? ChangePassword { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DateOfBirth(-18)]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
