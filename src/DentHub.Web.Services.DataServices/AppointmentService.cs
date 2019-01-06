using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentHub.Data.Common;
using DentHub.Data.Models;

namespace DentHub.Web.Services.DataServices
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository<Appointment> _appointmentRepository;

        public AppointmentService(IRepository<Appointment> appointmentRepository)
        {
            this._appointmentRepository = appointmentRepository;
        }

		public Appointment GetAppointmentById(int id)
		{
			return this._appointmentRepository
						.All()
						.FirstOrDefault(a => a.Id == id);
		}

		public Task AddAsync(Appointment appointment)
		{
			return this._appointmentRepository.AddAsync(appointment);
		}

		public IEnumerable<Appointment> GetAllDentistAppointments(string dentistId)
        {
            return this._appointmentRepository
                .All()
                .Where(a => a.DentistID == dentistId)
                .ToArray();
        }

		public IEnumerable<Appointment> GetAllPatientAppointments(string patientId)
		{
			return this._appointmentRepository
			   .All()
			   .Where(a => a.PatientId == patientId)
			   .ToArray();
		}

		public Task SaveChangesAsync()
		{
			return this._appointmentRepository.SaveChangesAsync();
		}

		public void Update(Appointment appointment)
		{
			this._appointmentRepository.Update(appointment);
		}

		public void Delete(Appointment appointment)
		{
			this._appointmentRepository.Delete(appointment);
		}

		public async Task BookAppointment(int id, DentHubUser user)
		{
			var appointment = GetAppointmentById(id);

			if (appointment != null)
			{
				appointment.Patient = user;
				appointment.Status = Status.Booked;
			}

			this.Update(appointment);
			await this.SaveChangesAsync();
		}

		public async void CancelAppointment(int id)
		{
			var appointment = this.GetAppointmentById(id);

			this.Delete(appointment);
			await this.SaveChangesAsync();
		}

		public bool DuplicateOfferingExists(DentHubUser user, DateTime timeStart, DateTime timeEnd)
		{
			var offerings = GetAllDentistAppointments(user.Id)
							.Where(a => a.Status.ToString() == "Offering");

			foreach (var offering in offerings)
			{
				if ((timeStart > offering.TimeStart && timeStart < offering.TimeEnd)
					|| (timeEnd > offering.TimeStart && timeEnd < offering.TimeEnd))
				{
					return true;
				}
			}

			return false;
		}

		public async Task CreateAppointment(DentHubUser user, DateTime timeStart, DateTime timeEnd)
		{
			var appointment = new Appointment
			{
				ClinicId = (int)user.ClinicId,
				Dentist = user,
				DentistID = user.Id,
				Status = Status.Offering,
				TimeStart = timeStart,
				TimeEnd = timeEnd,
			};

			await this.AddAsync(appointment);
			await this.SaveChangesAsync();
		}
	}
}
