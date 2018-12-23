using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentHub.Web.Areas.Administration.Models
{
	public class PatientsViewModel
	{
		public PatientsViewModel()
		{
			this.Patients = new HashSet<PatientViewModel>();
		}

		public int Id { get; set; }

		public IEnumerable<PatientViewModel> Patients;
	}
}
