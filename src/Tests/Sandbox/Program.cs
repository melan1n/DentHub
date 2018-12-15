using CommandLine;
using DentHub.Data;
using DentHub.Data.Common;
using DentHub.Data.Models;
using DentHub.Data.Seeding;
using DentHub.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;

namespace Sandbox
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine($"{typeof(Program).Namespace} ({string.Join(" ", args)}) starts working...");
			var serviceCollection = new ServiceCollection();
			ConfigureServices(serviceCollection);
			IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);

			// Seed data on application startup
			using (var serviceScope = serviceProvider.CreateScope())
			{
				var dbContext = serviceScope.ServiceProvider.GetRequiredService<DentHubContext>();
				//var userManager = serviceScope.ServiceProvider.GetService<UserManager<DentHubUser>>();
				dbContext.Database.Migrate();
				DentHubContextSeeder.Seed(dbContext, serviceScope.ServiceProvider);
			}

			using (var serviceScope = serviceProvider.CreateScope())
			{
				serviceProvider = serviceScope.ServiceProvider;
				SandboxCode(serviceProvider);
			}
		}

		private static void SandboxCode(IServiceProvider serviceProvider)
		{
			// TODO: code here
			var db = serviceProvider.GetService<DentHubContext>();
			Console.WriteLine(db.Users.Count());
		}

		private static void ConfigureServices(ServiceCollection services)
		{
			var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", false, true)
				.AddEnvironmentVariables()
				.Build();

			services.AddSingleton<IConfiguration>(configuration);
			services.AddDbContext<DentHubContext>(
				options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
						.UseLoggerFactory(new LoggerFactory()));

			//services.AddDbContext<DentHubContext>(options =>
			//		options.UseSqlServer(
			//			configuration.GetConnectionString("DefaultConnection")));

			services.AddScoped<DbContext, DentHubContext>();
			services.AddTransient<IUserStore<IdentityUser>, UserStore<IdentityUser>>();
			services.AddTransient<IPasswordHasher<DentHubUser>, PasswordHasher<DentHubUser>>();
			services.AddTransient<UserManager<DentHubUser>>(); 

			// Application services
			services.AddScoped(typeof(IRepository<>), typeof(DbRepository<>));

			

			services
				.AddIdentity<DentHubUser, IdentityRole>(options =>
				{
					options.Password.RequireDigit = false;
					options.Password.RequireLowercase = false;
					options.Password.RequireUppercase = false;
					options.Password.RequiredUniqueChars = 0;
					options.Password.RequireNonAlphanumeric = false;
					options.Password.RequiredLength = 6;
				})
				.AddEntityFrameworkStores<DentHubContext>()
				.AddUserStore<UserStore>()
				.AddRoleStore<RoleStore<IdentityRole>>()
				.AddDefaultTokenProviders();

			//services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
			//services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
			//services.AddScoped<IDbQueryRunner, DbQueryRunner>();

			//// Application services
			//services.AddTransient<IEmailSender, NullMessageSender>();
			//services.AddTransient<ISmsSender, NullMessageSender>();
			//services.AddTransient<ISettingsService, SettingsService>();

		}
	}
}
