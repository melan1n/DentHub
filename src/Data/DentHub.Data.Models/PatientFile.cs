using DentHub.Data.Common;
using System;

namespace DentHub.Data.Models
{
	public class PatientFile : BaseModel<int>
	{
		public string Name { get; set; }

		public FileType FileType { get; set; }

		public string Description { get; set; }

		public virtual DentHubUser Patient { get; set; }
		public string PatientId { get; set; }

		public virtual DentHubUser CreatedBy { get; set; }
		public string CreatedById { get; set; }

		public DateTime DateCreated { get; set; }

		public string FileUrl { get; set; }
	}
}