using DentHub.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DentHub.Data.Models
{
	public class Appointment : BaseModel<int>
	{
		public virtual Dentist Dentist { get; set; }
		public string DentistID { get; set; }

		public virtual Patient Patient { get; set; }
		public string PatientId { get; set; }

		public virtual Clinic Clinic { get; set; }
		public int ClinicId { get; set; }

		public DateTime Time { get; set; }

		public double Duration { get; set; }

		public Status Status { get; set; }

		//public string Name => this.Clinic.Name + this.Dentist.LastName + this.Time;

		public bool IsRatedByPatient { get; set; } = false;

		public bool IsRatedByDentist { get; set; } = false;

	}
}
