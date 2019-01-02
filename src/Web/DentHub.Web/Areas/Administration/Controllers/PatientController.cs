using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentHub.Data.Common;
using DentHub.Data.Models;
using DentHub.Web.Areas.Administration.Models;
using DentHub.Web.Services.DataServices;
using Microsoft.AspNetCore.Mvc;

namespace DentHub.Web.Areas.Administration.Controllers
{
	[Area("Administration")]
	public class PatientController : Controller
	{
		private readonly IPatientService _patientService;
		private readonly IRatingService _ratingService;

		public PatientController(IPatientService patientService,
			IRatingService ratingService)
		{
			this._patientService = patientService;
			this._ratingService = ratingService;
		}

		public IActionResult All()
		{
			var allActivePatients = this._patientService
								.GetAllActivePatients();

			var patientsViewModel = new PatientsViewModel
			{
				Patients = allActivePatients
						.Select(
								p => new PatientViewModel
								{
									Id = p.Id,
									FirstName = p.FirstName,
									LastName = p.LastName,
									SSN = p.SSN,
									AverageRating = this._ratingService
												.GetAveragePatientRating(p.Id),
								})
								.ToArray()
			};

			return View(patientsViewModel);
		}

		public IActionResult Details(string id)
		{
			var patient = _patientService
					.GetPatientById(id);

			var patientViewModel = new PatientViewModel
			{
				FirstName = patient.FirstName,
				LastName = patient.LastName,
				SSN = patient.SSN,
				UserName = patient.UserName
			};

			return View(patientViewModel);
		}

		public async Task<IActionResult> Deactivate(string id)
		{
			var patient = this._patientService
				.GetPatientById(id);

			patient.IsActive = false;

			this._patientService.Update(patient);

			await _patientService.SaveChangesAsync();

			return RedirectToAction("All");
		}
	}
}