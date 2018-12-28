using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentHub.Data.Common;
using DentHub.Data.Models;
using DentHub.Web.Areas.Administration.Models;
using DentHub.Web.Models.Appointment;
using DentHub.Web.Models.Rating;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DentHub.Web.Controllers
{
    public class DentistController : Controller
    {
		private readonly UserManager<DentHubUser> _userManager;
		private readonly IRepository<DentHubUser> _userRepository;
		private readonly IRepository<Rating> _ratingRepository;
		private readonly IRepository<Appointment> _appointmentRepository;


		public DentistController(UserManager<DentHubUser> userManager,
			IRepository<DentHubUser> userRepository,
			IRepository<Appointment> appointmentRepository,
			IRepository<Rating> ratingRepository)
		{
			this._userManager = userManager;
			this._userRepository = userRepository;
			this._appointmentRepository = appointmentRepository;
			this._ratingRepository = ratingRepository;
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

		public async Task<IActionResult> DentistPatients()
		{
			var user = await this._userManager.GetUserAsync(User);

			var patients = this._appointmentRepository
					.All()
					.Where(a => a.DentistID == user.Id)
					.Select(a => new PatientViewModel
					{
						Id = a.PatientId,
						FirstName = a.Patient.FirstName,
						LastName = a.Patient.LastName,
					}
					).ToArray();

			var ratings = this._ratingRepository
					.All()
					.Where(r => r.DentistId == user.Id
					&& r.RatingByPatient > 0)
					.Select(r => new RatingInputModel
					{
						DentistId = user.Id,
						PatientId = r.PatientId,
						RatingByPatient = r.RatingByPatient,
					})
					.ToArray();

			var averageRatingPerPatient = new Dictionary<string, string>();

			foreach (var patient in patients)
			{
				if (ratings.Length > 0)
				{
					double averageRating = ratings
					.Where(r => r.PatientId == patient.Id)
					.Average(r => r.RatingByPatient);

					averageRatingPerPatient[patient.FirstName + patient.LastName] = averageRating.ToString();
				}
				else
				{
					averageRatingPerPatient[patient.FirstName + patient.LastName] = "N/A";
				}
			}

			var dentistViewModel = new DentistViewModel
			{
				AverageRatingByPatient = averageRatingPerPatient,
				AverageRating = ratings.Count() > 0 ? 
								ratings.Average(r => r.RatingByPatient).ToString() :
								"N/A"
			};

			return View(dentistViewModel);
		}
	}
}