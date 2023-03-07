using System;
using System.Collections.Generic;
using TrusteeApp.Domain.Dtos;
using TrusteeApp.Models;

namespace TrusteeApp.ViewModels
{
    public class IndexViewModel
    {
        public string ErrorTitle { get; set; } = null;
        public string ErrorDescription { get; set; } = null;
        public string ExceptionType { get; set; } = null;
        public string UserName { get; set; } = null;
        public string Module { get; set; } = null;
    }
}

