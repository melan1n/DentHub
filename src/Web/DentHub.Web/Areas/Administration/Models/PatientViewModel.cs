using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DentHub.Web.Areas.Administration.Models
{
	public class PatientViewModel
	{
		public PatientViewModel()
		{
			this.AverageRatingByDentist = new Dictionary<string, string>();
		}

		public string Id { get; set; }

		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		[Display(Name = "User Name")]
		public string UserName { get; set; }

		// Patient-specific properties
		[Display(Name = "Social Security Number")]
		public string SSN { get; set; }

		public string AverageRating { get; set; }

		public Dictionary<string, string> AverageRatingByDentist { get; set; }
	}
}