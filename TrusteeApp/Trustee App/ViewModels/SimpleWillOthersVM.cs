using System;
using System.ComponentModel.DataAnnotations;
using TrusteeApp.Domain.Dtos;

namespace TrusteeApp.ViewModels
{
    public class SimpleWillOthersVM
    {
        public TrusteePackage TrusteePackage { get; set; }

        public SimpleWillOthers SimpleWillOthers { get; set; }

        public List<SimpleWillGuardian> SimpleWillGuardians { get; set; }

        public List<SimpleWillExecutor> SimpleWillExecutors { get; set; }
    }
}
