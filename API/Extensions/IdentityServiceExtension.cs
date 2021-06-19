using System.Text;
using Core.Entities.Identity;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceExtension
    {
        
        public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration config)
        {
            var builder = services.AddIdentityCore<AppUser>();
            builder =  new IdentityBuilder (builder.UserType, builder.Services);
            builder.AddEntityFrameworkStores<AppIdentityContext>();
            builder.AddSignInManager< SignInManager<AppUser>>();
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(option=>
                    {
                        option.TokenValidationParameters= new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey=true,
                            IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
                            ValidIssuer=config["Token:Issuer"],
                            ValidateIssuer=true,
                            ValidateAudience=false
                        };
                    });
            
            return services;

        }
    }
}