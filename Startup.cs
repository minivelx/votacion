using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApiVotacion.Entidades;

namespace WebApiVotacion
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
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowAnyOrigin()
                      .AllowCredentials()
                .Build()
                );
            });

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("serverConnection"))
            .EnableSensitiveDataLogging());

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = false;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYYZ0123456789-._@ +";
            }).AddEntityFrameworkStores<ApplicationDbContext>()
                 .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration.GetConnectionString("serverDomain"),
                    ValidAudience = Configuration.GetConnectionString("serverDomain"),
                    IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Configuration.GetConnectionString("Key").ToString())),
                    ClockSkew = TimeSpan.Zero
                });

            services.AddAuthorization(options =>
            {
            });

            services.AddMvc().AddJsonOptions(ConfigureJson);
            services.AddCors();
        }

        private void ConfigureJson(MvcJsonOptions obj)
        {
            obj.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseDefaultFiles();
            app.UseMvc();
        }
    }
}
