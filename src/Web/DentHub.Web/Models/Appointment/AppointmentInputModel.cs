﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentHub.Web.Models.Appointment
{
	public class AppointmentInputModel
	{
		[Required]
		[Display(Name = "Appointment Start")]
		public DateTime TimeStart { get; set; }

		[Required]
		[Display(Name = "Appointmetnt End")]
		public DateTime TimeEnd { get; set; }

		[Range(0.25, 8.00)] 
		public double Duration => (this.TimeEnd - this.TimeStart).Hours;
	}
}