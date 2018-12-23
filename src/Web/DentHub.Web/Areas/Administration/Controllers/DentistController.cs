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
		private readonly IRepository<DentHubUser> _dentistRepository;

		public DentistController(IRepository<DentHubUser> dentistRepository)
		{
			this._dentistRepository = dentistRepository;
		}

		public IActionResult All()
		{
			var dentistsViewModel = new DentistsViewModel();
			GetAllDentists(dentistsViewModel);

			return View(dentistsViewModel);
		}

		private void GetAllDentists(DentistsViewModel dentistsViewModel)
		{
			dentistsViewModel.Dentists = this._dentistRepository
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
	}
}