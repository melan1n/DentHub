using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentHub.Data.Common;
using DentHub.Data.Models;
using DentHub.Web.Areas.Administration.Models;
using DentHub.Web.Models.Rating;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DentHub.Web.Controllers
{
	public class PatientController : Controller
	{
		private readonly UserManager<DentHubUser> _userManager;
		private readonly IRepository<DentHubUser> _userRepository;
		private readonly IRepository<Rating> _ratingRepository;
		private readonly IRepository<Appointment> _appointmentRepository;


		public PatientController(UserManager<DentHubUser> userManager,
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

		public async Task<IActionResult> PatientDentists()
		{
			var user = await this._userManager.GetUserAsync(User);

			var dentists = this._appointmentRepository
					.All()
					.Where(a => a.PatientId == user.Id)
					.Select(a => new DentistViewModel
					{
						Id = a.DentistID,
						FirstName = a.Dentist.FirstName,
						LastName = a.Dentist.LastName,
					})
					.Distinct()
					.ToArray();

			var ratings = this._ratingRepository
					.All()
					.Where(r => r.PatientId == user.Id 
					&& r.RatingByDentist > 0)
					.Select(r => new RatingInputModel
					{
						DentistId = r.DentistId,
						PatientId = user.Id,
						RatingByDentist = r.RatingByDentist,
					})
					.ToArray();

			if (ratings.Length == 0)
			{
				return View("NoDentists");
			}
			var averageRatingPerDentist = new Dictionary<string, string>();

			foreach (var dentist in dentists)
			{
				var ratingsByDentist = this._ratingRepository
					.All()
					.Where(r => r.PatientId == user.Id
					&& r.DentistId == dentist.Id
					&& r.RatingByDentist > 0)
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
					.Where(r => r.DentistId == dentist.Id)
					.Average(r => r.RatingByDentist);

					averageRatingPerDentist[dentist.FirstName + dentist.LastName] = averageRating.ToString();
				}
				else
				{
					averageRatingPerDentist[dentist.FirstName + dentist.LastName] = "N/A";
				}				
			}

			var patientViewModel = new PatientViewModel
			{
				AverageRatingByDentist = averageRatingPerDentist,
				AverageRating = ratings.Count() > 0 ?
								ratings.Average(r => r.RatingByDentist).ToString() :
								"N/A"
			};

			return View(patientViewModel);
		}
	}
}