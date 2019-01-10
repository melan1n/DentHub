using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentHub.Web.Areas.Administration.Models
{
	public class ClinicViewModel
	{
		public ClinicViewModel()
		{
			this.Dentists = new HashSet<DentistViewModel>();
		}

		public int Id { get; set; }

		public string Name { get; set; }

		public string Street { get; set; }

		public string City { get; set; }

		public string PostalCode { get; set; }

		public string Country { get; set; }

		[RegularExpression(@"^([ 01]?[0-9]|2[0-3])(:[0-5][0-9])? - ([ 01]?[0-9]|2[0-3])(:[0-5][0-9])?$", ErrorMessage = "Input a string in the format HH:mm - HH:mm")]
		public string WorkingHours { get; set; }

		public string Addresss =>
			this.Street ?? string.Empty +
			(string.IsNullOrEmpty(this.City) ||
				string.IsNullOrEmpty(this.PostalCode) ? string.Empty : this.City + this.PostalCode) +
			this.Country ?? string.Empty;

		public ICollection<DentistViewModel> Dentists { get; set; }

	}
}
