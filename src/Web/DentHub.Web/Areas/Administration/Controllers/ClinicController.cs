using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentHub.Data;
using DentHub.Data.Common;
using DentHub.Data.Models;
using DentHub.Web.Areas.Administration.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentHub.Web.Areas.Administration.Controllers
{
	[Area("Administration")]
	public class ClinicController : Controller
	{
		private readonly IRepository<Clinic> _clinicRepository;
		private readonly IRepository<DentHubUser> _dentistRepository;


		public ClinicController(IRepository<Clinic> clinicRepository,
			IRepository<DentHubUser> dentistRepository)
		{
			this._clinicRepository = clinicRepository;
			this._dentistRepository = dentistRepository;
		}

		[Authorize(Roles = "Administrator,Patient")]
		public IActionResult All()
		{
			var clinicsViewModel = new ClinicsViewModel();

			clinicsViewModel.Clinics = this._clinicRepository
									.All()
									.Select(
					c => new ClinicViewModel
					{
						City = c.City,
						Country = c.Country,
						Name = c.Name,
						PostalCode = c.PostalCode,
						Street = c.Street,
						WorkingHours = c.WorkingHours,
						Dentists = this._dentistRepository
									.All()
									.Select(
						d => new DentistViewModel
						{
							FirstName = d.FirstName,
							LastName = d.LastName,
							Specialty = d.Specialty.Name,
							ImageUrl = d.ImageUrl,
						}).ToList(),
					})
						.ToArray();

			return View(clinicsViewModel);
		}
	}
}
