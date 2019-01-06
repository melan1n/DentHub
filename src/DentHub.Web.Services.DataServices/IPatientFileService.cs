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

		Task AddAsync(PatientFile file);

		Task SaveChangesAsync();
	}
}
