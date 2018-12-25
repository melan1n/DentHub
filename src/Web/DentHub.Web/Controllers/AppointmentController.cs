using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentHub.Data.Common;
using DentHub.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentHub.Web.Controllers
{
    public class AppointmentController : Controller
    {
		private readonly IRepository<Clinic> _clinicRepository;
		private readonly IRepository<DentHubUser> _userRepository;

		public AppointmentController(IRepository<Clinic> clinicRepository,
			IRepository<DentHubUser> userRepository)
		{
			this._clinicRepository = clinicRepository;
			this._userRepository = userRepository;
		}

		[Authorize(Roles = "Dentist")]
		public IActionResult Index()
        {
            return View();
        }

		[Authorize(Roles = "Dentist")]
		public IActionResult CreateOffering(DateTime time, Clinic clinic)
		{
			this.ViewData["ClinicList"] = _clinicRepository.All().ToList();
			return View();
		}

	}
}