using TrusteeApp.Models;

namespace TrusteeApp.Services
{
    public interface IAuthProvider
    {
        string AcquireAdToken(string authcode);
        AzureAdUserInfo GetLoggedInUser(string accessToken);
    }
}
