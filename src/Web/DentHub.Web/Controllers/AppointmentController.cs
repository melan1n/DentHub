using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentHub.Data.Common;
using DentHub.Data.Models;
using DentHub.Web.Models;
using DentHub.Web.Models.Appointment;
using DentHub.Web.Services.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DentHub.Web.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly UserManager<DentHubUser> _userManager;
        //private readonly IRepository<DentHubUser> _userRepository;
        private readonly IClinicService _clinicService;
        private readonly IAppointmentService _appointmentService;
        private readonly IDentistService _dentistService;
        private readonly IPatientService _patientService;

        public AppointmentController(IClinicService clinicService,
            //IRepository<DentHubUser> userRepository,
            UserManager<DentHubUser> userManager,
            IAppointmentService appointmentService,
            IDentistService dentistService,
            IPatientService patientService)
        {
            this._userManager = userManager;
            this._clinicService = clinicService;
            //this._userRepository = userRepository;
            this._appointmentService = appointmentService;
            this._dentistService = dentistService;
            this._patientService = patientService;
        }

        [Authorize(Roles = "Dentist,Patient")]
        public IActionResult Index()
        {
			var user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();

			var appointmentsViewModel = new AppointmentsViewModel();

			if (user != null)
			{
				if (this.User.IsInRole("Dentist"))
				{
					appointmentsViewModel.Appointments = _appointmentService
						.GetAllDentistAppointments(user.Id)
						.Where(a => (a.Status.ToString() == "Booked"
										|| (a.Status.ToString() == "Offering" &&
											DateTime.Now < a.TimeStart)))
						.OrderByDescending(a => a.TimeStart)
						.Select(a => new AppointmentViewModel
						{
							Id = a.Id,
							ClinicName = this._clinicService
									.GetClinic(a.ClinicId).Name,
							DentistName = this._dentistService.GetDentistFullName(a.DentistID),
							PatientName = this._patientService.GetPatientFullName(a.PatientId),
							TimeStart = a.TimeStart,
							TimeEnd = a.TimeEnd,
							Status = a.Status.ToString(),
							IsRatedByDentist = a.IsRatedByDentist,
							IsRatedByPatient = a.IsRatedByPatient,
						}).ToArray();
				}
				else
				{
					appointmentsViewModel.Appointments = _appointmentService
							.GetAllPatientAppointments(user.Id)
							.Where(a => a.Status.ToString() == "Booked")
							.OrderByDescending(a => a.TimeStart)
							.Select(a => new AppointmentViewModel
							{
								Id = a.Id,
								ClinicName = this._clinicService
									.GetClinic(a.ClinicId).Name,
								DentistName = this._dentistService.GetDentistFullName(a.DentistID),
								PatientName = this._patientService.GetPatientFullName(a.PatientId),
								TimeStart = a.TimeStart.Date,
								TimeEnd = a.TimeEnd,
								Status = a.Status.ToString(),
								IsRatedByDentist = a.IsRatedByDentist,
								IsRatedByPatient = a.IsRatedByPatient,
							})
							.ToArray();
				}
			}

            return View(appointmentsViewModel);
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

			if (!ModelState.IsValid)
            {
                 ViewBag.ErrorMessage = "Appointment start should be after at least one hour but not later than in 1 year. Appointment duration should be between 15 minutes and 8 hours. Please input a valid start and end.";
                return View("CreateOffering");
            }

			var duration = (appointmentInputModel.TimeEnd).Subtract(appointmentInputModel.TimeStart).TotalMinutes;

			if (duration < 15 || duration > 8 * 60)
			{
				ViewBag.ErrorMessage = "Appointments should last between 15 minutes and 8 hours. Please input a valid start and end.";
				return View("CreateOffering");
			}

			var minutesFromNow = (int)(appointmentInputModel.TimeEnd).Subtract(DateTime.Now).TotalMinutes;

			if (minutesFromNow < 60 || minutesFromNow > 365*24*80)
			{
				ViewBag.ErrorMessage = "Appointment start should be after at least one hour but not later than in 1 year. Please input a valid start and end.";
				return View("CreateOffering");
			}

			if (this._appointmentService.DuplicateOfferingExists(user, appointmentInputModel.TimeStart, appointmentInputModel.TimeEnd))
			{
				ViewBag.ErrorMessage = "Your new offering's time overlaps with an existing offering. Please input a valid start and end or cancel your other offering before you proceed to create the new one.";
				return View("CreateOffering");
			}

			await this._appointmentService
				.CreateAppointment(user, appointmentInputModel.TimeStart, appointmentInputModel.TimeEnd);

			return RedirectToAction("Index");
		}

		[Authorize(Roles = "Patient")]
		public async Task<IActionResult> Book(int id)
		{
			var user = await _userManager.GetUserAsync(User);

			await this._appointmentService
				.BookAppointmentAsync(id, user);

			return RedirectToAction("Index");
		}

		// // Uncomment if you revive status "Confirmed"
		//[Authorize(Roles = "Dentist")]
		//public async Task<IActionResult> Confirm(int id)
		//{
		//	var appointment = this._appointmentService
		//				.All()
		//				.FirstOrDefault(a => a.Id == id);
		//	if (appointment != null)
		//	{
		//		appointment.Status = Status.Confirmed;
		//	}

		//	this._appointmentRepository.Update(appointment);
		//	await this._appointmentRepository.SaveChangesAsync();

		//	return RedirectToAction("Index");
		//}

		[Authorize(Roles = "Dentist")]
		public IActionResult Details(int id)
		{
			Appointment appointment;
			try
			{
				appointment = this._appointmentService
										.GetAppointmentById(id);
			}
			catch (Exception)
			{
				ViewBag.ErrorMessage = "No such appointment exists.";
				return View("ErrorMessage");
			}

			string patientName = "Not Appointed";

			DentHubUser patient;
			if (appointment.Status == Status.Booked)
			{
				try
				{
					patient = this._patientService
						.GetPatientById(appointment.PatientId);
				}
				catch (Exception)
				{
					ViewBag.ErrorMessage = "No such patient exists.";
					return View("ErrorMessage");
				}

				patientName = this._patientService.GetPatientFullName(patient.Id);
			}
			
			var appointmentViewModel = new AppointmentViewModel
			{
				PatientName = patientName,
				Id = appointment.Id,
				TimeStart = appointment.TimeStart,
				TimeEnd = appointment.TimeEnd,
				Duration = (appointment.TimeEnd - appointment.TimeStart).Minutes,
				Status = appointment.Status.ToString(),
			};

			return View(appointmentViewModel);
		}

		[Authorize(Roles = "Dentist")]
		public IActionResult Cancel(int id)
		{
			this._appointmentService
				.CancelAppointmentAsync(id);

			return RedirectToAction("Index");
		}

		// //Uncomment if you revive status "Completed"
		//[Authorize(Roles = "Dentist,Patient")]
		//public async Task<IActionResult> Complete(int id)
		//{
		//	var appointment = this._appointmentRepository
		//				.All()
		//				.FirstOrDefault(a => a.Id == id);
		//	if (appointment != null)
		//	{
		//		appointment.Status = Status.Completed;
		//	}

		//	this._appointmentRepository.Update(appointment);
		//	await this._appointmentRepository.SaveChangesAsync();

		//	return RedirectToAction("Index");
		//}

		//[Authorize(Roles = "Dentist,Patient")]
		//public async Task<IActionResult> Cancel(int id)
		//{
		//	var appointment = this._appointmentRepository
		//				.All()
		//				.FirstOrDefault(a => a.Id == id);

		//	var user = await this._userManager.GetUserAsync(User);

		//	if (appointment != null)
		//	{
		//		//if executed by a dentist
		//		if (user.SSN != null)
		//		{
		//			_appointmentRepository.Delete(appointment);
		//		}
		//		else
		//		{
		//			appointment.Status = Status.Offering;
		//			this._appointmentRepository.Update(appointment);

		//		}
		//	}

		//	await this._appointmentRepository.SaveChangesAsync();

		//	return RedirectToAction("Index");
		//}

	}
}