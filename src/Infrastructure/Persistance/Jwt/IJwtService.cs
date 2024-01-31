using TesteBackend.Domain.Entities.Users;
using System.Threading.Tasks;

namespace TesteBackend.Persistence.Jwt
{
    public interface IJwtService
    {
        Task<AccessToken> GenerateAsync(User user);
        int? ValidateJwtAccessTokenAsync(string token);
    }
}
