﻿using System;
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
	public class PatientController : Controller
    {
		private readonly IRepository<DentHubUser> _userRepository;
		private readonly IRepository<Rating> _ratingRepository;

		public PatientController(IRepository<DentHubUser> userRepository,
			IRepository<Rating> ratingRepository)
		{
			this._userRepository = userRepository;
			this._ratingRepository = ratingRepository;
		}

		public IActionResult All ()
        {
			var patientsViewModel = new PatientsViewModel();
			GetAllPatients(patientsViewModel);

			return View(patientsViewModel);
		}

		private void GetAllPatients(PatientsViewModel patientsViewModel)
		{
			patientsViewModel.Patients = this._userRepository
												.All()
												.Where(u => u.SSN != null && u.IsActive)
												.Select(
								p => new PatientViewModel
								{
									Id = p.Id,
									FirstName = p.FirstName,
									LastName = p.LastName,
									SSN = p.SSN,
									AverageRating = (this._ratingRepository
											.All()
											.Where(r => r.PatientId == p.Id && r.RatingByDentist > 0)
											.Count() > 0 ?
											this._ratingRepository
											.All()
											.Where(r => r.PatientId == p.Id && r.RatingByDentist > 0)
											.Average(r => r.RatingByDentist).ToString() : "N/A"),
								})
									.ToArray();
		}

		public IActionResult Details(string id)
		{
			var patient = _userRepository
				.All()
				.Where(p => p.SSN != null)
				.FirstOrDefault(p => p.Id == id);

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
			var patient = this._userRepository
				.All()
				.FirstOrDefault(p => p.Id == id);


			patient.IsActive = false;

			this._userRepository.Update(patient);

			await _userRepository.SaveChangesAsync();

			return RedirectToAction("All");
		}
	}
}