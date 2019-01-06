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
	public class ClinicServiceTests
	{
		[Fact]
		public void GetAllActive_WithValidClinics_ShouldReturnClinicsCollection()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);

			var clinic = new Clinic
			{
				Id = 1,
				IsActive = true,
				Name = "Clinic 1",
			};
			var dentist = new DentHubUser
			{
				Id = "1",
				IsActive = true,
				ClinicId = 1
			};
			
			var clinic2 = new Clinic
			{
				Id = 2,
				IsActive = true,
				Name = "Clinic 2",
			};
			var dentist2 = new DentHubUser
			{
				Id = "2",
				IsActive = true,
				ClinicId = 2
			};
			var dentist3 = new DentHubUser
			{
				Id = "3",
				IsActive = false,
				ClinicId = 2
			};

			var clinic3 = new Clinic
			{
				Id = 3,
				IsActive = false,
				Name = "Clinic 3",
			};
			var dentist4 = new DentHubUser
			{
				Id = "4",
				IsActive = false,
				ClinicId = 3
			};
			dbContext.Clinics.Add(clinic);
			dbContext.Clinics.Add(clinic2);
			dbContext.Clinics.Add(clinic3);
			dbContext.DentHubUsers.Add(dentist);
			dbContext.DentHubUsers.Add(dentist2);
			dbContext.DentHubUsers.Add(dentist3);
			dbContext.DentHubUsers.Add(dentist4);
			dbContext.SaveChanges();

			var repository = new DbRepository<Clinic>(dbContext);
			var service = new ClinicService(repository);
			var result = service.GetAllActive();
			Assert.Equal(new Clinic[] { clinic, clinic2 }, result);
		}

		[Fact]
		public void GetAllActive_WithInvalidClinics_ShouldReturnEmptyClinicsCollection()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);

			var clinic = new Clinic
			{
				Id = 10,
				IsActive = true,
				Name = "Clinic 1",
			};
			var dentist = new DentHubUser
			{
				Id = "1",
				IsActive = false,
				ClinicId = 1
			};

			var clinic2 = new Clinic
			{
				Id = 2,
				IsActive = false,
				Name = "Clinic 2",
			};
			var dentist2 = new DentHubUser
			{
				Id = "2",
				IsActive = true,
				ClinicId = 2
			};

			dbContext.Clinics.Add(clinic);
			dbContext.Clinics.Add(clinic2);
			dbContext.DentHubUsers.Add(dentist);
			dbContext.DentHubUsers.Add(dentist2);
			dbContext.SaveChanges();

			var repository = new DbRepository<Clinic>(dbContext);
			var service = new ClinicService(repository);
			var result = service.GetAllActive();
			Assert.Empty(result);
		}

		[Fact]
		public void GetClinicById_WithValidId_ShouldReturnClinic()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);
			var clinic = new Clinic
			{
				Id = 10,
				Name = "Clinic 10"
			};
			var clinic2 = new Clinic
			{
				Id = 11,
				Name = "clinic 11"
			};
			dbContext.Clinics.Add(clinic);
			dbContext.Clinics.Add(clinic2);
			dbContext.SaveChanges();

			var repository = new DbRepository<Clinic>(dbContext);
			var service = new ClinicService(repository);
			var result = service.GetClinicById(11);
			Assert.Same(clinic2, result);
		}

		[Fact]
		public async Task GetClinicById_WithInvalidId_ShouldReturnException()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);
			var clinic = new Clinic
			{
				Id = 20,
				Name = "Clinic 20"
			};
			var clinic2 = new Clinic
			{
				Id = 21,
				Name = "Clinic 21"
			};
			dbContext.Clinics.Add(clinic);
			dbContext.Clinics.Add(clinic2);
			await dbContext.SaveChangesAsync();

			var repository = new DbRepository<Clinic>(dbContext);
			var service = new ClinicService(repository);
			Assert.Throws<ArgumentException>(() => service.GetClinicById(22));
		}

		[Fact]
		public void GetClinicById_WithEmptyClinicSet_ShouldReturnException()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);

			var repository = new DbRepository<Clinic>(dbContext);
			var service = new ClinicService(repository);
			Assert.Throws<ArgumentException>(() => service.GetClinicById(7));
		}
	}
}
