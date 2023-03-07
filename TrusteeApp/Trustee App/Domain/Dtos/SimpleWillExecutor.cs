using System;
using System.ComponentModel.DataAnnotations;

namespace TrusteeApp.Domain.Dtos
{
    public class SimpleWillExecutor
    {
        [Required]
        public string? PackageId { get; set; }

        //[Required]
        //public string? OwnerEmail { get; set; }

        //[Required]
        //public string? OwnerId { get; set; }

        //[Required]
        //public string? Product { get; set; }

        [Display(Name = "Last Name of Executor")]
        [Required]
        public string? LastNameOfExecutor { get; set; }

        [Display(Name = "First Name of Executor")]
        [Required]
        public string? FirstNameOfExecutor { get; set; }
    }
}
