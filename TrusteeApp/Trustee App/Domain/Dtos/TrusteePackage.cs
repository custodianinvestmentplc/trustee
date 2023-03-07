using System;

namespace TrusteeApp.Domain.Dtos
{
    public class TrusteePackage
    {
		public string? Id { get; set; }
		public string? Template { get; set; }
		public string? Language { get; set; }
		public string? Product { get; set; }
		public string? Price { get; set; }
		public string? OwnerId { get; set; }
		public string? OwnerEmail { get; set; }
		public DateTime CreateDate { get; set; }
		public string? PackageStatus { get; set; }
    }
}
