using System;
using System.ComponentModel.DataAnnotations;
using Trustee_App.Validation;

namespace TrusteeApp.Domain.Dtos
{
    public class UserContactDetails
    {
        [Required]
        public string? PackageId { get; set; }

        //[Required]
        //public string? OwnerEmail { get; set; }

        //[Required]
        //public string? OwnerId { get; set; }

        //[Required]
        //public string? Product { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required]
        [Display(Name = "State of Origin")]
        public string? StateOfOrigin { get; set; }

        [Required]
        [Display(Name = "Permanent Address")]
        public string? PermanentAddress { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DateOfBirth(-18)]
        [DataType(DataType.Date)]
        //[DataType(DataType.Date), DisplayFormat(DataFormatstring? = "{dd MMM yyyy}", ApplyFormatInEditMode = true)]
        //[DataType(DataType.Date)]
        //[CustomDateValidationAttribute(isYear: true, maxLimit: -18, minLimit: 0)]
        public DateTime DateOfBirth { get; set; }
    }
}
