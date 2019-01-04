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
                    appointmentsViewModel.Appointments = _appointmentService
                        .GetAllDentistAppointments(user.Id)
                        //.Where(a => a.Status.ToString() != "Offering")
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
                        }).ToArray();
                }
                else
                {
                    appointmentsViewModel.Appointments = _appointmentService
                            .GetAllPatientAppointments(user.Id)
                            .Where(a => a.Status.ToString() != "Offering")
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
            //if (appointmentInputModel.Duration < 15 || appointmentInputModel.Duration > 8*60)
            //{

            //	ViewBag["Error"] = "Appointments should last between 15 minutes and 8 hours.";
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

            var duration = (appointment.TimeEnd - appointment.TimeStart).Minutes;

            if (duration < 15 || duration > 8*60)
            {
                ViewBag.ErrorMessage = "Appointments should last between 15 minutes and 8 hours. Please input a valid start and end.";
                //ModelState.AddModelError("", "Appointments should last between 15 minutes and 8 hours.");
                return View("CreateOffering");
            }

			await _appointmentService.AddAsync(appointment);
			await _appointmentService.SaveChangesAsync();

			return RedirectToAction("Index");
		}

		[Authorize(Roles = "Patient")]
		public async Task<IActionResult> Book(int id)
		{
			var user = await _userManager.GetUserAsync(User);

			var appointment = this._appointmentService
					.GetAppointmentById(id);

			if (appointment != null)
			{
				appointment.Patient = user;
				appointment.Status = Status.Booked;
			}

			this._appointmentService.Update(appointment);
			await this._appointmentService.SaveChangesAsync();

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
			var appointment = this._appointmentService
						.GetAppointmentById(id);

			var patient = this._patientService
						.GetPatientById(appointment.PatientId);

			var appointmentViewModel = new AppointmentViewModel
			{
				PatientName = this._patientService.GetPatientFullName(patient.Id),
				Id = appointment.Id,
				TimeStart = appointment.TimeStart,
				TimeEnd = appointment.TimeEnd,
				Status = appointment.Status.ToString(),
			};

			return View(appointmentViewModel);
		}

		[Authorize(Roles = "Dentist")]
		public IActionResult Cancel(int id)
		{
			var appointment = this._appointmentService
					.GetAppointmentById(id);

			this._appointmentService.Delete(appointment);
			this._appointmentService.SaveChangesAsync();

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