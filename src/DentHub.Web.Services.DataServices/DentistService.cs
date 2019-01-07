using DentHub.Data.Common;
using DentHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentHub.Web.Services.DataServices
{
    public class DentistService : IDentistService
    {
        private readonly IRepository<DentHubUser> _userRepository;
        private readonly IAppointmentService _appointmentService;
        
        public DentistService(IRepository<DentHubUser> userRepository,
			IAppointmentService appointmentService)
        {
            this._userRepository = userRepository;
			this._appointmentService = appointmentService;
        }

		public IEnumerable<DentHubUser> GetAllActiveDentists()
		{
			return this._userRepository
						.All()
						.Where(u => u.Specialty != null && u.IsActive)
						.ToArray();
		}

		public IEnumerable<DentHubUser> GetAllActiveClinicDentists(int clinicId)
        {
            return this._userRepository
                     .All()
                     .Where(d => d.IsActive && d.ClinicId == clinicId)
                     .ToArray();
        }

		public DentHubUser GetDentistById(string id)
		{
			var dentist = this._userRepository
				.All()
				.Where(d => d.Specialty != null)
				.FirstOrDefault(d => d.Id == id);

			if (dentist == null)
			{
				throw new ArgumentException("No such dentist exists.");
			}

			return dentist;
		}

		public Task SaveChangesAsync()
		{
			return this._userRepository.SaveChangesAsync();
		}

		public void Update(DentHubUser dentist)
		{
			this._userRepository.Update(dentist);
		}

		public IEnumerable<DentHubUser> GetAllPatientDentists(string patientId)
		{
			var patientDentistIds = this._appointmentService
				.GetAllPatientAppointments(patientId)
				.Select(a => a.DentistID)
				.Distinct()
				.ToArray();

			var dentists = this._userRepository
				.All()
				.Where(d => patientDentistIds.Contains(d.Id))
				.ToArray();

			return dentists;
		}

		public string GetDentistFullName(string dentistId)
		{
			var dentist = this._userRepository
				.All()
				.FirstOrDefault(d => d.Id == dentistId);

			return $"{dentist.FirstName} {dentist.LastName}";
		}

        public DentHubUser GetAppointmentDentist(int appointmentId)
        {
			var dentist = this._userRepository
						.All()
						.FirstOrDefault(d => d.Id == this._appointmentService
												.GetAppointmentById(appointmentId)
												.DentistID);
			if (dentist == null)
			{
				throw new NullReferenceException("Dentist not found");
			}

			return dentist;
		}
    }
}
