using System;
using System.Collections.Generic;
using System.Text;

namespace DentHub.Data.Models
{
	public class Patient : DentHubUser
	{
		public Patient()
		{
			this.PatientRecords = new HashSet<PatientRecord>();
		}

		public string SSN { get; set; }

		public ICollection<PatientRecord> PatientRecords { get; set; }
	}
}
