using System;
using System.Collections.Generic;
using System.Text;

namespace DentHub.Data.Models
{
	public class Dentist : DentHubUser
	{
		public Specialty Specialty { get; set; }

		public int SpecialtyId { get; set; }

		public string ImageUrl { get; set; }

		public virtual Clinic Clinic { get; set; }

		public int ClinicId { get; set; }
	}
}
