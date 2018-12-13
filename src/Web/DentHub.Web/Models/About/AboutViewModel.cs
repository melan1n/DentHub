using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentHub.Web.Models.About
{
	public class AboutViewModel
	{
		public IEnumerable<SpecialtyViewModel>  Specialties { get; set; }
	}
}
