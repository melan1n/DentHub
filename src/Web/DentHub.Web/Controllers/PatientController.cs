using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DentHub.Data.Common;
using DentHub.Data.Models;
using DentHub.Web.Areas.Administration.Models;
using DentHub.Web.Models.Rating;
using DentHub.Web.Services.DataServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DentHub.Web.Controllers
{
	public class PatientController : Controller
	{
		private readonly UserManager<DentHubUser> _userManager;
		private readonly IDentistService _dentistService;
		private readonly IRatingService _ratingService;
		private readonly IRepository<Appointment> _appointmentRepository;


		public PatientController(UserManager<DentHubUser> userManager,
			IDentistService dentistService,
			IRepository<Appointment> appointmentRepository,
			IRatingService ratingService)
		{
			this._userManager = userManager;
			this._dentistService = dentistService;
			this._appointmentRepository = appointmentRepository;
			this._ratingService = ratingService;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> PatientDentists()
		{
			var user = await this._userManager.GetUserAsync(User);

			var dentists = this._dentistService
				.GetAllPatientDentists(user.Id)
				.Select(d => new DentistViewModel
				{
					Id = d.Id,
					FirstName = d.FirstName,
					LastName = d.LastName,
				})
				.ToArray();

				
			var ratings = this._ratingService
					.GetAllRatingsForPatient(user.Id)
					.Select(r => new RatingInputModel
					{
						DentistId = r.DentistId,
						PatientId = user.Id,
						RatingByDentist = r.RatingByDentist,
					})
					.ToArray();

			if (ratings.Length == 0 && dentists.Length == 0)
			{
				return View("NoDentists");
			}
			var averageRatingForPatientPerDentist = new Dictionary<string, string>();

			foreach (var dentist in dentists)
			{
				var ratingsByDentist = this._ratingService
					.GetAllRatingsForPatientByDentist(user.Id, dentist.Id)
					.Select(r => new RatingInputModel
					{
						DentistId = r.DentistId,
						PatientId = user.Id,
						RatingByDentist = r.RatingByDentist,
					})
					.ToArray();

				if (ratingsByDentist.Length > 0)
				{
					double averageRating = ratingsByDentist
						.Average(r => r.RatingByDentist);

					averageRatingForPatientPerDentist[dentist.FirstName + " " + dentist.LastName] = 
						averageRating.ToString("0.00", CultureInfo.InvariantCulture);
				}
				else
				{
					averageRatingForPatientPerDentist[dentist.FirstName + " " + dentist.LastName] = 
						"Not Rated";
				}				
			}

			var patientViewModel = new PatientViewModel
			{
				AverageRatingByDentist = averageRatingForPatientPerDentist,
				AverageRating = ratings.Count() > 0 ?
								ratings.Average(r => r.RatingByDentist).ToString("0.00", CultureInfo.InvariantCulture) :
								"Not Rated"
			};

			return View(patientViewModel);
		}
	}
}