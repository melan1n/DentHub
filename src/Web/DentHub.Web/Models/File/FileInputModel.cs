using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentHub.Web.Models.File
{
	public class FileInputModel
	{
		[Required]
		[Display(Name = "Name")]
		public string Name { get; set; }

		[Required]
		[Display(Name = "File Type")]
		public string FileType { get; set; }

		[Display(Name = "Description")]
		public string Description { get; set; }

		[Display(Name = "Patient Id")]
		public string PatientId { get; set; }

		public string CreatedById { get; set; }

		public DateTime DateCreated { get; set; }

		[Display(Name = "File Url")]
		public string FileUrl { get; set; }

		[Required]
		[Display(Name = "File")]
		public string File { get; set; }
	}
}
