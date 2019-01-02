using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentHub.Data.Common;
using DentHub.Data.Models;
using DentHub.Web.Areas.Administration.Models;
using DentHub.Web.Services.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentHub.Web.Areas.Administration.Controllers
{
	[Area("Administration")]
	public class DentistController : Controller
	{
		private readonly IDentistService _dentistService;
		private readonly ISpecialtyService _specialtyService;
		private readonly IClinicService _clinicService;
		private readonly IRatingService _ratingService;

		public DentistController(IDentistService dentistService,
			ISpecialtyService specialtyService, 
			IClinicService clinicService,
			IRatingService ratingService)
		{
			this._dentistService = dentistService;
			this._specialtyService = specialtyService;
			this._clinicService = clinicService;
			this._ratingService = ratingService;
		}

		public IActionResult All()
		{
			var allActiveDentists = this._dentistService
								.GetAllActiveDentists();

			var dentistsViewModel = new DentistsViewModel
			{
				Dentists = allActiveDentists
						.Select(
								d => new DentistViewModel
								{
									Id = d.Id,
									FirstName = d.FirstName,
									LastName = d.LastName,
									ClinicName = this._clinicService
											.GetClinic((int)d.ClinicId).Name,
									Specialty = this._specialtyService
												.GetSpecialtyNameById((int)d.SpecialtyId),
									ImageUrl = d.ImageUrl,
									AverageRating = this._ratingService
												.GetAverageDentistRating(d.Id),
								})
								.ToArray()
			};

			return View(dentistsViewModel);
		}

		public IActionResult Details(string id)
		{
			var dentist = this._dentistService
						.GetDentistById(id);

			var dentistViewModel = new DentistViewModel
			{
				FirstName = dentist.FirstName,
				LastName = dentist.LastName,
				Specialty = this._specialtyService
								.GetSpecialtyNameById((int)dentist.SpecialtyId),
				ClinicName = this._clinicService
								.GetClinic((int)dentist.ClinicId)
								.Name,
				ImageUrl = dentist.ImageUrl
			};

			return View(dentistViewModel);
		}

		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> Deactivate(string id)
		{
			var dentist = this._dentistService
						.GetDentistById(id);

			dentist.IsActive = false;

			this._dentistService.Update(dentist);

			await _dentistService.SaveChangesAsync();

			return RedirectToAction("All");
		}
	}
}