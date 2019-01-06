
using System;
using System.Linq;
using System.Threading.Tasks;
using DentHub.Data;
using DentHub.Data.Common;
using DentHub.Data.Models;
using DentHub.Web.Models.File;
using DentHub.Web.Services.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DentHub.Web.Controllers
{
	public class FileController : Controller
	{
		private readonly UserManager<DentHubUser> _userManager;
		private readonly IPatientService _patientService;
		private readonly IPatientFileService _patientFileService;
		private readonly CloudinaryService _cloudinaryService;

		public FileController(UserManager<DentHubUser> userManager,
			IPatientService patientService,
			IPatientFileService patientFileService,
			CloudinaryService cloudinaryService)
		{
			this._userManager = userManager;
			this._patientService = patientService;
			this._patientFileService = patientFileService;
			this._cloudinaryService = cloudinaryService;
		}

		public IActionResult Index()
		{
			return View();
		}

		[Authorize(Roles="Dentist")]
		public IActionResult AddFile(string id)
		{
			this.ViewData["FileTypeList"] = Enum.GetValues(typeof(FileType));

			var user = this._userManager.GetUserAsync(User).GetAwaiter().GetResult();

			var fileInputModel = new FileInputModel
			{
				CreatedById = user.Id,
				DateCreated = DateTime.Now,
				PatientId = id
			};

			return View(fileInputModel);
		}

		[HttpPost]
		[Authorize(Roles = "Dentist")]
		public async Task<IActionResult> AddFile(FileInputModel fileInputModel)
		{
			var files = HttpContext.Request.Form.Files.ToList();
			var cloudinaryUris = await this._cloudinaryService.UploadFilesAsync(files);

			var createdById = fileInputModel.CreatedById;
			var dateCreated = fileInputModel.DateCreated;
			var description = fileInputModel.Description;
			var fileType = (FileType)Enum.Parse(typeof(FileType), fileInputModel.FileType);
			var name = fileInputModel.Name;
			var patientId = fileInputModel.PatientId;
			var fileUri = cloudinaryUris.FirstOrDefault();

			await this._patientFileService
				.CreateFileAsync(name, fileType, patientId, fileUri, description, createdById, dateCreated);

			return RedirectToAction("DentistPatients", "Dentist");
		}

		[Authorize(Roles = "Dentist,Patient")]
		public async Task<IActionResult> PatientFiles(string id)
		{
			var user = await this._userManager.GetUserAsync(User);

			DentHubUser patient = null;

			if (User.IsInRole("Patient"))
			{
				patient = this._patientService
					.GetPatientById(user.Id);
			}
			else
			{
				patient = this._patientService
					.GetPatientById(id);
			}

			var patientFiles = new FilesViewModel
			{
				PatientId = patient.Id,
				PatientName = this._patientService.GetPatientFullName(patient.Id),
				Files = this._patientFileService
						.GetPatientFiles(patient.Id)
						.Select(f => new FileInputModel
						{
							Id = f.Id,
							DateCreated = f.DateCreated,
							FileType = f.FileType.ToString(),
							Description = f.Description,
							Name = f.Name,
							PatientId = patient.Id,

						}).ToArray()
			};

			return View("PatientFiles", patientFiles);
		}

		[Authorize(Roles = "Patient,Dentist")]
		public IActionResult Details(int id)
		{
			var fileInputModel = new FileInputModel
			{
				FileUrl = this._patientFileService
					.GetFileUrl(id),
			};

			return View(fileInputModel);

		}

	}
}