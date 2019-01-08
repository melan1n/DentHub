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

        [Fact]
        public async Task GetFileUrl_WithValidFileId_ShouldShouldReturnFileUrl()
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

            var result = service.GetFileUrl(1);

            Assert.Equal(patientFile.FileUrl, result);
        }

        [Fact]
        public async Task GetFileUrl_WithInvalidFileId_ShouldShouldThrowException()
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

            Assert.Throws<ArgumentException>(() => service.GetFileUrl(2));
        }

        [Fact]
        public void GetPatientFiles_WithValidPatientIdAndFiles_ShouldReturnPatientFilesCollection()
        {
            var options = new DbContextOptionsBuilder<DentHubContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
               .Options;
            var dbContext = new DentHubContext(options);

            var patient = new DentHubUser
            {
                Id = "1",
                IsActive = true,
                FirstName = "Patient 1",
                LastName = "Test",
            };

            var patient2 = new DentHubUser
            {
                Id = "2",
                IsActive = true,
                FirstName = "Patient 2",
                LastName = "Test2"
            };

            var file1 = new PatientFile
            {
                Id = 1,
                PatientId = "1"
            };

            var file2 = new PatientFile
            {
                Id = 2,
                PatientId = "1"
            };

            var file3 = new PatientFile
            {
                Id = 3,
                PatientId = "2"
            };

            dbContext.DentHubUsers.Add(patient);
            dbContext.DentHubUsers.Add(patient2);
            dbContext.PatientFiles.Add(file1);
            dbContext.PatientFiles.Add(file2);
            dbContext.PatientFiles.Add(file3);
            dbContext.SaveChanges();

            var patuientFilesRepository = new DbRepository<PatientFile>(dbContext);
            var service = new PatientFileService(patuientFilesRepository);
            var result = service.GetPatientFiles("1");
            Assert.Equal(new PatientFile[] { file1, file2 }, result);
        }

        [Fact]
        public void GetPatientFiles_WithInvalidFiles_ShouldReturnEmptyPatientFileCollection()
        {
            var options = new DbContextOptionsBuilder<DentHubContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
               .Options;
            var dbContext = new DentHubContext(options);

            var patient = new DentHubUser
            {
                Id = "1",
                IsActive = true,
                FirstName = "Patient 1",
                LastName = "Test",
            };

            var patient2 = new DentHubUser
            {
                Id = "2",
                IsActive = true,
                FirstName = "Patient 2",
                LastName = "Test2"
            };

            var file1 = new PatientFile
            {
                Id = 1,
                PatientId = "1"
            };

            var file2 = new PatientFile
            {
                Id = 2,
                PatientId = "1"
            };

            var file3 = new PatientFile
            {
                Id = 3,
                PatientId = "2"
            };

            dbContext.DentHubUsers.Add(patient);
            dbContext.DentHubUsers.Add(patient2);
            dbContext.PatientFiles.Add(file1);
            dbContext.PatientFiles.Add(file2);
            dbContext.PatientFiles.Add(file3);
            dbContext.SaveChanges();

            var patuientFilesRepository = new DbRepository<PatientFile>(dbContext);
            var service = new PatientFileService(patuientFilesRepository);
            var result = service.GetPatientFiles("3");
            Assert.Empty(result);
        }
    }
}
