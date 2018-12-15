using DentHub.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DentHub.Data.Models
{
	public class Rating : BaseModel<int>
	{
		public virtual Appointment Appointment { get; set; }
		public int ForAppointmentId { get; set; }

		public virtual DentHubUser Dentist { get; set; }
		public string DentistId { get; set; }

		public virtual DentHubUser Patient { get; set; }
		public string PatientId { get; set; }

		public int RatingByDentist { get; set; }

		public int RatingByPatient { get; set; }
	}
}
