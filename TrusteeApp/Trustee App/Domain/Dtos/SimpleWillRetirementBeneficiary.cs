using System;
using System.ComponentModel.DataAnnotations;

namespace TrusteeApp.Domain.Dtos
{
    public class SimpleWillRetirementBeneficiary
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
        public string? BeneficiaryAddress { get; set; }

        [Required]
        public string? Relationship { get; set; }

        [Required]
        public string? BeneficiaryName { get; set; }

        [Required]
        [Display(Name = "Beneficiary Number")]
        public string? BeneficiaryAccountNumber { get; set; }

        [Required]
        [Display(Name = "Beneficiary Bank Name")]
        public string? BeneficiaryBankName { get; set; }

        [Required]
        [Display(Name = "Amount/Share to be given")]
        public string? AmountToBeGiven { get; set; }
    }
}
