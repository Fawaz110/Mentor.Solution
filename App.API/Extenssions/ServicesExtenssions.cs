using App.API.Helpers.MappingProfiles;
using Core.Entities;
using Core.Service.Contract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository;
using Service;
using System.Text;

namespace App.API.Extenssions
{
    public static class ServicesExtenssions
    {
        public static IServiceCollection ConfigureContexts(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddDbContext<MentorDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {

            }).AddEntityFrameworkStores<MentorDbContext>().AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"])),
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidateAudience = false,
                };
            });

            return services;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {

            #region ConfigureRepository

            #endregion

            #region ConfigureServices

            services.AddScoped(typeof(IAuthService), typeof(AuthService));

            #endregion

            #region MappingProfiles

             services.AddAutoMapper(typeof(MentorMappingProfile));

            #endregion

            return services;
        }

        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mentor System", Version = "v1" });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Description = "Jwt Bearer Token: bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "bearer"
                    }
                };

                c.AddSecurityDefinition("bearer", securityScheme);

                var securityRequirements = new OpenApiSecurityRequirement
                {
                    { securityScheme, new[] {"bearer"} }
                };

                c.AddSecurityRequirement(securityRequirements);
            });

            return services;
        }
    }
}
