using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentHub.Data.Common;
using DentHub.Data.Models;
using DentHub.Web.Models.Appointment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DentHub.Web.Controllers
{
    public class DentistController : Controller
    {
		private readonly UserManager<DentHubUser> _userManager;
		private readonly IRepository<DentHubUser> _userRepository;
		//private readonly IRepository<Clinic> _clinicRepository;
		private readonly IRepository<Appointment> _appointmentRepository;

		public DentistController(UserManager<DentHubUser> userManager,
			IRepository<DentHubUser> userRepository,
			IRepository<Appointment> appointmentRepository)
		{
			this._userManager = userManager;
			this._userRepository = userRepository;
			this._appointmentRepository = appointmentRepository;
		}

		public IActionResult Index()
        {
            return View();
        }

		[Authorize(Roles="Patient")]
		public IActionResult Offerings(string id)
		{
			var appointmentsViewModel = new AppointmentsViewModel();
			GetDentistOfferings(id, appointmentsViewModel);

			return View(appointmentsViewModel);
		}

		private void GetDentistOfferings(string id, AppointmentsViewModel appointmentsViewModel)
		{
			var dentist = this._userRepository
				.All()
				.FirstOrDefault(u => u.Id == id);

			if (dentist != null)
			{
					appointmentsViewModel.Appointments = _appointmentRepository.
						All()
						.Where(a => a.DentistID == dentist.Id && a.Status.ToString() == "Offering")
						.Select(a => new AppointmentViewModel
						{
							Id = a.Id,
							ClinicName = a.Clinic.Name,
							DentistName = a.Dentist.FirstName + a.Dentist.LastName,
							PatientName = a.Patient.FirstName + a.Patient.LastName,
							TimeStart = a.TimeStart.Date,
							TimeEnd = a.TimeEnd,
							Status = a.Status.ToString(),
						}).ToArray();
				}
			}
	}
}