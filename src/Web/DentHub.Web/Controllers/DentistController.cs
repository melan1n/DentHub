using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentHub.Data.Common;
using DentHub.Data.Models;
using DentHub.Web.Areas.Administration.Models;
using DentHub.Web.Models.Appointment;
using DentHub.Web.Models.Rating;
using DentHub.Web.Services.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DentHub.Web.Controllers
{
	public class DentistController : Controller
	{
		private readonly UserManager<DentHubUser> _userManager;
		private readonly IDentistService _dentistService;
		private readonly IPatientService _patientService;
        private readonly IRatingService _ratingService;
		private readonly IAppointmentService _appointmentService;
		private readonly IClinicService _clinicService;
			   
		public DentistController(UserManager<DentHubUser> userManager,
			IDentistService dentistService,
			IPatientService patientService,
			IAppointmentService appointmentService,
			IRatingService ratingService,
			IClinicService clinicService)
		{
			this._userManager = userManager;
			this._dentistService = dentistService;
			this._patientService = patientService;
			this._appointmentService = appointmentService;
			this._ratingService = ratingService;
			this._clinicService = clinicService;
		}

		public IActionResult Index()
		{
			return View();
		}

		[Authorize(Roles = "Patient")]
		public IActionResult Offerings(string id)
		{
			var appointmentsViewModel = new AppointmentsViewModel
			{
				Appointments = this._appointmentService
							.GetAllDentistAppointments(id)
							.Where(a => a.Status.ToString() == "Offering")
							.Select(a => new AppointmentViewModel
							{
								Id = a.Id,
								ClinicName = this._clinicService
										.GetClinic(a.ClinicId)
										.Name,
								DentistName = $"{this._dentistService.GetDentistById(a.DentistID).FirstName} " +
											$"{this._dentistService.GetDentistById(a.DentistID).LastName}",
								//PatientName = $"{this._patientService.GetPatientById(a.PatientId).FirstName} " +
								//			$"{this._patientService.GetPatientById(a.PatientId).LastName}",
								TimeStart = a.TimeStart.Date,
								TimeEnd = a.TimeEnd,
								Status = a.Status.ToString(),
							}).ToArray()
			};

			if (appointmentsViewModel.Appointments.Count() == 0)
			{
				return View("NoAvailability");
			}
			return View(appointmentsViewModel);
		}

        public async Task<IActionResult> DentistPatients()
        {
            var user = await this._userManager.GetUserAsync(User);

            //var allDentistAppointments = this._appointmentService
            //    .GetAllDentistAppointments(user.Id);

            var patients = this._patientService
                .GetAllDentistPatients(user.Id)
                .Select(a => new PatientViewModel
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                })
                .ToArray();

            //var patients = this._appointmentService
            //        .GetAllDentistAppointments(user.Id)
            //        .Distinct(a => a.PatientId)
            //        .Select(a => new PatientViewModel
            //        {
            //            Id = a.PatientId,
            //            FirstName = this._patientService.GetPatientById(a.PatientId).FirstName,
            //            LastName = this._patientService.GetPatientById(a.PatientId).LastName,
            //        })
            //        .ToArray();

            var ratings = this._ratingService
                    .GetAllRatingsForDentist(user.Id)                                   
                    .Select(r => new RatingInputModel
                    {
                        DentistId = user.Id,
                        PatientId = r.PatientId,
                        RatingByPatient = r.RatingByPatient,
                    })
                    .ToArray();

            if (ratings.Length == 0 && patients.Length == 0)
            {
                return View("NoPatients");
            }

            var averageRatingForDentistPerPatient = new Dictionary<string, string[]>();

            foreach (var patient in patients)
            {
                var ratingsByPatient = this._ratingService
                    .GetAllRatingsForDentistByPatient(user.Id, patient.Id)
                    .Select(r => new RatingInputModel
                    {
                        DentistId = user.Id,
                        PatientId = r.PatientId,
                        RatingByPatient = r.RatingByPatient,
                    })
                    .ToArray();

                if (ratingsByPatient.Length > 0)
                {
                    //double averageRating = ratingsByPatient
                    //.Where(r => r.PatientId == patient.Id)
                    //.Average(r => r.RatingByPatient);

                    double averageRating = ratingsByPatient
                        .Average(r => r.RatingByPatient);

                    averageRatingForDentistPerPatient[patient.Id] = new string[]
                        { $"{patient.FirstName} {patient.LastName}",
                          averageRating.ToString() };
                }
                else
                {
                    averageRatingForDentistPerPatient[patient.Id] = new string[]
                        { $"{patient.FirstName} {patient.LastName}",
                          "Not Rated" };
                }
            }

            var dentistViewModel = new DentistViewModel
            {
                AverageRatingByPatient = averageRatingForDentistPerPatient,
                AverageRating = ratings.Count() > 0 ?
                                ratings.Average(r => r.RatingByPatient).ToString() :
                                "Not Rated"
            };

            return View(dentistViewModel);
        }
    }
}