using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentHub.Web.Models.Rating
{
	public class RatingInputModel
	{
		
		public int ForAppointmentId { get; set; }

		public string DentistId { get; set; }

		public string PatientId { get; set; }

		public int RatingByDentist { get; set; }

		public int RatingByPatient { get; set; }

		[Required]
		[Display(Name = "Provide your rating for this appointment")]
		[Range(1, 10)]
		public int Rating { get; set; }
	}
}
