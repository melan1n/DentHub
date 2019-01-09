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
        private readonly IRepository<Rating> _ratingRepository;

        public AppointmentService(IRepository<Appointment> appointmentRepository,
            IRepository<Rating> ratingRepository)
        {
            this._appointmentRepository = appointmentRepository;
            this._ratingRepository = ratingRepository;
        }

		public Appointment GetAppointmentById(int id)
		{
			var appointment = this._appointmentRepository
						.All()
						.FirstOrDefault(a => a.Id == id);

			if (appointment == null)
			{
				throw new ArgumentException("No such appointment exists");
			}

			return appointment; 
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

		public async Task BookAppointmentAsync(int id, DentHubUser user)
		{
			var appointment = GetAppointmentById(id);

			appointment.Patient = user;
			appointment.Status = Status.Booked;

            var ratingRecord = new Rating
            {
                Appointment = appointment,
                DentistId = appointment.DentistID,
                PatientId = appointment.PatientId,
            };

            await this._ratingRepository.AddAsync(ratingRecord);
            await this._ratingRepository.SaveChangesAsync();

            this.Update(appointment);
			await this.SaveChangesAsync();
		}

		public bool DuplicateOfferingExists(DentHubUser user, DateTime timeStart, DateTime timeEnd)
		{
			var offerings = GetAllDentistAppointments(user.Id)
							.Where(a => a.Status.ToString() == "Offering");

			foreach (var offering in offerings)
			{
				if ((timeStart >= offering.TimeStart && timeStart < offering.TimeEnd)
					|| (timeEnd > offering.TimeStart && timeEnd <= offering.TimeEnd))
				{
					return true;
				}
			}

			return false;
		}
		
		public async Task CancelAppointmentAsync(int id)
		{
			var appointment = this.GetAppointmentById(id);

			this.Delete(appointment);
			await this.SaveChangesAsync();
		}

		public Task AddAsync(Appointment appointment)
		{
			return this._appointmentRepository.AddAsync(appointment);
		}

		public void Update(Appointment appointment)
		{
			this._appointmentRepository.Update(appointment);
		}

		public void Delete(Appointment appointment)
		{
			this._appointmentRepository.Delete(appointment);
		}

		public Task SaveChangesAsync()
		{
			return this._appointmentRepository.SaveChangesAsync();
		}

		

		

		

		
	}
}
