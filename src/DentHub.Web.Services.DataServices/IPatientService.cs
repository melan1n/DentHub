using DentHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DentHub.Web.Services.DataServices
{
	public interface IPatientService
	{
		IEnumerable<DentHubUser> GetAllActivePatients();

		DentHubUser GetPatientById(string id);

		string GetPatientFullName(string patientId);

		IEnumerable<DentHubUser> GetAllDentistPatients(string dentistId);

        void Update(DentHubUser patient);

		Task SaveChangesAsync();
	}
}
