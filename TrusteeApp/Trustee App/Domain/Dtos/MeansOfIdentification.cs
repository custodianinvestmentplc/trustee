using System;
using System.ComponentModel.DataAnnotations;

namespace TrusteeApp.Domain.Dtos
{
    public class MeansOfIdentification
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
        [Display(Name = "Name of Employer")]
        public string? NameOfEmployer { get; set; }

        [Required]
        [Display(Name = "Means of ID")]
        public string? MeansOfId { get; set; }

        [Required]
        [Display(Name = "ID Number")]
        public string? IdNumber { get; set; }

        [Required]
        [Display(Name = "Tax Clearance ID No.")]
        public string? TaxClearanceIdNo { get; set; }
    }
}
