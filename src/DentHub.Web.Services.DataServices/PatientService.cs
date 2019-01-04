using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentHub.Data.Common;
using DentHub.Data.Models;

namespace DentHub.Web.Services.DataServices
{
	public class PatientService : IPatientService
	{
		private IRepository<DentHubUser> _userRepository;
		private IAppointmentService _appointmentService;

		public PatientService(IRepository<DentHubUser> userRepository,
            IAppointmentService appointmentService)
		{
			this._userRepository = userRepository;
            this._appointmentService = appointmentService;
        }

		public IEnumerable<DentHubUser> GetAllActivePatients()
		{
			return this._userRepository
						.All()
						.Where(u => u.SSN != null && u.IsActive);
		}

		public DentHubUser GetPatientById(string id)
		{
			return  this._userRepository
				.All()
				.Where(p => p.SSN != null)
				.FirstOrDefault(p => p.Id == id);
		}

        public IEnumerable<DentHubUser> GetAllDentistPatients(string dentistId)
        {
            var dentistPatientIds = this._appointmentService
                .GetAllDentistAppointments(dentistId)
                .Select(a => a.PatientId)
                .Distinct()
                .ToArray();

            var patients = this._userRepository
                .All()
                .Where(p => dentistPatientIds.Contains(p.Id))
                .ToArray();

            return patients;
        }

		public string GetPatientFullName(string patientId)
		{

            if (patientId != null)
            {
                var patient = this._userRepository
                .All()
                .FirstOrDefault(d => d.Id == patientId);

                return $"{patient.FirstName} {patient.LastName}";
            }

            return "Not Appointed";
        }

		public Task SaveChangesAsync()
		{
			return this._userRepository.SaveChangesAsync();
		}

		public void Update(DentHubUser patient)
		{
			this._userRepository.Update(patient); 
		}

		public DentHubUser GetAppointmentPatient(int appointmentId)
		{
			return this._userRepository
						.All()
						.FirstOrDefault(d => d.Id == this._appointmentService
												.GetAppointmentById(appointmentId)
												.PatientId);
		}
	}
}
