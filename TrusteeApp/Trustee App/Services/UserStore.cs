using System;
using System.Data.SqlClient;
using Dapper;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity;
using TrusteeApp;
using TrusteeApp.Domain.Dtos;
using TrusteeApp.Domain.Options;
using TrusteeApp.Repo;

namespace Trustee_App.Services
{
    public class UserStore : IUserStore<ApplicationUser>, IUserEmailStore<ApplicationUser>, IUserPhoneNumberStore<ApplicationUser>, IUserTwoFactorStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>
    {
        private readonly string _connectionString;

        public UserStore(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user.LockoutEnd = user.LockoutEnd ?? DateTimeOffset.Now;

            //var isSuccessful = false;

            var userDto = new ApplicationUserDto
            {
                AccessFailedCount = user.AccessFailedCount,
                ConcurrencyStamp = user.ConcurrencyStamp,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                //FullName = user.FullName,
                Id = user.Id.ToString(),
                LockoutEnabled = user.LockoutEnabled,
                LockoutEnd = user.LockoutEnd.ToString(),
                NormalizedEmail = user.NormalizedEmail,
                NormalizedUserName = user.NormalizedUserName,
                PasswordHash = user.PasswordHash,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                SecurityStamp = user.SecurityStamp,
                TwoFactorEnabled = user.TwoFactorEnabled,
                UserName = user.UserName,
            };

            var isSuccessful = RoutesController<ApplicationUserDto>.PostDbSet(userDto, WebConstants.ApplicationUser);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var isSuccessful = RoutesController<ApplicationUserDto>.DeleteDbSet("Id", user.Id, WebConstants.ApplicationUser);

            return IdentityResult.Success;
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var applicationUserDto = Repository<ApplicationUserDto>.Find(u => u.Id == userId, WebConstants.ApplicationUser);

            if (applicationUserDto != null)
            {
                applicationUserDto.LockoutEnd = applicationUserDto.LockoutEnd == "" ? DateTimeOffset.Now.ToString() : applicationUserDto.LockoutEnd;

                var applicationUser = new ApplicationUser
                {
                    AccessFailedCount = applicationUserDto.AccessFailedCount,
                    ConcurrencyStamp = applicationUserDto.ConcurrencyStamp,
                    Email = applicationUserDto.Email,
                    EmailConfirmed = applicationUserDto.EmailConfirmed,
                    //FullName = applicationUserDto.FullName,
                    Id = applicationUserDto.Id.ToString(),
                    LockoutEnabled = applicationUserDto.LockoutEnabled,
                    LockoutEnd = DateTimeOffset.Parse(applicationUserDto.LockoutEnd),
                    NormalizedEmail = applicationUserDto.NormalizedEmail,
                    NormalizedUserName = applicationUserDto.NormalizedUserName,
                    PasswordHash = applicationUserDto.PasswordHash,
                    PhoneNumber = applicationUserDto.PhoneNumber,
                    PhoneNumberConfirmed = applicationUserDto.PhoneNumberConfirmed,
                    SecurityStamp = applicationUserDto.SecurityStamp,
                    TwoFactorEnabled = applicationUserDto.TwoFactorEnabled,
                    UserName = applicationUserDto.UserName,
                };

                return applicationUser;
            }
            else return null;
        }

        public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var applicationUserDto = Repository<ApplicationUserDto>.Find(u => u.NormalizedUserName == normalizedUserName, WebConstants.ApplicationUser);

            if (applicationUserDto != null)
            {
                applicationUserDto.LockoutEnd = applicationUserDto.LockoutEnd == "" ? DateTimeOffset.Now.ToString() : applicationUserDto.LockoutEnd;

                var applicationUser = new ApplicationUser
                {
                    AccessFailedCount = applicationUserDto.AccessFailedCount,
                    ConcurrencyStamp = applicationUserDto.ConcurrencyStamp,
                    Email = applicationUserDto.Email,
                    EmailConfirmed = applicationUserDto.EmailConfirmed,
                    //FullName = applicationUserDto.FullName,
                    Id = applicationUserDto.Id.ToString(),
                    LockoutEnabled = applicationUserDto.LockoutEnabled,
                    LockoutEnd = DateTimeOffset.Parse(applicationUserDto.LockoutEnd),
                    NormalizedEmail = applicationUserDto.NormalizedEmail,
                    NormalizedUserName = applicationUserDto.NormalizedUserName,
                    PasswordHash = applicationUserDto.PasswordHash,
                    PhoneNumber = applicationUserDto.PhoneNumber,
                    PhoneNumberConfirmed = applicationUserDto.PhoneNumberConfirmed,
                    SecurityStamp = applicationUserDto.SecurityStamp,
                    TwoFactorEnabled = applicationUserDto.TwoFactorEnabled,
                    UserName = applicationUserDto.UserName,
                };

                return applicationUser;
            }
            else return null;


        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user != null)
            {
                var userDto = new ApplicationUserDto
                {
                    AccessFailedCount = user.AccessFailedCount,
                    ConcurrencyStamp = user.ConcurrencyStamp,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    //FullName = user.FullName,
                    Id = user.Id.ToString(),
                    LockoutEnabled = user.LockoutEnabled,
                    LockoutEnd = "",
                    NormalizedEmail = user.NormalizedEmail,
                    NormalizedUserName = user.NormalizedUserName,
                    PasswordHash = user.PasswordHash,
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    SecurityStamp = user.SecurityStamp,
                    TwoFactorEnabled = user.TwoFactorEnabled,
                    UserName = user.UserName,
                };

                var isSuccessful = RoutesController<ApplicationUserDto>.UpdateDbSet(userDto, WebConstants.ApplicationUser, "ReferenceNbr", user.Id);
            }

            return IdentityResult.Success;
        }

        public Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var applicationUserDto = Repository<ApplicationUserDto>.Find(u => u.NormalizedEmail == normalizedEmail, WebConstants.ApplicationUser);

            if (applicationUserDto != null)
            {
                applicationUserDto.LockoutEnd = applicationUserDto.LockoutEnd == "" ? DateTimeOffset.Now.ToString() : applicationUserDto.LockoutEnd;

                var applicationUser = new ApplicationUser
                {
                    AccessFailedCount = applicationUserDto.AccessFailedCount,
                    ConcurrencyStamp = applicationUserDto.ConcurrencyStamp,
                    Email = applicationUserDto.Email,
                    EmailConfirmed = applicationUserDto.EmailConfirmed,
                    //FullName = applicationUserDto.FullName,
                    Id = applicationUserDto.Id.ToString(),
                    LockoutEnabled = applicationUserDto.LockoutEnabled,
                    LockoutEnd = DateTimeOffset.Parse(applicationUserDto.LockoutEnd),
                    NormalizedEmail = applicationUserDto.NormalizedEmail,
                    NormalizedUserName = applicationUserDto.NormalizedUserName,
                    PasswordHash = applicationUserDto.PasswordHash,
                    PhoneNumber = applicationUserDto.PhoneNumber,
                    PhoneNumberConfirmed = applicationUserDto.PhoneNumberConfirmed,
                    SecurityStamp = applicationUserDto.SecurityStamp,
                    TwoFactorEnabled = applicationUserDto.TwoFactorEnabled,
                    UserName = applicationUserDto.UserName,
                };

                return applicationUser;
            }
            else return null;
        }

        public Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult(0);
        }

        public Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public void Dispose()
        {
        }
    }
}

