using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentHub.Data.Common;
using DentHub.Data.Models;

namespace DentHub.Web.Services.DataServices
{
	public class PatientFileService : IPatientFileService
	{
		private readonly IRepository<PatientFile> _patientFileRepository;

		public PatientFileService(IRepository<PatientFile> patientFileRepository)
		{
			this._patientFileRepository = patientFileRepository;		
		}

		public Task AddAsync(PatientFile file)
		{
			return this._patientFileRepository.AddAsync(file);
		}

		public async Task CreateFileAsync(string name, FileType fileType, string patientId, 
			string fileUri, string description, string createdById, DateTime dateCreated)
		{
			var patientFile = new PatientFile
			{
				CreatedById = createdById,
				DateCreated = dateCreated,
				Description = description,
				FileType = fileType,
				Name = name,
				PatientId = patientId,
				FileUrl = fileUri,
			};

			await this.AddAsync(patientFile);
			await this.SaveChangesAsync(); ;
		}

		public string GetFileUrl(int fileId)
		{
			return this._patientFileRepository
				.All()
				.FirstOrDefault(f => f.Id == fileId)
				.FileUrl;
		}

		public IEnumerable<PatientFile> GetPatientFiles(string patientId)
		{
			return this._patientFileRepository
				.All()
				.Where(f => f.PatientId == patientId);
		}

		public Task SaveChangesAsync()
		{
			return this._patientFileRepository.SaveChangesAsync();
		}
	}
}
