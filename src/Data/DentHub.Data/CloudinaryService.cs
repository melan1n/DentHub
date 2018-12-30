using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

using CloudinaryDotNet;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Linq;
using DentHub.Data.Common;
using System.Threading.Tasks;

namespace DentHub.Data
{
	public class CloudinaryService
	{
		public CloudinaryService()
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();

			var cloudinarySection = configuration.GetSection("Cloudinary");
			
			var clodinaryAccountSettings = new CloudinaryAccountSettings();

			var cloudName = cloudinarySection.GetValue<string>("CloudName");
			var apiKey = cloudinarySection.GetValue<string>("ApiKey");
			var apiSecret = cloudinarySection.GetValue<string>("ApiSecret");

			CloudinaryDotNet.Account account = new CloudinaryDotNet.Account(
						cloudName,
						 apiKey,
						 apiSecret);

			this.Cloudinary = new Cloudinary(account);
		}

		public IConfiguration Configuration { get; }

		public CloudinaryDotNet.Cloudinary Cloudinary { get; set; }

		public async Task<List<string>> UploadFilesAsync(List<IFormFile> files)
		{
			long size = files.Sum(f => f.Length);

			// full path to file in temp location
			var filePath = Path.GetTempFileName();

			var cloudinaryUris = new List<string>();

			foreach (var formFile in files)
			{
				if (formFile.Length > 0)
				{
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await formFile.CopyToAsync(stream);
					}

					// process uploaded files to Cloudinary
					CloudinaryDotNet.Actions.ImageUploadParams uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams()
					{
						File = new FileDescription($@"{filePath}")
					};

					CloudinaryDotNet.Actions.ImageUploadResult uploadResult = this.Cloudinary.Upload(uploadParams);
					cloudinaryUris.Add(uploadResult.Uri.ToString());
				}
			}

			return cloudinaryUris;
		}
	}
}
