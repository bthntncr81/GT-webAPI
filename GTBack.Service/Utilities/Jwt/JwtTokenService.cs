using GTBack.Core.DTO;
using GTBack.Core.Services;
using GTBack.Service.Configurations;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.Entities;
using Newtonsoft.Json;

namespace GTBack.Service.Utilities.Jwt
{
    public class JwtTokenService<T>: IJwtTokenService<BaseRegisterDTO>
    {
        private readonly JwtConfiguration _configuration;

        public JwtTokenService(GoThereAppConfig configuration)
        {
            _configuration = configuration.JwtConfiguration;
        }

        public AccessTokenDto GenerateAccessToken(BaseRegisterDTO userDto)
        {
            var expirationTime = DateTime.UtcNow.AddHours(_configuration.AccessTokenExpirationMinutes);
         
            
            var claims = new List<Claim>
            {
                new("Id", userDto.Id.ToString()),
                new(ClaimTypes.Name, userDto.Name),
                new(ClaimTypes.Expiration, expirationTime.ToString()),
                new(ClaimTypes.Surname, userDto.Surname),
                new("UserType", userDto.UserTypeId.ToString()),
                new("CompanyId", userDto.CompanyId.ToString()),
                new("Roles", JsonConvert.SerializeObject(userDto.RoleList)),
            };
            claims.Add(new Claim("name", userDto.Name));
            claims.Add(new Claim("surname", userDto.Surname));
            claims.Add(new Claim("userType", userDto.UserTypeId.ToString()));
            claims.Add(new Claim("companyId", userDto.CompanyId.ToString()));
            claims.Add(new Claim("roles", JsonConvert.SerializeObject(userDto.RoleList)));


            claims.Add(new Claim("ExpTime", expirationTime.ToString()));
            var value = GenerateToken(
                _configuration.AccessTokenSecret,
                _configuration.Issuer,
                _configuration.Audience,
                expirationTime,
                claims);
            return new AccessTokenDto()
            {
                Value = value,
                ExpirationTime = expirationTime
            };
        }

        
     
        
        
    
        public string GenerateRefreshToken()
        {
            var expirationTime = DateTime.UtcNow.AddHours(_configuration.RefreshTokenExpirationMinutes);

            return GenerateToken(
                _configuration.RefreshTokenSecret,
                _configuration.Issuer,
                _configuration.Audience,
                expirationTime);
        }

        public bool Validate(string refreshToken)
        {
            var validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.RefreshTokenSecret)),
                ValidIssuer = _configuration.Issuer,
                ValidAudience = _configuration.Audience,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(refreshToken, validationParameters, out var validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string GenerateToken(string secretKey, string issuer, string audience,
            DateTime utcExpirationTime,
            IEnumerable<Claim> claims = null)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                DateTime.UtcNow,
                utcExpirationTime,
                credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}