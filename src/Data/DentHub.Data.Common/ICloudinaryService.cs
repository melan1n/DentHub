using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DentHub.Data.Common
{
	public interface ICloudinaryService
	{
		IConfiguration Configuration { get; }

		CloudinaryDotNet.Cloudinary Cloudinary { get; set; }

		IEnumerable<string> UploadFiles(IEnumerable<IFormFile> files);
	}
}
