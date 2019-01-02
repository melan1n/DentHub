using DentHub.Common;
using DentHub.Data.Models;
using DentHub.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;


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
            var userManager = serviceProvider.GetRequiredService<UserManager<DentHubUser>>();

            Seed(dbContext, roleManager, userManager);

            SeedAdmin(GlobalConstants.AdminUserName, GlobalConstants.AdministratorRoleName, userManager, roleManager);

            SeedSpecialties(dbContext, GlobalConstants.Specialties);

            SeedClinics(dbContext, GlobalConstants.Clinics);

            SeedDentists(dbContext, GlobalConstants.Dentists, userManager, roleManager);

            SeedPatients(dbContext, GlobalConstants.Patients, userManager, roleManager);

        }

        private static void SeedSpecialties(DentHubContext dbContext, string[][] specialties)
        {
            foreach (var specialty in specialties)
            {
                var newSpecialty = new Specialty
                {
                    Name = specialty[0],
                    Description = specialty[1]
                };
                dbContext.Specialties.Add(newSpecialty);
                dbContext.SaveChanges();
            }
        }

        private static void SeedPatients(DentHubContext dbContext, string[][] patients, UserManager<DentHubUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            foreach (var patient in patients)
            {
                var user = new DentHubUser
                {
                    UserName = patient[1],
                    FirstName = patient[3],
                    LastName = patient[4],
                    Email = patient[1],
                    SSN = patient[5],

                };

                var userResult = userManager.CreateAsync(user, patient[2]).GetAwaiter().GetResult();
                var role = roleManager.FindByNameAsync(patient[0]).GetAwaiter().GetResult();

                //if (role == null)
                //{
                //    var result = roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();

                //    if (!result.Succeeded)
                //    {
                //        throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                //    }
                //}

                //if (user == null)
                //{
                //	var result = userManager.CreateAsync(new DentHubUser(GlobalConstants.AdminUserName)).GetAwaiter().GetResult();

                //	if (!result.Succeeded)
                //	{
                //		throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                //	}
                //}

                if (userResult.Succeeded)
                {
                    var resultRole = userManager.AddToRoleAsync(user, role.Name).GetAwaiter().GetResult();
                    var resultClaimRole = userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimTypes.Role, "Patient")).GetAwaiter().GetResult();
                }
            }
        }

        private static void SeedDentists(DentHubContext dbContext, string[][] dentists, UserManager<DentHubUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            foreach (var dentist in dentists)
            {
                var user = new DentHubUser
                {
                    UserName = dentist[1],
                    FirstName = dentist[3],
                    LastName = dentist[4],
                    Email = dentist[1],
                    Clinic = dbContext.Clinics.FirstOrDefault(c => c.Name == dentist[5]),
                    SpecialtyId = dbContext.Specialties.FirstOrDefault(s => s.Name == dentist[6]).Id
                };

                var userResult = userManager.CreateAsync(user, dentist[2]).GetAwaiter().GetResult();
                var role = roleManager.FindByNameAsync(dentist[0]).GetAwaiter().GetResult();

                //if (role == null)
                //{
                //    var result = roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();

                //    if (!result.Succeeded)
                //    {
                //        throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                //    }
                //}

                //if (user == null)
                //{
                //	var result = userManager.CreateAsync(new DentHubUser(GlobalConstants.AdminUserName)).GetAwaiter().GetResult();

                //	if (!result.Succeeded)
                //	{
                //		throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                //	}
                //}

                if (userResult.Succeeded)
                {
                    var resultRole = userManager.AddToRoleAsync(user, role.Name).GetAwaiter().GetResult();
                    var resultClaimRole = userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimTypes.Role, "Dentist")).GetAwaiter().GetResult();
                }
            }


        }

        private static void SeedClinics(DentHubContext dbContext, string[][] clinics)
        {
            foreach (var clinic in clinics)
            {
                var newClinic = new Clinic
                {
                    //Id = int.Parse(clinic[0]),
                    Name = clinic[1],
                    Street = clinic[2],
                    City = clinic[3],
                    Country = clinic[4],
                    PostalCode = clinic[5],
                    WorkingHours = clinic[6]
                };
                dbContext.Clinics.Add(newClinic);
                dbContext.SaveChanges();
            };
        }

        public static void Seed(DentHubContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<DentHubUser> userManager)
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
            //SeedAdmin(DentHub.Common.GlobalConstants.AdminUserName, "Administrator", userManager, roleManager);
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            SeedRole(GlobalConstants.AdministratorRoleName, roleManager);
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

        private static void SeedAdmin(string userName, string roleName, UserManager<DentHubUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var user = new DentHubUser
            {
                UserName = GlobalConstants.AdminUserName,
                FirstName = GlobalConstants.AdminUserName,
                Email = GlobalConstants.AdminUserName,
            };

            var userResult = userManager.CreateAsync(user, GlobalConstants.AdminPassword).GetAwaiter().GetResult();
            var role = roleManager.FindByNameAsync(roleName).GetAwaiter().GetResult();

            if (role == null)
            {
                var result = roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

            if (user == null)
            {
                var result = userManager.CreateAsync(new DentHubUser(GlobalConstants.AdminUserName)).GetAwaiter().GetResult();

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

            if (userResult.Succeeded)
            {
                var resultRole = userManager.AddToRoleAsync(user, role.Name).GetAwaiter().GetResult();
                var resultClaimRole = userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimTypes.Role, "Administrator")).GetAwaiter().GetResult();

            }

        }
    }
}
