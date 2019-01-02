using DentHub.Data.Common;
using DentHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
	}
}
