using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Grpc.Core;
using Jwt;
using Microsoft.IdentityModel.Tokens;

namespace GrpcServer.Services;

public class JwtService : Jwt.JwtService.JwtServiceBase
{
    public override Task<Response> GetJwt(Request request, ServerCallContext context)
    {
        var jwt = GenerateJwtTokenAsync(request.Username);
        return Task.FromResult(new Response { Token = jwt });
    }
    
    public string? GenerateJwtTokenAsync(string username)
    {
        var now = DateTime.UtcNow;
        var jwt = new JwtSecurityToken(
            issuer: "issuer",
            audience: "audience",
            notBefore: now,
            claims: GetIdentity(username).Claims,
            expires: now + TimeSpan.FromMinutes(5),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey("4A485D40-2D3C-42D7-BFFA-13BADE5763C8-4A485D40-2D3C-42D7-BFFA-13BADE5763C8"u8.ToArray()),
                SecurityAlgorithms.HmacSha256));
    
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
    
    private static ClaimsIdentity GetIdentity(string username)
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, username),
        };
        var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        return claimsIdentity;
    }
}