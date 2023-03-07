using System;
using System.ComponentModel.DataAnnotations;

namespace TrusteeApp.Domain.Dtos
{
    public class SimpleWillMoneyAccountDetails
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
        [Display(Name = "Account Number")]
        public string? AccountNumber { get; set; }

        [Required]
        [Display(Name = "Bank Name")]
        public string? BankName { get; set; }
    }
}
