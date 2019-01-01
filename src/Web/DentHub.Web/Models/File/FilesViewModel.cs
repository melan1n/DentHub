using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentHub.Web.Models.File
{
	public class FilesViewModel
	{
		public FilesViewModel()
		{
			this.Files = new HashSet<FileInputModel>();
		}

		public int Id { get; set; }

		public string PatientName { get; set; }

		public string PatientId { get; set; }

		public IEnumerable<FileInputModel> Files;
	}
}
