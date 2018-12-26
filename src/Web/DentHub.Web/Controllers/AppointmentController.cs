using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentHub.Data.Common;
using DentHub.Data.Models;
using DentHub.Web.Models;
using DentHub.Web.Models.Appointment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DentHub.Web.Controllers
{
	public class AppointmentController : Controller
	{
		private readonly UserManager<DentHubUser> _userManager;
		private readonly IRepository<DentHubUser> _userRepository;
		private readonly IRepository<Clinic> _clinicRepository;
		private readonly IRepository<Appointment> _appointmentRepository;

		public AppointmentController(IRepository<Clinic> clinicRepository,
			IRepository<DentHubUser> userRepository,
			UserManager<DentHubUser> userManager,
			IRepository<Appointment> appointmentRepository)
		{
			this._userManager = userManager;
			this._clinicRepository = clinicRepository;
			this._userRepository = userRepository;
			this._appointmentRepository = appointmentRepository;
		}

		[Authorize(Roles = "Dentist,Patient")]
		public  IActionResult Index()
		{
			var appointmentsViewModel = new AppointmentsViewModel();
			GetMyAppointments(appointmentsViewModel);

			return View(appointmentsViewModel);
		}

		private void GetMyAppointments(AppointmentsViewModel appointmentsViewModel)
		{
			var user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
			
			if (user != null)
			{
				if (this.User.IsInRole("Dentist"))
				{
					appointmentsViewModel.Appointments = _appointmentRepository.
						All()
						.Where(a => a.DentistID == user.Id)
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
				else
				{
					appointmentsViewModel.Appointments = _appointmentRepository.
							All()
							.Where(a => a.PatientId == user.Id)
							.Select(a => new AppointmentViewModel
							{
								Id = a.Id,
								ClinicName = a.Clinic.Name,
								DentistName = a.Dentist.FirstName + a.Dentist.LastName,
								PatientName = a.Patient.FirstName + a.Patient.LastName,
								TimeStart = a.TimeStart.Date,
								TimeEnd = a.TimeEnd,
								Status = a.Status.ToString(),
							})
							.ToArray();
				}
			}
		
		}

		[Authorize(Roles = "Dentist")]
		public IActionResult CreateOffering()
		{
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "Dentist")]
		public async Task<IActionResult> CreateOffering(AppointmentInputModel appointmentInputModel)
		{
			var user = await _userManager.GetUserAsync(User);

			//// Uncomment throwing exception for server-side validation only
			//if (appointmentInputModel.Duration <= 0 || appointmentInputModel.Duration > 8)
			//{

			//	//ViewBag["Error"] = "Appointments should last between 15 minutes and 8 hours.";
			//	return RedirectToAction("CreateOffering");

			//	//throw new InvalidOperationException("Appointments should last between 15 minutes and 8 hours.");
			//}

			if (!ModelState.IsValid)
			{
				//ViewData["ErrorMessage"] = "Appointments should last between 15 minutes and 8 hours.";
				ViewBag.ErrorMessage = "Appointments should last between 15 minutes and 8 hours. Please input a valid start and end.";
				//ModelState.AddModelError("", "Appointments should last between 15 minutes and 8 hours.");
				return View("CreateOffering");
			}

			var appointment = new Appointment
			{
				ClinicId = (int)user.ClinicId,
				Dentist = user,
				Status = Status.Offering,
				TimeStart = appointmentInputModel.TimeStart,
				TimeEnd = appointmentInputModel.TimeEnd,
			};

			await _appointmentRepository.AddAsync(appointment);
			await _appointmentRepository.SaveChangesAsync();

			return RedirectToAction("Index");

		}
	}
}