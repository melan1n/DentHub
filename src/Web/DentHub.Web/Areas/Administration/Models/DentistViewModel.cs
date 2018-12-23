using DentHub.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace DentHub.Web.Areas.Administration.Models
{
	public class DentistViewModel
	{
		public string Id { get; set; }

		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		// Dentist-specific properties
		[Display(Name = "Clinic")]
		public string ClinicName { get; set; }

		[Display(Name = "Specialty")]
		public string Specialty { get; set; }

		[Display(Name = "Image")]
		public string ImageUrl { get; set; }

	}
}