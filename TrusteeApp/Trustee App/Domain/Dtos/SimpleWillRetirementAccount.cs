using System;
using System.ComponentModel.DataAnnotations;

namespace TrusteeApp.Domain.Dtos
{
    public class SimpleWillRetirementAccount
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
        [Display(Name = "RSA Number")]
        public string? RsaNumber { get; set; }

        [Required]
        [Display(Name = "PFA Name")]
        public string? PfaName { get; set; }

    }
}
