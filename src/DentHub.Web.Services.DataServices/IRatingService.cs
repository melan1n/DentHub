using DentHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DentHub.Web.Services.DataServices
{
	public interface IRatingService
	{
		string GetAverageDentistRating(string dentistId);

		string GetAveragePatientRating(string patientId);

        IEnumerable<Rating> GetAllRatingsForDentist(string dentistId);

        IEnumerable<Rating> GetAllRatingsForDentistByPatient(string dentistId, string patientId);

		IEnumerable<Rating> GetAllRatingsForPatient(string patientId);

		IEnumerable<Rating> GetAllRatingsForPatientByDentist(string patientId, string dentistId);

        Rating GetRatingForAppointment(int appointmentId);

        Task AddAsync(Rating rating);

        Task SaveChangesAsync();

        void Update(Rating rating);
    }
}
