using System;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyExpenses.Helpers;
using MyExpenses.Models;
using MyExpenses.Repositories;
using MyExpenses.Services;

namespace MyExpenses
{
    public static class MyExpensesConfiguration
    {
        public static IServiceCollection AddMyExpenses(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddTransient<IUnitOfWork, MyExpensesUnitOfWork>();

            service.AddTransient<IUserService, UserService>();
            service.AddTransient<IGroupService, GroupService>();
            service.AddTransient<ILabelService, LabelService>();
            service.AddTransient<IExpenseService, ExpenseService>();

            service.AddTransient<IUserRepository, UserRepository>();
            service.AddTransient<IGroupRepository, GroupRepository>();
            service.AddTransient<ILabelRepository, LabelRepository>();
            service.AddTransient<IExpenseRepository, ExpenseRepository>();

            service.AddTransient<IValidateHelper, ValidateHelper>();

            service.AddAutoMapper(typeof(MyExpensesProfile));

            var appConfig = configuration.GetSection("AppConfig").Get<AppConfig>() ?? new AppConfig();
            service.Configure<AppConfig>(configuration.GetSection("AppConfig"));

            if (appConfig.UseInMemoryDatabase)
            {
                service
                    .AddDbContext<MyExpensesContext>(options =>
                        options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            }
            else
            {
                var migrationAssembly = configuration.GetSection("MigrationAssembly").Value;
                var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = configuration.GetConnectionString(appConfig.ConnectionString);
                }

                service
                    .AddDbContext<MyExpensesContext>(options =>
                        options.UseNpgsql(connectionString,
                            x => x.MigrationsAssembly(migrationAssembly)));
            }

            service
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var projectId = configuration.GetSection("Firebase:ProjectId").Value;
                    options.Authority = $"https://securetoken.google.com/{projectId}";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = $"https://securetoken.google.com/{projectId}",
                        ValidateAudience = true,
                        ValidAudience = projectId,
                        ValidateLifetime = true
                    };
                });

            return service;
        }

        public static IApplicationBuilder DatabaseMigrate(this IApplicationBuilder app, IConfiguration configuration)
        {
            var appConfig = configuration.GetSection("AppConfig").Get<AppConfig>() ?? new AppConfig();

            if (!appConfig.UseInMemoryDatabase)
            {
                using (var serviceScope = app.ApplicationServices
                    .GetRequiredService<IServiceScopeFactory>()
                    .CreateScope())
                {
                    using (var context = serviceScope.ServiceProvider.GetService<MyExpensesContext>())
                    {
                        // context.Database.EnsureDeleted();
                        context.Database.Migrate();
                    }
                }
            }

            if (appConfig.ClearDatabaseAndSeedData)
            {
                using (var serviceScope = app.ApplicationServices
                    .GetRequiredService<IServiceScopeFactory>()
                    .CreateScope())
                {
                    using (var context = serviceScope.ServiceProvider.GetService<MyExpensesContext>())
                    {
                        new MyExpensesSeed(context).Run();
                    }
                }
            }

            return app;
        }
    }
}
