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

		public DentistController(IRepository<DentHubUser> userRepository,
			IRepository<Specialty> specialtyRepository,
			IRepository<Clinic> clinicRepository)
		{
			this._userRepository = userRepository;
			this._specialtyRepository = specialtyRepository;
			this._clinicRepository = clinicRepository;
		}

		public IActionResult All()
		{
			var dentistsViewModel = new DentistsViewModel();
			GetAllDentists(dentistsViewModel);

			return View(dentistsViewModel);
		}

		private void GetAllDentists(DentistsViewModel dentistsViewModel)
		{
			dentistsViewModel.Dentists = this._userRepository
												.All()
												.Where(u => u.Specialty != null)

												.Select(
								d => new DentistViewModel
								{
									Id = d.Id,
									FirstName = d.FirstName,
									LastName = d.LastName,
									ClinicName = d.Clinic.Name,
									Specialty = d.Specialty.Name,
									ImageUrl = d.ImageUrl
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
	}
}