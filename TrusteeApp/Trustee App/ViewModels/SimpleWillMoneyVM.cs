using System;
using System.ComponentModel.DataAnnotations;
using TrusteeApp.Domain.Dtos;

namespace TrusteeApp.ViewModels
{
    public class SimpleWillMoneyVM
    {
        public TrusteePackage TrusteePackage { get; set; }

        public List<SimpleWillMoneyAccountDetails> SimpleWillMoneyAccountDetails { get; set; }

        public List<SimpleWillMoneyBeneficiary> SimpleWillMoneyBeneficiary { get; set; }

    }
}
