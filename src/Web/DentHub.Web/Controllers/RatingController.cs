using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentHub.Data.Common;
using DentHub.Data.Models;
using DentHub.Web.Models.Rating;
using DentHub.Web.Services.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DentHub.Web.Controllers
{
    public class RatingController : Controller
    {

		private readonly IAppointmentService _appointmentService;
		private readonly IRatingService _ratingService;
		private readonly UserManager<DentHubUser> _userManager;
		private readonly IDentistService _dentistService;
		private readonly IPatientService _patientService;

		public RatingController(IDentistService dentistService,
			UserManager<DentHubUser> userManager,
			IRatingService ratingService,
            IAppointmentService appointmentService,
			IPatientService patientService)
		{
			this._userManager = userManager;
			this._dentistService = dentistService;
            this._appointmentService = appointmentService;
			this._ratingService = ratingService;
			this._patientService = patientService;
		}

		public IActionResult Index()
        {
            return View();
        }

		[Authorize(Roles = "Patient,Dentist")]
		public IActionResult RateAppointment(int id)
		{
            var appointment = this._appointmentService
                    .GetAppointmentById(id);

            var ratingInputModel = new RatingInputModel
			{
				DentistId = appointment.DentistID,
				PatientId = appointment.PatientId,
				AppointmentId = id,
			};

			return View(ratingInputModel);
		}

        [HttpPost]
        [Authorize(Roles = "Patient,Dentist")]
        public async Task<IActionResult> RateAppointment(int appointmentId, int rating)
        {
			if (!ModelState.IsValid || rating < 1 || rating > 10)
			{
				this.ViewBag.ErrorMessage = "Rating should be a whole number between 1 and 10. Please provide a valid number.";
				return View();
			}

			var ratingRecord = this._ratingService
                   .GetRatingForAppointment(appointmentId);

            var appointment = this._appointmentService
                    .GetAppointmentById(appointmentId);

            var dentist = this._dentistService
                    .GetAppointmentDentist(appointmentId);

			var patient = this._patientService
					.GetAppointmentPatient(appointmentId);

            if (User.IsInRole("Dentist"))
            {
                ratingRecord.RatingByDentist = rating;
                ratingRecord.Appointment.IsRatedByDentist = true;
            }
            else if (User.IsInRole("Patient"))
            {
                ratingRecord.RatingByPatient = rating;
                ratingRecord.Appointment.IsRatedByPatient = true;
            }

            // Uncomment if you revive status "Completed"
            //appointment.Status = Status.Completed;

            this._appointmentService.Update(appointment);
            this._ratingService.Update(ratingRecord);

            await this._appointmentService.SaveChangesAsync();
            await this._ratingService.SaveChangesAsync();

            return RedirectToAction("../Appointment/Index");
        }
    }
}