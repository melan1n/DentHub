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
		//private readonly IPatientService _patientService;
		private readonly IRepository<Rating> _ratingRepository;
		private readonly IAppointmentService _appointmentService;
		private readonly IClinicService _clinicService;
			   
		public DentistController(UserManager<DentHubUser> userManager,
			IDentistService dentistService,
			//IPatientService patientService,
			IAppointmentService appointmentService,
			IRepository<Rating> ratingRepository,
			IClinicService clinicService)
		{
			this._userManager = userManager;
			this._dentistService = dentistService;
			//this._patientService = patientService;
			this._appointmentService = appointmentService;
			this._ratingRepository = ratingRepository;
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

		//public async Task<IActionResult> DentistPatients()
		//{
		//	var user = await this._userManager.GetUserAsync(User);

		//	var patients = this._appointmentRepository
		//			.All()
		//			.Where(a => a.DentistID == user.Id)
		//			.Select(a => new PatientViewModel
		//			{
		//				Id = a.PatientId,
		//				FirstName = a.Patient.FirstName,
		//				LastName = a.Patient.LastName,
		//			})
		//			.Distinct()
		//			.ToArray();

		//	var ratings = this._ratingRepository
		//			.All()
		//			.Where(r => r.DentistId == user.Id
		//			&& r.RatingByPatient > 0)
		//			.Select(r => new RatingInputModel
		//			{
		//				DentistId = user.Id,
		//				PatientId = r.PatientId,
		//				RatingByPatient = r.RatingByPatient,
		//			})
		//			.ToArray();

		//	if (ratings.Length == 0)
		//	{
		//		return View("NoPatients");
		//	}

		//	var averageRatingPerPatient = new Dictionary<string, string[]>();

		//	foreach (var patient in patients)
		//	{
		//		var ratingsByPatient = this._ratingRepository
		//			.All()
		//			.Where(r => r.DentistId == user.Id
		//			&& r.PatientId == patient.Id
		//			&& r.RatingByPatient > 0)
		//			.Select(r => new RatingInputModel
		//			{
		//				DentistId = user.Id,
		//				PatientId = r.PatientId,
		//				RatingByPatient = r.RatingByPatient,
		//			})
		//			.ToArray();

		//		if (ratingsByPatient.Length > 0)
		//		{
		//			double averageRating = ratingsByPatient
		//			.Where(r => r.PatientId == patient.Id)
		//			.Average(r => r.RatingByPatient);

		//			averageRatingPerPatient[patient.Id] = new string[]
		//				{ $"{patient.FirstName} {patient.LastName}",
		//				  averageRating.ToString() };
		//		}
		//		else
		//		{
		//			averageRatingPerPatient[patient.Id] = new string[]
		//				{ $"{patient.FirstName} {patient.LastName}",
		//				  "N/A" };
		//		}
		//	}

		//	var dentistViewModel = new DentistViewModel
		//	{
		//		AverageRatingByPatient = averageRatingPerPatient,
		//		AverageRating = ratings.Count() > 0 ?
		//						ratings.Average(r => r.RatingByPatient).ToString() :
		//						"N/A"
		//	};

		//	return View(dentistViewModel);
		//}
	}
}