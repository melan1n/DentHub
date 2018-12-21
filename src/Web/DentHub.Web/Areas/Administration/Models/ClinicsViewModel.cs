using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentHub.Web.Areas.Administration.Models
{
	public class ClinicsViewModel
	{
		public ClinicsViewModel()
		{
			this.Clinics = new HashSet<ClinicViewModel>(); 
		}

		public int Id { get; set; }

		public IEnumerable<ClinicViewModel> Clinics;


	}
}
