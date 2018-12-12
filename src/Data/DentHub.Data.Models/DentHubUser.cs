using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DentHub.Data.Models
{
    // Add profile data for application users by adding properties to the DentHubUser class
    public class DentHubUser : IdentityUser
    {
		public string FirstName { get; set; }

		public string MiddleName { get; set; }

		public string LastName { get; set; }

		//public DateTime DateCreated { get; set; }
	}
}
