using DentHub.Data.Common;
using DentHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentHub.Web.Services.DataServices
{
	public class RatingService : IRatingService
	{
		private readonly IRepository<Rating> _ratingRepository;
	
		public RatingService(IRepository<Rating> ratingRepository)
		{
			this._ratingRepository = ratingRepository;
		}

		public string GetAverageDentistRating(string dentistId)
		{
			bool dentistIsRated = this._ratingRepository
					.All()
					.Where(r => r.DentistId == dentistId && r.RatingByPatient > 0)
					.Count() > 0;

			if (dentistIsRated)
			{
				return this._ratingRepository
					.All()
					.Where(r => r.DentistId == dentistId && r.RatingByPatient > 0)
					.Average(r => r.RatingByPatient).ToString();
			}

			return "Not Rated";
		}

		public string GetAveragePatientRating(string patientId)
		{
			bool patientIsRated = this._ratingRepository
					.All()
					.Where(r => r.PatientId == patientId && r.RatingByDentist > 0)
					.Count() > 0;

			if (patientIsRated)
			{
				return this._ratingRepository
					.All()
					.Where(r => r.PatientId == patientId && r.RatingByDentist > 0)
					.Average(r => r.RatingByDentist).ToString();
			}

			return "Not Rated";
		}

        public IEnumerable<Rating> GetAllRatingsForDentist(string dentistId)
        {
            return this._ratingRepository
                    .All()
                    .Where(r => r.DentistId == dentistId
                    && r.RatingByPatient > 0);
        }

        public IEnumerable<Rating> GetAllRatingsForDentistByPatient(string dentistId, string patientId)
        {
            return this._ratingRepository
                   .All()
                   .Where(r => r.DentistId == dentistId
                   && r.PatientId == patientId
                   && r.RatingByPatient > 0);
        }

		public IEnumerable<Rating> GetAllRatingsForPatient(string patientId)
		{
			return this._ratingRepository
					.All()
					.Where(r => r.PatientId == patientId
					&& r.RatingByDentist > 0);
		}

		public IEnumerable<Rating> GetAllRatingsForPatientByDentist(string patientId, string dentistId)
		{
			return this._ratingRepository
				   .All()
				   .Where(r => r.PatientId == patientId
				   && r.DentistId == dentistId
				   && r.RatingByDentist > 0);
		}

        public Task AddAsync(Rating rating)
        {
            return this._ratingRepository.AddAsync(rating);
        }

        public Task SaveChangesAsync()
        {
            return this._ratingRepository.SaveChangesAsync();
        }

        public void Update(Rating rating)
        {
            this._ratingRepository.Update(rating);
        }

        public Rating GetRatingForAppointment(int appointmentId)
        {
            var ratingRecord = this._ratingRepository
                .All()
                .FirstOrDefault(r => r.Appointment.Id == appointmentId);

            if (ratingRecord == null)
            {
                throw new InvalidOperationException("No rating found");
            }
            return ratingRecord;
		}
    }
}
