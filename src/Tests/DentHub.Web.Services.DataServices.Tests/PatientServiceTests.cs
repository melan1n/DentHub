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
	public class PatientServiceTests
	{
		[Fact]
		public async Task GetPatientById_WithValidId_ShouldReturnPatient()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);
			var patient = new DentHubUser
			{
				Id = "1",
				SSN = "123456",
				Email = "patient@test.com",
				FirstName = "Test",
				LastName = "LastName",
				IsActive = true,
				PhoneNumber = "1234",
				UserName = "patient@test.com",
			};
			var patient2 = new DentHubUser
			{
				Id = "2",
				SSN = "1234567",
				Email = "patient2@test.com",
				FirstName = "Test2",
				LastName = "LastName2",
				IsActive = true,
				PhoneNumber = "123456",
				UserName = "patient2@test.com",
			};
			dbContext.DentHubUsers.Add(patient);
			dbContext.DentHubUsers.Add(patient2);
			await dbContext.SaveChangesAsync();

			var userRepository = new DbRepository<DentHubUser>(dbContext);
			var appointmentRepository = new DbRepository<Appointment>(dbContext);
			var appointmentService = new AppointmentService(appointmentRepository);
			var service = new PatientService(userRepository, appointmentService);
			var result = service.GetPatientById("2");
			Assert.Same(patient2, result);
		}

		[Fact]
		public async Task GetPatientById_WithInvalidId_ShouldReturnException()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);
			var inactivePatient3 = new DentHubUser
			{
				Id = "3",
				SSN = "123456",
				Email = "patient3@test.com",
				FirstName = "Test3",
				LastName = "LastName3",
				IsActive = false,
				PhoneNumber = "1234",
				UserName = "patient3@test.com",
			};
			var patient4 = new DentHubUser
			{
				Id = "4",
				SSN = "1234567",
				Email = "patient2@test.com",
				FirstName = "Test2",
				LastName = "LastName2",
				IsActive = true,
				PhoneNumber = "123456",
				UserName = "patient2@test.com",
			};
			dbContext.DentHubUsers.Add(inactivePatient3);
			dbContext.DentHubUsers.Add(patient4);
			await dbContext.SaveChangesAsync();

			var userRepository = new DbRepository<DentHubUser>(dbContext);
			var appointmentRepository = new DbRepository<Appointment>(dbContext);
			var appointmentService = new AppointmentService(appointmentRepository);
			var service = new PatientService(userRepository, appointmentService);
			Assert.Throws<ArgumentException>(() => service.GetPatientById("5"));
		}

		[Fact]
		public async Task GetPatientById_WithValidIdAndNoSSN_ShouldReturnException()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);
			var patient5 = new DentHubUser
			{
				Id = "5",
				SSN = "123456",
				Email = "patient5@test.com",
				FirstName = "Test5",
				LastName = "LastName5",
				IsActive = false,
				PhoneNumber = "1234",
				UserName = "patient3@test.com",
			};
			var patient6 = new DentHubUser
			{
				Id = "6",
				SSN = null,
				Email = "patient6@test.com",
				FirstName = "Test6",
				LastName = "LastName6",
				IsActive = true,
				PhoneNumber = "123456",
				UserName = "patient6@test.com",
			};
			dbContext.DentHubUsers.Add(patient5);
			dbContext.DentHubUsers.Add(patient6);
			await dbContext.SaveChangesAsync();

			var userRepository = new DbRepository<DentHubUser>(dbContext);
			var appointmentRepository = new DbRepository<Appointment>(dbContext);
			var appointmentService = new AppointmentService(appointmentRepository);
			var service = new PatientService(userRepository, appointmentService);
			Assert.Throws<ArgumentException>(() => service.GetPatientById("6"));
		}

		[Fact]
		public void GetPatientById_WithEmptyPatientSet_ShouldReturnException()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);

			var userRepository = new DbRepository<DentHubUser>(dbContext);
			var appointmentRepository = new DbRepository<Appointment>(dbContext);
			var appointmentService = new AppointmentService(appointmentRepository);
			var service = new PatientService(userRepository, appointmentService);
			Assert.Throws<ArgumentException>(() => service.GetPatientById("7"));
		}
	}
}
