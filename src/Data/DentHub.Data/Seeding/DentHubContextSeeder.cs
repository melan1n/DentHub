using DentHub.Common;
using DentHub.Data.Models;
using DentHub.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DentHub.Data.Seeding
{
	public static class DentHubContextSeeder
	{
		public static void Seed(DentHubContext dbContext, IServiceProvider serviceProvider)
		{
			if (dbContext == null)
			{
				throw new ArgumentNullException(nameof(dbContext));
			}

			if (serviceProvider == null)
			{
				throw new ArgumentNullException(nameof(serviceProvider));
			}

			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			//var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
			Seed(dbContext, roleManager);
			//SeedAdmin(GlobalConstants.AdminUserName, GlobalConstants.AdministratorRoleName, userManager, roleManager);
		}

		public static void Seed(DentHubContext dbContext, RoleManager<IdentityRole> roleManager)
		{
			if (dbContext == null)
			{
				throw new ArgumentNullException(nameof(dbContext));
			}

			if (roleManager == null)
			{
				throw new ArgumentNullException(nameof(roleManager));
			}

			SeedRoles(roleManager);
		}

		private static void SeedRoles(RoleManager<IdentityRole> roleManager)
		{
			SeedRole(GlobalConstants.AdministratorRoleName, roleManager);
			SeedRole(GlobalConstants.UserRoleName, roleManager);
			SeedRole(GlobalConstants.DentistRoleName, roleManager);
			SeedRole(GlobalConstants.PatientRoleName, roleManager);

		}

		private static void SeedRole(string roleName, RoleManager<IdentityRole> roleManager)
		{
			var role = roleManager.FindByNameAsync(roleName).GetAwaiter().GetResult();
			if (role == null)
			{
				var result = roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();

				if (!result.Succeeded)
				{
					throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
				}
			}
		}

		//private static void SeedAdmin(string userName, string roleName, UserManager<DentHubUser> userManager, RoleManager<IdentityRole> roleManager)
		//{
		//	var user = userManager.FindByNameAsync(userName).GetAwaiter().GetResult();
		//	var role = roleManager.FindByNameAsync(roleName).GetAwaiter().GetResult();

		//	if (role == null)
		//	{
		//		var result = roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();

		//		if (!result.Succeeded)
		//		{
		//			throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
		//		}
		//	}

		//	if (user == null)
		//	{
		//		var result = userManager.CreateAsync(new DentHubUser(GlobalConstants.AdminUserName)).GetAwaiter().GetResult();

		//		if (!result.Succeeded)
		//		{
		//			throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
		//		}
		//	}

		//	userManager.AddToRoleAsync(user, "Admin");
		//}

	}
}
