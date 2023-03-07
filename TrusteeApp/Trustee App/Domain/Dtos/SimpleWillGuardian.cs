using System;
using System.ComponentModel.DataAnnotations;

namespace TrusteeApp.Domain.Dtos
{
    public class SimpleWillGuardian
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
        [Display(Name = "Last Name of Guardian")]
        public string? LastNameOfGuardian { get; set; }

        [Display(Name = "First Name of Guardian")]
        [Required]
        public string? FirstNameOfGuardian { get; set; }
    }
}
