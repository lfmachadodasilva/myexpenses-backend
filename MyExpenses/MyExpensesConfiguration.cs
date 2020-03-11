using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            service.AddAutoMapper(typeof(MyExpensesProfile));

            var useInMemoryDatabase = Environment.GetEnvironmentVariable("MEMORY_DATABASE");
            if (string.IsNullOrEmpty(useInMemoryDatabase))
            {
                useInMemoryDatabase = configuration.GetSection("WebSettings").GetSection("UseInMemoryDatabase").Value;
            }

            if (useInMemoryDatabase != null && useInMemoryDatabase == false.ToString())
            {
                var migrationAssembly = configuration.GetSection("MigrationAssembly").Value;
                var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = configuration.GetConnectionString("DefaultConnection");
                }

                service
                    .AddDbContext<MyExpensesContext>(options =>
                        options.UseNpgsql(connectionString,
                            x => x.MigrationsAssembly(migrationAssembly)));
            }
            else
            {
                service
                    .AddDbContext<MyExpensesContext>(options =>
                        options.UseInMemoryDatabase("myexpenses"));
            }

            return service;
        }

        public static IApplicationBuilder DatabaseMigrate(this IApplicationBuilder app, IConfiguration configuration)
        {
            var useInMemoryDatabase = Environment.GetEnvironmentVariable("MEMORY_DATABASE");
            if (string.IsNullOrEmpty(useInMemoryDatabase))
            {
                useInMemoryDatabase = configuration.GetSection("WebSettings").GetSection("UseInMemoryDatabase").Value;
            }

            if (useInMemoryDatabase != null && useInMemoryDatabase == false.ToString())
            {
                using (var serviceScope = app.ApplicationServices
                    .GetRequiredService<IServiceScopeFactory>()
                    .CreateScope())
                {
                    using (var context = serviceScope.ServiceProvider.GetService<MyExpensesContext>())
                    {
                        context.Database.Migrate();
                    }
                }
            }

            return app;
        }
    }
}
