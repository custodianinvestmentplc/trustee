using System;
using System.ComponentModel.DataAnnotations;

namespace TrusteeApp.Domain.Dtos
{
    public class SimpleWillOthers
    {
        [Required]
        public string? PackageId { get; set; }

        //[Required]
        //public string? OwnerEmail { get; set; }

        //[Required]
        //public string? OwnerId { get; set; }

        //[Required]
        //public string? Product { get; set; }

        [Display(Name = "General Wishes")]
        [Required]
        public string? GeneralWishes { get; set; }
    }
}
