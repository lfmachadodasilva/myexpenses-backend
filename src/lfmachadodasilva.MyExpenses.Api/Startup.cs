﻿using AutoMapper;
using lfmachadodasilva.MyExpenses.Api.Controllers;
using lfmachadodasilva.MyExpenses.Api.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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

            services.TryAddSingleton(new MapperConfiguration(cfg => cfg.AddProfile<MyExpensesProfile>()).CreateMapper());
            services.TryAddSingleton<FakeDatabase, FakeDatabase>();

            services.TryAddTransient<ILabelRepository, LabelRepository>();
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
