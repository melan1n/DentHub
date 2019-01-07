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
	public class DentistServiceTests
	{
		[Fact]
		public void GetAllActiveDentists_WithValidDentists_ShouldReturnDentistsCollection()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);

			var dentist = new DentHubUser
			{
				Id = "1",
				IsActive = true,
				FirstName = "Dentist 1",
				LastName = "Test",
				SpecialtyId = 1,
			};

			var dentist2 = new DentHubUser
			{
				Id = "2",
				IsActive = true,
				FirstName = "Dentist 2",
				LastName = "Test2",
				SpecialtyId = 2,
			};

			var dentist3 = new DentHubUser
			{
				Id = "3",
				IsActive = false,
				FirstName = "Dentist 3",
				LastName = "Test3",
				SpecialtyId = 3,
			};
		
			var dentist4 = new DentHubUser
			{
				Id = "4",
				IsActive = true,
				FirstName = "Dentist 4",
				LastName = "Test4",
				SpecialtyId = 44,
			};

			dbContext.DentHubUsers.Add(dentist);
			dbContext.DentHubUsers.Add(dentist2);
			dbContext.DentHubUsers.Add(dentist3);
			dbContext.DentHubUsers.Add(dentist4);
			dbContext.SaveChanges();

			var userRepository = new DbRepository<DentHubUser>(dbContext);
			var appointmentRepository = new DbRepository<Appointment>(dbContext);
			var appointmentService = new AppointmentService(appointmentRepository);
			var service = new DentistService(userRepository, appointmentService);
			var result = service.GetAllActiveDentists();
			Assert.Equal(new DentHubUser[] { dentist, dentist2, dentist4 }, result);
		}

		[Fact]
		public void GetAllActiveDentists_WithInvalidDentists_ShouldReturnEmptyDentistsCollection()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);

			var dentist = new DentHubUser
			{
				Id = "1",
				IsActive = false,
				FirstName = "Dentist 1",
				LastName = "Test",
				SpecialtyId = 1,
			};

			var dentist2 = new DentHubUser
			{
				Id = "2",
				IsActive = true,
				FirstName = "Dentist 2",
				LastName = "Test2",
				Specialty = null,
			};

			dbContext.DentHubUsers.Add(dentist);
			dbContext.DentHubUsers.Add(dentist2);
			dbContext.SaveChanges();

			var userRepository = new DbRepository<DentHubUser>(dbContext);
			var appointmentRepository = new DbRepository<Appointment>(dbContext);
			var appointmentService = new AppointmentService(appointmentRepository);
			var service = new DentistService(userRepository, appointmentService);
			var result = service.GetAllActiveDentists();
			Assert.Empty(result);
		}

		[Fact]
		public void GetAllActiveClinicDentists_WithValidDentists_ShouldReturnDentistsCollection()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);

			var dentist = new DentHubUser
			{
				Id = "10",
				ClinicId = 1,
				IsActive = true,
				FirstName = "Dentist 1",
				LastName = "Test",
				SpecialtyId = 1,
			};

			var dentist2 = new DentHubUser
			{
				Id = "11",
				ClinicId = 1,
				IsActive = true,
				FirstName = "Dentist 2",
				LastName = "Test2",
				SpecialtyId = 2,
			};

			var dentist3 = new DentHubUser
			{
				Id = "13",
				ClinicId = 14,
				IsActive = false,
				FirstName = "Dentist 3",
				LastName = "Test3",
				SpecialtyId = 3,
			};

			var dentist4 = new DentHubUser
			{
				Id = "4",
				IsActive = true,
				FirstName = "Dentist 4",
				LastName = "Test4",
				SpecialtyId = 44,
			};

			dbContext.DentHubUsers.Add(dentist);
			dbContext.DentHubUsers.Add(dentist2);
			dbContext.DentHubUsers.Add(dentist3);
			dbContext.DentHubUsers.Add(dentist4);
			dbContext.SaveChanges();

			var userRepository = new DbRepository<DentHubUser>(dbContext);
			var appointmentRepository = new DbRepository<Appointment>(dbContext);
			var appointmentService = new AppointmentService(appointmentRepository);
			var service = new DentistService(userRepository, appointmentService);
			var result = service.GetAllActiveClinicDentists(1);
			Assert.Equal(new DentHubUser[] { dentist, dentist2 }, result);
		}

		[Fact]
		public void GetAllActiveClinicDentists_WithInvalidDentists_ShouldReturnEmptyDentistsCollection()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);

			var dentist = new DentHubUser
			{
				Id = "1",
				IsActive = false,
				FirstName = "Dentist 1",
				LastName = "Test",
				SpecialtyId = 1,
			};

			var dentist2 = new DentHubUser
			{
				Id = "2",
				IsActive = true,
				ClinicId = 1,
				FirstName = "Dentist 2",
				LastName = "Test2",
				Specialty = null,
			};

			dbContext.DentHubUsers.Add(dentist);
			dbContext.DentHubUsers.Add(dentist2);
			dbContext.SaveChanges();

			var userRepository = new DbRepository<DentHubUser>(dbContext);
			var appointmentRepository = new DbRepository<Appointment>(dbContext);
			var appointmentService = new AppointmentService(appointmentRepository);
			var service = new DentistService(userRepository, appointmentService);
			var result = service.GetAllActiveClinicDentists(2);
			Assert.Empty(result);
		}

		[Fact]
		public void GetDentistById_WithValidId_ShouldReturnDentist()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);
			var dentist = new DentHubUser
			{
				Id = "1",
				SpecialtyId = 1,
				Email = "dentist@test.com",
				FirstName = "Test",
				LastName = "LastName",
				IsActive = true,
				PhoneNumber = "1234",
				UserName = "dentist@test.com",
			};
			var dentist2 = new DentHubUser
			{
				Id = "2",
				SpecialtyId = 2,
				Email = "dentist2@test.com",
				FirstName = "Test2",
				LastName = "LastName2",
				IsActive = true,
				PhoneNumber = "123456",
				UserName = "dentist2@test.com",
			};
			dbContext.DentHubUsers.Add(dentist);
			dbContext.DentHubUsers.Add(dentist2);
			dbContext.SaveChanges();

			var userRepository = new DbRepository<DentHubUser>(dbContext);
			var appointmentRepository = new DbRepository<Appointment>(dbContext);
			var appointmentService = new AppointmentService(appointmentRepository);
			var service = new DentistService(userRepository, appointmentService);
			var result = service.GetDentistById("2");
			Assert.Same(dentist2, result);
		}

		[Fact]
		public async Task GetDentistById_WithInvalidId_ShouldReturnException()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);
			var inactiveDentist = new DentHubUser
			{
				Id = "10",
				ClinicId = 1,
				Email = "dentist@test.com",
				FirstName = "Test3",
				LastName = "LastName3",
				IsActive = false,
				PhoneNumber = "1234",
				UserName = "dentist@test.com",
			};
			var dentist = new DentHubUser
			{
				Id = "11",
				ClinicId = 1,
				Email = "dentist2@test.com",
				FirstName = "Test2",
				LastName = "LastName2",
				IsActive = true,
				PhoneNumber = "123456",
				UserName = "dentist2@test.com",
			};
			dbContext.DentHubUsers.Add(inactiveDentist);
			dbContext.DentHubUsers.Add(dentist);
			await dbContext.SaveChangesAsync();

			var userRepository = new DbRepository<DentHubUser>(dbContext);
			var appointmentRepository = new DbRepository<Appointment>(dbContext);
			var appointmentService = new AppointmentService(appointmentRepository);
			var service = new DentistService(userRepository, appointmentService);
			Assert.Throws<ArgumentException>(() => service.GetDentistById("15"));
		}

		[Fact]
		public async Task GetDentistById_WithValidIdAndNoSpecialty_ShouldReturnException()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);
			var dentist = new DentHubUser
			{
				Id = "20",
				SpecialtyId = 1,
				Email = "dentist@test.com",
				FirstName = "Test",
				LastName = "LastName",
				IsActive = false,
				PhoneNumber = "1234",
				UserName = "dentist@test.com",
			};
			var dentist2 = new DentHubUser
			{
				Id = "21",
				Specialty = null,
				Email = "dentist2@test.com",
				FirstName = "Test2",
				LastName = "LastName2",
				IsActive = true,
				PhoneNumber = "123456",
				UserName = "dentist2@test.com",
			};
			dbContext.DentHubUsers.Add(dentist);
			dbContext.DentHubUsers.Add(dentist2);
			await dbContext.SaveChangesAsync();

			var userRepository = new DbRepository<DentHubUser>(dbContext);
			var appointmentRepository = new DbRepository<Appointment>(dbContext);
			var appointmentService = new AppointmentService(appointmentRepository);
			var service = new DentistService(userRepository, appointmentService);
			Assert.Throws<ArgumentException>(() => service.GetDentistById("21"));
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
			var service = new DentistService(userRepository, appointmentService);
			Assert.Throws<ArgumentException>(() => service.GetDentistById("7"));
		}

		[Fact]
		public void GetAllPatientDentists_WithValidIdAndDentists_ShouldReturnDentist()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);
			var appointment = new Appointment
			{
				PatientId = "1",
				DentistID = "1"
			};
			var appointment2 = new Appointment
			{
				PatientId = "1",
				DentistID = "2",
			};
			var appointment3 = new Appointment
			{
				PatientId = "1",
				DentistID = "2",
			};
			var dentist1 = new DentHubUser
			{
				Id = "1",
				SpecialtyId = 1,
				Email = "dentist@test.com",
				FirstName = "Test",
				LastName = "LastName",
				IsActive = true,
				PhoneNumber = "1234",
				UserName = "dentist@test.com",
			};
			var dentist2 = new DentHubUser
			{
				Id = "2",
				SpecialtyId = 2,
				Email = "dentist2@test.com",
				FirstName = "Test2",
				LastName = "LastName2",
				IsActive = true,
				PhoneNumber = "123456",
				UserName = "dentist2@test.com",
			};

			dbContext.Appointments.Add(appointment);
			dbContext.Appointments.Add(appointment2);
			dbContext.Appointments.Add(appointment3);
			dbContext.DentHubUsers.Add(dentist1);
			dbContext.DentHubUsers.Add(dentist2);
			dbContext.SaveChanges();

			var userRepository = new DbRepository<DentHubUser>(dbContext);
			var appointmentRepository = new DbRepository<Appointment>(dbContext);
			var appointmentService = new AppointmentService(appointmentRepository);
			var service = new DentistService(userRepository, appointmentService);
			var result = service.GetAllPatientDentists("1");
			Assert.Equal(new DentHubUser[] { dentist1, dentist2 }, result);
		}

		[Fact]
		public void GetAllPatientDentists_WithValidIdAndNoDentists_ShouldReturnEmptyDentistCollection()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);
			var appointment = new Appointment
			{
				PatientId = "1",
				DentistID = "1"
			};
			var appointment2 = new Appointment
			{
				PatientId = "1",
				DentistID = "2",
			};
			var appointment3 = new Appointment
			{
				PatientId = "1",
				DentistID = "2",
			};
			var dentist1 = new DentHubUser
			{
				Id = "1",
				SpecialtyId = 1,
				Email = "dentist@test.com",
				FirstName = "Test",
				LastName = "LastName",
				IsActive = true,
				PhoneNumber = "1234",
				UserName = "dentist@test.com",
			};
			var dentist2 = new DentHubUser
			{
				Id = "2",
				SpecialtyId = 2,
				Email = "dentist2@test.com",
				FirstName = "Test2",
				LastName = "LastName2",
				IsActive = true,
				PhoneNumber = "123456",
				UserName = "dentist2@test.com",
			};

			dbContext.Appointments.Add(appointment);
			dbContext.Appointments.Add(appointment2);
			dbContext.Appointments.Add(appointment3);
			dbContext.DentHubUsers.Add(dentist1);
			dbContext.DentHubUsers.Add(dentist2);
			dbContext.SaveChanges();

			var userRepository = new DbRepository<DentHubUser>(dbContext);
			var appointmentRepository = new DbRepository<Appointment>(dbContext);
			var appointmentService = new AppointmentService(appointmentRepository);
			var service = new DentistService(userRepository, appointmentService);
			var result = service.GetAllPatientDentists("5");
			Assert.Empty(result);
		}

		[Fact]
		public void GetDentistFullName_WithValidId_ShouldReturnFullName()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);
			var dentist = new DentHubUser
			{
				Id = "1",
				SpecialtyId = 1,
				Email = "dentist@test.com",
				FirstName = "Test",
				LastName = "LastName",
				IsActive = true,
				PhoneNumber = "1234",
				UserName = "dentist@test.com",
			};
			var dentist2 = new DentHubUser
			{
				Id = "2",
				SpecialtyId = 2,
				Email = "dentist2@test.com",
				FirstName = "Test2",
				LastName = "LastName2",
				IsActive = true,
				PhoneNumber = "123456",
				UserName = "dentist2@test.com",
			};
			dbContext.DentHubUsers.Add(dentist);
			dbContext.DentHubUsers.Add(dentist2);
			dbContext.SaveChanges();

			var userRepository = new DbRepository<DentHubUser>(dbContext);
			var appointmentRepository = new DbRepository<Appointment>(dbContext);
			var appointmentService = new AppointmentService(appointmentRepository);
			var service = new DentistService(userRepository, appointmentService);
			var result = service.GetDentistFullName("1");
			Assert.Equal("Test LastName", result);
		}

		[Fact]
		public void GetAppointmentDentist_WithValidAppointmentIdAndDentists_ShouldReturnDentist()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);
			var appointment = new Appointment
			{
				Id = 1,
				PatientId = "1",
				DentistID = "1"
			};
			
			var dentist1 = new DentHubUser
			{
				Id = "1",
				SpecialtyId = 1,
				Email = "dentist@test.com",
				FirstName = "Test",
				LastName = "LastName",
				IsActive = true,
				PhoneNumber = "1234",
				UserName = "dentist@test.com",
			};
			var dentist2 = new DentHubUser
			{
				Id = "2",
				SpecialtyId = 2,
				Email = "dentist2@test.com",
				FirstName = "Test2",
				LastName = "LastName2",
				IsActive = true,
				PhoneNumber = "123456",
				UserName = "dentist2@test.com",
			};

			dbContext.Appointments.Add(appointment);
			dbContext.DentHubUsers.Add(dentist1);
			dbContext.DentHubUsers.Add(dentist2);
			dbContext.SaveChanges();

			var userRepository = new DbRepository<DentHubUser>(dbContext);
			var appointmentRepository = new DbRepository<Appointment>(dbContext);
			var appointmentService = new AppointmentService(appointmentRepository);
			var service = new DentistService(userRepository, appointmentService);
			var result = service.GetAppointmentDentist(1);
			Assert.Equal(dentist1, result);
		}

		
	}
}
