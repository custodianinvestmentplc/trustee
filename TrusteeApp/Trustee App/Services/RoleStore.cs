using System;
using System.Data.SqlClient;
using Dapper;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity;
using TrusteeApp;
using TrusteeApp.Domain.Dtos;
using TrusteeApp.Repo;

namespace Trustee_App.Services
{
    public class RoleStore : IRoleStore<IdentityRole>
    {
        private readonly string _connectionString;

        public RoleStore(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            role.ConcurrencyStamp = role.ConcurrencyStamp ?? "";

            var isSuccessful = RoutesController<IdentityRole>.PostDbSet(role, WebConstants.IdentityRole);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var isSuccessful = RoutesController<IdentityRole>.UpdateDbSet(role, WebConstants.IdentityRole, "ReferenceNbr", role.Id);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var isSuccessful = RoutesController<IdentityRole>.DeleteDbSet("Id", role.Id, WebConstants.IdentityRole);

            return IdentityResult.Success;
        }

        public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.FromResult(0);
        }

        public Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.FromResult(0);
        }

        public async Task<IdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Repository<IdentityRole>.Find(u => u.Id == roleId, WebConstants.IdentityRole);
        }

        public async Task<IdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Repository<IdentityRole>.Find(u => u.NormalizedName == normalizedRoleName, WebConstants.IdentityRole);
        }

        public void Dispose()
        {
        }
    }
}

