using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentHub.Web.Models.Appointment
{
	public class AppointmentViewModel
	{
		public int Id { get; set; }

		public string DentistName { get; set; }

		public string PatientName { get; set; }

		public string ClinicName { get; set; }

		public DateTime TimeStart { get; set; }

		public DateTime TimeEnd { get; set; }

		public double Duration => (this.TimeEnd - this.TimeStart).Hours;

		public string Status { get; set; }
	}
}
