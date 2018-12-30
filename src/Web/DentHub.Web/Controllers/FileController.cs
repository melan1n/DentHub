
using System;
using System.Linq;
using DentHub.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentHub.Web.Controllers
{
	public class FileController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		[Authorize(Roles="Dentist")]
		public IActionResult AddFile()
		{
			this.ViewData["FileTypeList"] = Enum.GetValues(typeof(FileType));
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "Dentist")]
		public IActionResult AddFile(string id)
		{
			return View();
		}
	}
}