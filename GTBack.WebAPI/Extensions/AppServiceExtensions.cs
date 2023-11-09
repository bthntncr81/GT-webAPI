using FluentValidation;
using GTBack.Core.DTO;
using GTBack.Core.Services;
using GTBack.Core.UnitOfWorks;
using GTBack.Repository;
using GTBack.Repository.UnitOfWorks;
using GTBack.Service.Configurations;
using GTBack.Service.Services;
using GTBack.Service.Utilities.Jwt;
using GTBack.Service.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GTBack.WebAPI.Extensions
{
    public static class AppServicesExtensions
    {
    


        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var appConfig = configuration.Get<GoThereAppConfig>();

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appConfig.JwtConfiguration.AccessTokenSecret)),
                    ValidIssuer = appConfig.JwtConfiguration.Issuer,
                    ValidAudience = appConfig.JwtConfiguration.Audience,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
        public static void LoadValidators(this IServiceCollection services)
        {
            services.AddTransient<IValidatorFactory, ServiceProviderValidatorFactory>();
            services.AddTransient<IValidator<UserRegisterDTO>, CustomerDtoValidator>();
            services.AddTransient<IValidator<LoginDto>,LoginDtoValidator >();

        }

        public static void AddAppConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var appConfig = configuration.Get<GoThereAppConfig>();
            services.AddSingleton(appConfig);
        }

      

    }
}
