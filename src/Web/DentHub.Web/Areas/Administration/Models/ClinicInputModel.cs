using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentHub.Web.Areas.Administration.Models
{
	public class ClinicInputModel
	{
        public string Name { get; set; }

		public string Street { get; set; }

		public string City { get; set; }

		public string PostalCode { get; set; }

		public string Country { get; set; }

		public string WorkingHours { get; set; }
	}
}
