using DentHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DentHub.Web.Services.DataServices
{
	public interface IRatingService
	{
		string GetAverageDentistRating(string dentistId);

		string GetAveragePatientRating(string patientId);

        IEnumerable<Rating> GetAllRatingsForDentist(string dentistId);

        IEnumerable<Rating> GetAllRatingsForDentistByPatient(string dentistId, string patientId);
    }
}
