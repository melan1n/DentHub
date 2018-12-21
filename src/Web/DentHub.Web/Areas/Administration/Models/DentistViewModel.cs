namespace DentHub.Web.Areas.Administration.Models
{
	public class DentistViewModel
	{
		public string Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		// Dentist-specific properties
		public string Specialty { get; set; }

		public string ImageUrl { get; set; }

	}
}