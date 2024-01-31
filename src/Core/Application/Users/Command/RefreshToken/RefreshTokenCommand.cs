using MediatR;

namespace TesteBackend.Application.Users.Command.RefreshToken
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResponse>
    {
        public string RefreshToken { get; set; }

        public string AccessToken { get; set; }
    }
}
