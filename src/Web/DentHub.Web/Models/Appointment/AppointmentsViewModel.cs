using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentHub.Web.Models.Appointment
{
	public class AppointmentsViewModel
	{
		public AppointmentsViewModel()
		{
			this.Appointments = new HashSet<AppointmentViewModel>();
		}

		public int Id { get; set; }

		public IEnumerable<AppointmentViewModel> Appointments;
	}
}
