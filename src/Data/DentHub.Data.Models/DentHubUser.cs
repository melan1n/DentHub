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
		public DentHubUser()
		{
			this.PatientRecords = new HashSet<PatientRecord>();
		}

		public DentHubUser(string userName) : base()
		{
			this.UserName = userName;
		}


		public string FirstName { get; set; }

		public string LastName { get; set; }

		public UserType UserType { get; set; }

		// Dentist-specific properties
		public Specialty Specialty { get; set; }

		public int? SpecialtyId { get; set; }

		public string ImageUrl { get; set; }

		public virtual Clinic Clinic { get; set; }

		public int? ClinicId { get; set; }

		// Patient-specific properties
		public string SSN { get; set; }

		public virtual ICollection<PatientRecord> PatientRecords { get; set; }

	}
}
