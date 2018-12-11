using CommandLine;
using DentHub.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
			//using (var serviceScope = serviceProvider.CreateScope())
			//{
			//	var dbContext = serviceScope.ServiceProvider.GetRequiredService<DentHubContext>();
			//	dbContext.Database.Migrate();
			//	ApplicationDbContextSeeder.Seed(dbContext, serviceScope.ServiceProvider);
			//}

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

		private static void ConfigureServices(ServiceCollection serviceCollection)
		{
			var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", false, true)
				.AddEnvironmentVariables()
				.Build();

			serviceCollection.AddDbContext<DentHubContext>(options =>
					options.UseSqlServer(
						configuration.GetConnectionString("DefaultConnection")));



			//	services.AddSingleton<IConfiguration>(configuration);
			//	services.AddDbContext<ApplicationDbContext>(
			//		options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
			//			.UseLoggerFactory(new LoggerFactory()));

			//	services
			//		.AddIdentity<ApplicationUser, ApplicationRole>(options =>
			//		{
			//			options.Password.RequireDigit = false;
			//			options.Password.RequireLowercase = false;
			//			options.Password.RequireUppercase = false;
			//			options.Password.RequireNonAlphanumeric = false;
			//			options.Password.RequiredLength = 6;
			//		})
			//		.AddEntityFrameworkStores<ApplicationDbContext>()
			//		.AddUserStore<ApplicationUserStore>()
			//		.AddRoleStore<ApplicationRoleStore>()
			//		.AddDefaultTokenProviders();

			//	services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
			//	services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
			//	services.AddScoped<IDbQueryRunner, DbQueryRunner>();

			//	// Application services
			//	services.AddTransient<IEmailSender, NullMessageSender>();
			//	services.AddTransient<ISmsSender, NullMessageSender>();
			//	services.AddTransient<ISettingsService, SettingsService>();
			//
		}
	}
}
