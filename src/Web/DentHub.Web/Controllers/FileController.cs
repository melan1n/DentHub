
using System;
using System.Linq;
using System.Threading.Tasks;
using DentHub.Data;
using DentHub.Data.Common;
using DentHub.Data.Models;
using DentHub.Web.Models.File;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DentHub.Web.Controllers
{
	public class FileController : Controller
	{
		private readonly UserManager<DentHubUser> _userManager;
		private readonly IRepository<DentHubUser> _userRepository;
		private readonly IRepository<PatientFile> _fileRepository;
		private readonly CloudinaryService _cloudinaryService;

		public FileController(UserManager<DentHubUser> userManager,
			IRepository<DentHubUser> userRepository,
			IRepository<PatientFile> fileRepository,
			CloudinaryService cloudinaryService)
		{
			this._userManager = userManager;
			this._userRepository = userRepository;
			this._fileRepository = fileRepository;
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

			var patientFile = new PatientFile
			{
				CreatedById = fileInputModel.CreatedById,
				DateCreated = fileInputModel.DateCreated,
				Description = fileInputModel.Description,
				FileType = (FileType)Enum.Parse(typeof(FileType), fileInputModel.FileType),
				Name = fileInputModel.Name,
				PatientId = fileInputModel.PatientId,
				FileUrl = cloudinaryUris.FirstOrDefault()
			};

			await this._fileRepository.AddAsync(patientFile);
			await this._fileRepository.SaveChangesAsync();

			return RedirectToAction("DentistPatients", "Dentist");
			
		}

		[Authorize(Roles = "Dentist,Patient")]
		public async Task<IActionResult> PatientFiles(string id)
		{
			var user = await this._userManager.GetUserAsync(User);

			DentHubUser patient = null;

			if (User.IsInRole("Patient"))
			{
				patient = this._userRepository
			   .All()
			   .FirstOrDefault(p => p.Id == user.Id);
			}
			else
			{
				patient = this._userRepository
					.All()
					.FirstOrDefault(p => p.Id == id);
			}

			var patientFiles = new FilesViewModel
			{
				PatientId = patient.Id,
				PatientName = $"{patient.FirstName} {patient.LastName}",
				Files = this._fileRepository
				.All()
				.Where(f => f.PatientId == patient.Id)
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
				FileUrl = this._fileRepository
				.All()
				.FirstOrDefault(f => f.Id == id)
				.FileUrl
			};

			return View(fileInputModel);

		}

	}
}