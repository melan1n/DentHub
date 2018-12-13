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

namespace DentHub.Web.Controllers
{
	public class HomeController : Controller
	{
		private IRepository<Specialty> specialtyRepository;

		public HomeController(IRepository<Specialty> specialtyRepository)
		{
			this.specialtyRepository = specialtyRepository;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult About()
		{
			var specialties = this.specialtyRepository.All()
				.OrderBy(x => x.Name)
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

		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";

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
