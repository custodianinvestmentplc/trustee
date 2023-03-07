using System;
using System.ComponentModel.DataAnnotations;
using TrusteeApp.Domain.Dtos;

namespace TrusteeApp.ViewModels
{
    public class SimpleWillAccountVM
    {
        public TrusteePackage TrusteePackage { get; set; }

        public List<SimpleWillRetirementAccount> SimpleWillRetirementAccount { get; set; }

        public List<SimpleWillRetirementBeneficiary> SimpleWillRetirementBeneficiary { get; set; }
    }
}
