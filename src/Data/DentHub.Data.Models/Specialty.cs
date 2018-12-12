using DentHub.Data.Common;

namespace DentHub.Data.Models
{
	public class Specialty : BaseModel<int>
	{
		public string Name { get; set; }

		public string Description { get; set; }
	}
}