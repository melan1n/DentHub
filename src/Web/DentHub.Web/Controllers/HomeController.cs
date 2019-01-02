using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DentHub.Web.Models;
using DentHub.Data.Common;
using DentHub.Data.Models;
using DentHub.Web.Models.About;
using DentHub.Web.Services.DataServices;

namespace DentHub.Web.Controllers
{
	public class HomeController : Controller
	{
		private ISpecialtyService _specialtyService;

		public HomeController(ISpecialtyService specialtyService)
		{
			this._specialtyService = specialtyService;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Specialties()
		{
			var specialties = this._specialtyService
				.GetAll()
				.Select(x => new SpecialtyViewModel
				{
					Name = x.Name,
					Description = x.Description
				});

			var viewModel = new AboutViewModel
			{
				Specialties = specialties
			};

			return View(viewModel);
		}

		public IActionResult Guidelines()
		{
			return View();
		}

		public IActionResult ScheduleAppointment()
		{
			return View();
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Our address is:";

			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
