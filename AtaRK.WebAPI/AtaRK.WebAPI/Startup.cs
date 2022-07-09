using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using React.AspNet;
using JavaScriptEngineSwitcher.V8;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AtaRK.WebAPI.Authentication;
using AtaRK.EF.DataContext;
using Microsoft.EntityFrameworkCore;
using AtaRK.BLL.Implementations;
using AtaRK.BLL.Interfaces;
using AtaRK.DAL.Interfaces;
using AtaRK.DAL.Implementations;
using FluentValidation.AspNetCore;
using AtaRK.WebAPI.Services;
using Microsoft.Net.Http.Headers;

namespace AtaRK.WebAPI
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
            services.AddDbContext<AtaRKDataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnection"));
            });

            services.AddScoped<DbContext, AtaRKDataContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };
                });

            services
                .AddControllersWithViews()
                .AddNewtonsoftJson()
                .AddFluentValidation(configuration =>
                {
                    configuration.RegisterValidatorsFromAssembly(this.GetType().Assembly);
                });

            services.AddHttpContextAccessor();

            services.AddTransient<IAuthorizationService, AuthorizationService>();

            services.AddTransient<IEncryptionService, EncryptionService>();
            services.AddTransient<IJsonEncryptionService, EncryptionService>();

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IDeviceService, DeviceService>();
            services.AddTransient<IGroupService, GroupService>();
            services.AddTransient<IDeviceService, DeviceService>();

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
