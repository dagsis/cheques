using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DsCheques.Data;
using DsCheques.Data.Entities;
using DsCheques.Data.Repositories.Clases;
using DsCheques.Data.Repositories.Interfaces;
using DsCheques.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; // Cambiado para IWebHostEnvironment
using Microsoft.IdentityModel.Tokens;

namespace DsCheques
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
                cfg.SignIn.RequireConfirmedEmail = false;
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequireDigit = false;
                cfg.Password.RequiredUniqueChars = 0;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequiredLength = 6;
            })
             .AddDefaultTokenProviders()
             .AddEntityFrameworkStores<DataContext>();

            services.AddAuthentication()
            .AddCookie()
            .AddJwtBearer(cfg =>
            {
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = this.Configuration["Tokens:Issuer"],
                    ValidAudience = this.Configuration["Tokens:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(this.Configuration["Tokens:Key"]))
                };
            });

            services.AddDbContext<DataContext>(cfg =>
            {
                cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddTransient<SeedDb>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IChequeRepository, ChequeRepository>();
            services.AddScoped<IUserHelper, UserHelper>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/NotAuthorized";
                options.AccessDeniedPath = "/Account/NotAuthorized";
            });

            // CORRECCIÓN: Se cambia el AddMvc() viejo por el soporte completo de Controladores con Vistas para .NET 8
            services.AddControllersWithViews();
        }

        // CORRECCIÓN: Se cambia IHostingEnvironment por IWebHostEnvironment (estándar moderno)
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            IList<CultureInfo> sc = new List<CultureInfo> { new CultureInfo("es-MX") };

            var lo = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("es-MX"),
                SupportedCultures = sc,
                SupportedUICultures = sc
            };

            var cp = lo.RequestCultureProviders.OfType<CookieRequestCultureProvider>().First();
            cp.CookieName = "UserCulture";
            app.UseRequestLocalization(lo);

            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.UseHttpsRedirection();

            // CORRECCIÓN: Orden estricto de middlewares para .NET 8
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            // Solo una vez declarados los sistemas de identidad
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // Mapea tanto las rutas de Web APIs ([ApiController])
                endpoints.MapControllers();

                // CORRECCIÓN: La ruta por defecto que le faltaba a tus vistas MVC para no dar 404
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}