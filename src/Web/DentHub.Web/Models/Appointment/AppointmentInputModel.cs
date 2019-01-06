using System;
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
		[DataType(DataType.DateTime)]
		public DateTime TimeStart { get; set; }

		[Required]
		[Display(Name = "Appointmetnt End")]
		[DataType(DataType.DateTime)]
		public DateTime TimeEnd { get; set; }

		[Range(15, 8 * 60)]
		public int Duration => (int)(this.TimeEnd).Subtract(this.TimeStart).TotalMinutes;

		[Range(60, 365*24*60)]
		public int MinutesFromNow => (int)(this.TimeEnd).Subtract(DateTime.Now).TotalMinutes;
	}
}
