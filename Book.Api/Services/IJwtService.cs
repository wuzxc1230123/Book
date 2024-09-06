using Book.Api.Dtos;
using Book.Api.Dtos.Auth;
using Book.Api.Dtos.Auth.Request;
using Book.Api.Dtos.Auth.Response;
using Microsoft.AspNetCore.Identity;

namespace Book.Api.Services;

public interface IJwtService
{
    Task<ResultDto<TokenResponseDto>> GenerateToken(IdentityUser user);
    Task<ResultDto<RefreshTokenResponseDto>> VerifyToken(TokenRequestDto tokenRequest);
}
