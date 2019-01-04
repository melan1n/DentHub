using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DentHub.Web.Models;
using DentHub.Data.Models;
using DentHub.Data.Common;
using DentHub.Data;
using DentHub.Web.Services.DataServices;

namespace DentHub.Web
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
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.AddDbContext<DentHubContext>(options =>
					options.UseSqlServer(
						this.Configuration.GetConnectionString("DefaultConnection")));

			services.AddDefaultIdentity<DentHubUser>(
				options =>
				{
					options.Password.RequiredLength = 6;
					options.Password.RequireDigit = false;
					options.Password.RequireLowercase = false;
					options.Password.RequiredUniqueChars = 0;
					options.Password.RequireNonAlphanumeric = false;
					options.Password.RequireUppercase = false;
				}
				)
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<DentHubContext>();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			//services.AddAuthentication()
			//	.AddFacebook(facebookOptions =>
			//	{
			//		facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
			//		facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
			//	});

			// Application services
			services.AddScoped(typeof(IRepository<>), typeof(DbRepository<>));
            services.AddScoped(typeof(IClinicService), typeof(ClinicService));
            services.AddScoped(typeof(IDentistService), typeof(DentistService));
            services.AddScoped(typeof(IPatientService), typeof(PatientService));
            services.AddScoped(typeof(IAppointmentService), typeof(AppointmentService));
            services.AddScoped(typeof(ISpecialtyService), typeof(SpecialtyService));
            services.AddScoped(typeof(IRatingService), typeof(RatingService));
            services.AddScoped(typeof(IPatientFileService), typeof(PatientFileService));
			services.AddScoped(typeof(CloudinaryService));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();

			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "areas",
					template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");

				
			});
		}
	}
}
