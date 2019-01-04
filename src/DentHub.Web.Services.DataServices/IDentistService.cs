using DentHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DentHub.Web.Services.DataServices
{
    public interface IDentistService
    {
		IEnumerable<DentHubUser> GetAllActiveDentists();

		IEnumerable<DentHubUser> GetAllActiveClinicDentists(int clinicId);

		IEnumerable<DentHubUser> GetAllPatientDentists(string patientId);

		DentHubUser GetDentistById(string id);

		string GetDentistFullName(string dentistId);

        DentHubUser GetAppointmentDentist(int appointmentId);

        void Update(DentHubUser dentist);

		Task SaveChangesAsync();
	}
}
