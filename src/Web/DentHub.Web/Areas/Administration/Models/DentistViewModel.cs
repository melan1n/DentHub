using DentHub.Data.Models;
using DentHub.Web.Models.Appointment;
using DentHub.Web.Models.Rating;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DentHub.Web.Areas.Administration.Models
{
	public class DentistViewModel
	{
		public DentistViewModel()
		{
			this.Offerrings = new HashSet<AppointmentViewModel>();
			this.Patients = new HashSet<PatientViewModel>();
			this.AverageRatingByPatient = new Dictionary<string, string[]>();
		}

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

		public string AverageRating { get; set; }

		public Dictionary<string, string[]> AverageRatingByPatient { get; set; }
		
		public ICollection<AppointmentViewModel> Offerrings { get; set; }

		public ICollection<PatientViewModel> Patients { get; set; }

		}
}