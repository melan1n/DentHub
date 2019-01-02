﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DentHub.Web.Services.DataServices
{
	public interface IRatingService
	{
		string GetAverageDentistRating(string dentistId);

		string GetAveragePatientRating(string patientId);
	}
}
