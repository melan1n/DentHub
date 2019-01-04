using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentHub.Web.Models.Appointment
{
	public class AppointmentViewModel
	{
		public int Id { get; set; }

		public string DentistName { get; set; }

		[Display(Name = "Patient Name")]
		public string PatientName { get; set; }

		[Display(Name = "Clinic Name")]
		public string ClinicName { get; set; }

		[Display(Name = "Start")]
		public DateTime TimeStart { get; set; }

		[Display(Name = "End")]
		public DateTime TimeEnd { get; set; }

		[Display(Name = "Duration (minutes)")]
		public int Duration { get; set; }
	
		[Display(Name = "Status")]
		public string Status { get; set; }

		public bool IsRatedByPatient { get; set; } = false;

		public bool IsRatedByDentist { get; set; } = false;
	}
}
