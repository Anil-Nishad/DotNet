using Microsoft.AspNetCore.Identity;

namespace DotNet.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
