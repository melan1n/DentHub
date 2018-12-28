using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentHub.Data.Common;
using DentHub.Data.Models;
using DentHub.Web.Areas.Administration.Models;
using Microsoft.AspNetCore.Mvc;

namespace DentHub.Web.Areas.Administration.Controllers
{
	[Area("Administration")]
	public class DentistController : Controller
	{
		private readonly IRepository<DentHubUser> _userRepository;
		private readonly IRepository<Specialty> _specialtyRepository;
		private readonly IRepository<Clinic> _clinicRepository;
		private readonly IRepository<Rating> _ratingRepository;

		public DentistController(IRepository<DentHubUser> userRepository,
			IRepository<Specialty> specialtyRepository,
			IRepository<Clinic> clinicRepository,
			IRepository<Rating> ratingRepository)
		{
			this._userRepository = userRepository;
			this._specialtyRepository = specialtyRepository;
			this._clinicRepository = clinicRepository;
			this._ratingRepository = ratingRepository;
		}

		public IActionResult All()
		{
			var dentistsViewModel = new DentistsViewModel();
			GetAllActiveDentists(dentistsViewModel);

			return View(dentistsViewModel);
		}

		private void GetAllActiveDentists(DentistsViewModel dentistsViewModel)
		{
			dentistsViewModel.Dentists = this._userRepository
												.All()
												.Where(u => u.Specialty != null && u.IsActive)

												.Select(
								d => new DentistViewModel
								{
									Id = d.Id,
									FirstName = d.FirstName,
									LastName = d.LastName,
									ClinicName = d.Clinic.Name,
									Specialty = d.Specialty.Name,
									ImageUrl = d.ImageUrl,
									AverageRating = (this._ratingRepository
											.All()
											.Where(r => r.DentistId == d.Id && r.RatingByPatient > 0)
											.Count() > 0 ?
											this._ratingRepository
											.All()
											.Where(r => r.DentistId == d.Id && r.RatingByPatient > 0)
											.Average(r => r.RatingByPatient).ToString() : "N/A"),
								})
									.ToArray();
		}

		public IActionResult Details(string id)
		{
			var dentist = _userRepository
				.All()
				.Where(d => d.Specialty != null)
				.FirstOrDefault(d => d.Id == id);

			var dentistViewModel = new DentistViewModel
			{
				FirstName = dentist.FirstName,
				LastName = dentist.LastName,
				Specialty = this._specialtyRepository
								.All()
								.FirstOrDefault(s => s.Id == dentist.SpecialtyId)
								.Name,
				ClinicName = this._clinicRepository
								.All()
								.FirstOrDefault(c => c.Id == dentist.ClinicId)
								.Name,
				ImageUrl = dentist.ImageUrl
			};

			return View(dentistViewModel);
		}

		public async Task<IActionResult> Deactivate(string id)
		{
			var dentist = this._userRepository
				.All()
				.FirstOrDefault(d => d.Id == id);


			dentist.IsActive = false;

			this._userRepository.Update(dentist);

			await _userRepository.SaveChangesAsync();

			return RedirectToAction("All");
		}
	}
}