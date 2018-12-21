using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
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

		[Authorize(Roles = "Administrator,Dentist")]
		public IActionResult All()
		{
			//var xx = HttpContext.User;

			//var userIdentity = (ClaimsIdentity)User.Identity;
			//var claims = userIdentity.Claims;
			//var roleClaimType = userIdentity.RoleClaimType;
			//var roles = claims.Where(c => c.Type == ClaimTypes.Role).ToList();

			//var x = User.IsInRole("Dentist");

			var clinicsViewModel = new ClinicsViewModel();

			clinicsViewModel.Clinics = this._clinicRepository
									.All()
									.Select(
					c => new ClinicViewModel
					{
						Id = c.Id,
						City = c.City,
						Country = c.Country,
						Name = c.Name,
						PostalCode = c.PostalCode,
						Street = c.Street,
						WorkingHours = c.WorkingHours,
						Dentists = this._dentistRepository
									.All()
									.Where(d => d.ClinicId == c.Id)
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

		[Authorize(Roles = "Administrator")]
		public IActionResult Create()
		{
			return View();
		}

		[Authorize(Roles = "Administrator")]
		[HttpPost]
		public async Task<IActionResult> Create(ClinicInputModel model)
		//public async Task<IActionResult> Create([FromBody] string content)
		{
			if (ModelState.IsValid)
			{
				var newClinic = new Clinic
				{
					Name = model.Name,
					Street = model.Street,
					City = model.City,
					PostalCode = model.PostalCode,
					Country = model.Country,
					WorkingHours = model.WorkingHours,
				};

				var result = _clinicRepository.AddAsync(newClinic);
				await result;

				await _clinicRepository.SaveChangesAsync();
			}

			return RedirectToAction("All");
		}

		[Authorize(Roles = "Administrator")]
		public IActionResult Delete(int id)
		{
			var clinic = _clinicRepository
						.All()
						.FirstOrDefault(c => c.Id == id);

			this._clinicRepository.Delete(clinic);
			_clinicRepository.SaveChangesAsync().GetAwaiter().GetResult();

			return RedirectToAction("All");
		}
	}
}
