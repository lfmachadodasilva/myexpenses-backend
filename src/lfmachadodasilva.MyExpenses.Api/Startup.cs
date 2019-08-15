using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using lfmachadodasilva.MyExpenses.Api.Models;
using lfmachadodasilva.MyExpenses.Api.Models.Config;
using lfmachadodasilva.MyExpenses.Api.Repositories;
using lfmachadodasilva.MyExpenses.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace lfmachadodasilva.MyExpenses.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCors()
                .AddSwaggerGen(options =>
                {
                    options.SwaggerDoc(
                        "v1",
                        new Info
                        {
                            Title = "MyExpenses Core Api",
                            Description = "Project to with base/core stuff",
                            Version = "v1"
                        });
                    options.AddSecurityDefinition("oauth2",
                        new ApiKeyScheme
                        {
                            In = "header",
                            Description = "Please enter into field the word 'Bearer' following by space and JWT",
                            Name = "Authorization",
                            Type = "apiKey"
                        });
                    options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                        { "oauth2", Enumerable.Empty<string>() },
                    });

                    options.OperationFilter<AuthorizeCheckOperationFilter>(); // Required to use access token
                })
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.TryAddTransient<IExpenseRepository, ExpenseRepository>();
            services.TryAddTransient<ILabelRepository, LabelRepository>();
            services.TryAddTransient<IGroupRepository, GroupRepository>();
            services.TryAddTransient<IUserRepository, UserRepository>();

            services.TryAddTransient<IExpenseService, ExpenseService>();
            services.TryAddTransient<ILabelService, LabelService>();
            services.TryAddTransient<IGroupService, GroupService>();
            services.TryAddTransient<IUserService, UserService>();
            services.TryAddTransient<IReportService, ReportService>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.TryAddTransient<IMyExpensesSeed, MyExpensesSeed>();

            services.AddAutoMapper(typeof(MyExpensesProfile));

            services.Configure<WebSettingsConfig>(Configuration.GetSection("WebSettings"));
            services.TryAddTransient<IWebSettings, WebSettings>();

            var useInMemoryDatabase = Configuration.GetSection("WebSettings").GetSection("UseInMemoryDatabase").Value;
            var connection = Configuration.GetConnectionString("DefaultConnection");

            if (useInMemoryDatabase == true.ToString())
            {
                services
                    .AddDbContext<MyExpensesContext>(options =>
                        options.UseInMemoryDatabase(connection));
            }
            else
            {
                var migrationAssembly = Configuration.GetSection("MigrationAssembly").Value;
                services
                    .AddDbContext<MyExpensesContext>(options =>
                        options.UseNpgsql(connection,
                            x => x.MigrationsAssembly(migrationAssembly)));
            }

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://securetoken.google.com/myexpenses-a37a9";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "https://securetoken.google.com/myexpenses-a37a9",
                        ValidateAudience = true,
                        ValidAudience = "myexpenses-a37a9",
                        ValidateLifetime = true
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder
                //.WithOrigins("http://*.*.*.*:4200")
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Base API V1");

                //options.RoutePrefix = string.Empty;

                //options.OAuthClientId("myexpenses_api_swagger");
                //options.OAuthAppName("MyExpenses API - Swagger"); // presentation purposes only
            });

            app.UseAuthentication();

            app.UseMvc();
        }

        public class AuthorizeCheckOperationFilter : IOperationFilter
        {
            public void Apply(Operation operation, OperationFilterContext context)
            {
                // Check for authorize attribute
                if (context.ApiDescription.TryGetMethodInfo(out var methodInfo) &&
                    methodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any())
                {
                    operation.Responses.Add("401", new Response { Description = "Unauthorized" });
                    operation.Responses.Add("403", new Response { Description = "Forbidden" });

                    operation.Security = new List<IDictionary<string, IEnumerable<string>>>
                    {
                        new Dictionary<string, IEnumerable<string>> {{"oauth2", new[] {"myexpenses_api"}}}
                    };
                }
            }
        }
    }
}
