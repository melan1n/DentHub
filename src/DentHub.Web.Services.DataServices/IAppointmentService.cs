using DentHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DentHub.Web.Services.DataServices
{
    public interface IAppointmentService
    {
		Appointment GetAppointmentById(int id);

		IEnumerable<Appointment> GetAllDentistAppointments(string dentistId);

		IEnumerable<Appointment> GetAllPatientAppointments(string patientId);

		bool DuplicateOfferingExists(DentHubUser user, DateTime timeStart, DateTime timeEnd);

		Task CreateAppointment(DentHubUser user, DateTime timeStart, DateTime timeEnd);

		Task BookAppointment(int id, DentHubUser user);

		void CancelAppointment(int id);

		Task AddAsync(Appointment appointment);

		void Update(Appointment appointment);

		void Delete(Appointment appointment);

		Task SaveChangesAsync();
	}
}
