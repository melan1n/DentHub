using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentHub.Web.Areas.Administration.Models
{
	public class DentistsViewModel
	{
		public DentistsViewModel()
		{
			this.Dentists = new HashSet<DentistViewModel>();
		}

		public int Id { get; set; }

		public IEnumerable<DentistViewModel> Dentists;
	}
}
