using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Kubrik.Models.Identity;

using Microsoft.IdentityModel.Tokens;

namespace Kubrik.Api.Services;

public sealed class JwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private readonly JwtSecurityTokenHandler _handler = new JwtSecurityTokenHandler();
    
    public string GenerateToken(User user)
    {
        Claim[] claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Picture, user.Picture)
        ];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            null,
            DateTime.Now.AddMinutes(15),
            credentials
        );

        return _handler.WriteToken(token);
    }
}