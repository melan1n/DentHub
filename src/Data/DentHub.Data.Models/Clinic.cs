using DentHub.Data.Common;
using System.Collections.Generic;

namespace DentHub.Data.Models
{
	public class Clinic : BaseModel<int>
	{
		public Clinic()
		{
			this.Dentists = new HashSet<DentHubUser>();
		}

		public string Name { get; set; }

		public string Street { get; set; }

		public string City { get; set; }

		public string PostalCode { get; set; }

		public string Country { get; set; }

		public string WorkingHours { get; set; }

		public string Addresss =>
			this.Street ?? string.Empty +
			(string.IsNullOrEmpty(this.City) ||
				string.IsNullOrEmpty(this.PostalCode) ? string.Empty : this.City + this.PostalCode) +
			this.Country ?? string.Empty;

		//public bool IsGeoCoded { get; set; }

		//public decimal? Longitude { get; set; }

		//public decimal? Latitude { get; set; }

		public ICollection<DentHubUser> Dentists { get; set; }

	}
}