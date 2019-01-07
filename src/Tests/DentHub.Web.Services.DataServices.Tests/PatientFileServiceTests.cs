using DentHub.Data;
using DentHub.Data.Models;
using DentHub.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DentHub.Web.Services.DataServices.Tests
{
	public class PatientFileServiceTests
	{
		[Fact]
		public async Task CreateFileAsync_WithValidPrerequisites_ShouldCreateFile()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);

			var patientFile = new PatientFile
			{
				CreatedById = "1",
				DateCreated = new DateTime(2019, 1, 1, 10, 30, 0),
				Description = "description",
				FileType = FileType.Assesment,
				Name = "Name",
				PatientId = "1",
				FileUrl = "examplefile.com/pic/1564",
			};

			var createdById = "1";
			var dateCreated = new DateTime(2019, 1, 1, 10, 30, 0);
			var description = "description";
			var fileType = FileType.Assesment;
			var name = "Name";
			var patientId = "1";
			var fileUrl = "examplefile.com/pic/1564";

			dbContext.PatientFiles.Add(patientFile);
			dbContext.SaveChanges();

			var patientFileRepository = new DbRepository<PatientFile>(dbContext);
			var service = new PatientFileService(patientFileRepository);
			await service.CreateFileAsync(name,
				fileType, patientId, fileUrl,
				description, createdById, dateCreated);

			var result = dbContext.PatientFiles
				.FirstOrDefaultAsync(f => f.FileUrl == fileUrl)
				.GetAwaiter()
				.GetResult();

			Assert.Equal(patientFile, result);
		}
	}
}
