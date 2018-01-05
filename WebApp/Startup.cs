using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApp.Middlewares;
using System.IO;
using Microsoft.EntityFrameworkCore;
using WebApp.Entities;
using WebApp.Repositories;
using WebApp.Dtos;
using WebApp.Services;
using Microsoft.AspNetCore.Mvc.Formatters;
using NLog.Web;
using NLog.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApp
{


    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            env.ConfigureNLog("nlog.config");

            Configuration = builder.Build();           
        } 
        //  This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();


            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            services.AddDbContext<MyDBContext>(options =>
                            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ISeedDataService, SeedDataService>();

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new Info { Title = "Customer Info WebApi", Version = "v1" });
            });

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("resourcesAdmin", policyAdmin =>
            //    {
            //        policyAdmin.RequireClaim("role", "resources.admin");
            //    });
            //    options.AddPolicy("resourcesUser", policyUser =>
            //    {
            //        policyUser.RequireClaim("role", "resources.user");
            //    });
            //});

            //services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            //.AddIdentityServerAuthentication(options =>
            //{
            //    // base-address of your identityserver
            //    options.Authority = "http://localhost:57229";

            //    // name of the API resource
            //    options.ApiName = "resourcesScope";
            //    options.RequireHttpsMetadata = false;
            //});

            services.AddMvc(options =>
            {
                options.ReturnHttpNotAcceptable = true;
                //options.InputFormatters.Add(new XmlSerializerInputFormatter());
                //options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            });

            services.AddApiVersioning(config =>
            {
                config.ReportApiVersions = true;
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.AddSeedData();
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug(LogLevel.Error);
            loggerFactory.AddNLog();
            app.AddNLogWeb();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "text/plain";
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (errorFeature != null)
                    {
                        var logger = loggerFactory.CreateLogger("Global exception logger");
                        logger.LogError(500, errorFeature.Error, errorFeature.Error.Message);
                    }

                    await context.Response.WriteAsync("There was an error");
                });
            });

            //app.UseAuthentication();

            if (env.IsEnvironment("MyEnvironment"))
            {
                app.UseCustomeMiddleware();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            AutoMapper.Mapper.Initialize(mapper =>
            {
                mapper.CreateMap<Customer, CustomerDto>().ReverseMap();
                mapper.CreateMap<Customer, CustomerCreateDto>().ReverseMap();
                mapper.CreateMap<Customer, CustomerUpdateDto>().ReverseMap();
            });   

            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer info web api");
            });

            app.UseMvc();
        }
    }
}
