using DentHub.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DentHub.Data.Models
{
	public class Rating : BaseModel<int>
	{
		public virtual Appointment Appointment { get; set; }
		public int AppointmentId { get; set; }

		public virtual DentHubUser Dentist { get; set; }
		public string DentistId { get; set; }

		public virtual DentHubUser Patient { get; set; }
		public string PatientId { get; set; }

		[Range(1, 10)]
		public int RatingByDentist { get; set; }

		[Range(1, 10)]
		public int RatingByPatient { get; set; }
	}
}
