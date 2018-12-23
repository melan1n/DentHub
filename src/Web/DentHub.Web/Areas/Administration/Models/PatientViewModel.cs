using System.ComponentModel.DataAnnotations;

namespace DentHub.Web.Areas.Administration.Models
{
	public class PatientViewModel
	{
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

	}
}