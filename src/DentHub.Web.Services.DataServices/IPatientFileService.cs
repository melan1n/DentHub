using DentHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DentHub.Web.Services.DataServices
{
	public interface IPatientFileService
	{
		IEnumerable<PatientFile> GetPatientFiles(string patientId);

		string GetFileUrl(int fileId);

		Task CreateFileAsync(string name, FileType fileType, string patientId, 
			string fileUri, string description, string createdById, DateTime dateCreated);

		Task AddAsync(PatientFile file);

		Task SaveChangesAsync();
	}
}
