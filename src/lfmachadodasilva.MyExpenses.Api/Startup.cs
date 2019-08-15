using AutoMapper;
using lfmachadodasilva.MyExpenses.Api.Controllers;
using lfmachadodasilva.MyExpenses.Api.Models;
using lfmachadodasilva.MyExpenses.Api.Models.Config;
using lfmachadodasilva.MyExpenses.Api.Repositories;
using lfmachadodasilva.MyExpenses.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Swashbuckle.AspNetCore.Swagger;

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
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc(
                        "v1",
                        new Info
                        {
                            Title = "MyExpenses Core Api",
                            Description = "Project to with base/core stuff",
                            Version = "v1"
                        });
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
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Base API V1");
            });

            app.UseMvc();
        }
    }
}
