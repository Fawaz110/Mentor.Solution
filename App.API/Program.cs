
using App.API.Errors;
using App.API.Extenssions;
using App.API.Middlewares;
using App.API.Seeding;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace App.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            #region Extenssion Configurations
            builder.Services.AddSwaggerDocumentation();
            builder.Services.ConfigureContexts(builder);
            builder.Services.ConfigureServices();
            #endregion

            #region Configure Api Behavior
            builder.Services.Configure<ApiBehaviorOptions>(options =>
                {
                    options.InvalidModelStateResponseFactory = (actionContext) =>
                    {
                        var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                                                .SelectMany(p => p.Value.Errors)
                                                                .Select(p => p.ErrorMessage).ToList();

                        var validationErrorResponse = new ApiValidationErrorResponse
                        {
                            Errors = errors
                        };

                        return new BadRequestObjectResult(validationErrorResponse);
                    };
                });
            #endregion

            #region Cors Policy
            builder.Services.AddCors(options =>
                {
                    options.AddPolicy("MentorCorsPolicy", options =>
                    {
                        options
                            .WithOrigins(builder.Configuration.GetSection("AllowedHosts").Get<string[]>())
                            .AllowAnyMethod().AllowAnyHeader();
                    });
                }); 
            #endregion

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            var scope = app.Services.CreateScope();

            var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();

            var logger = loggerFactory.CreateLogger<Program>();
            
            var mentorDbContext = scope.ServiceProvider.GetService<MentorDbContext>();

            try
            {
                if (mentorDbContext != null)
                    await mentorDbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                logger.LogWarning("No Pending Migrations in MentorDbContext");
            }

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await ContextSeed.ApplyRolesSeeding(roleManager, logger);

            await ContextSeed.ApplySocialMediaSeeding(mentorDbContext, logger);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseCors("MentorCorsPolicy");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
