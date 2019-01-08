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
        public void GetAllActivePatients_WithValidPatients_ShouldReturnPatientsCollection()
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
                SSN = "123"
            };

            var patient2 = new DentHubUser
            {
                Id = "2",
                IsActive = true,
                FirstName = "Patient 2",
                LastName = "Test",
                SSN = "123"
            };

            var patient3 = new DentHubUser
            {
                Id = "3",
                IsActive = true,
                FirstName = "Patient 3",
                LastName = "Test",
            };

            var patient4 = new DentHubUser
            {
                Id = "4",
                IsActive = true,
                FirstName = "Patient 4",
                LastName = "Test",
                SSN = "123"
            };

            dbContext.DentHubUsers.Add(patient);
            dbContext.DentHubUsers.Add(patient2);
            dbContext.DentHubUsers.Add(patient3);
            dbContext.DentHubUsers.Add(patient4);
            dbContext.SaveChanges();

            var userRepository = new DbRepository<DentHubUser>(dbContext);
            var appointmentRepository = new DbRepository<Appointment>(dbContext);
            var appointmentService = new AppointmentService(appointmentRepository);
            var service = new PatientService(userRepository, appointmentService);
            var result = service.GetAllActivePatients();
            Assert.Equal(new DentHubUser[] { patient, patient2, patient4}, result);
        }

        [Fact]
        public void GetAllActiveDentists_WithInvalidDentists_ShouldReturnEmptyDentistsCollection()
        {
            var options = new DbContextOptionsBuilder<DentHubContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
               .Options;
            var dbContext = new DentHubContext(options);

            var patient = new DentHubUser
            {
                Id = "1",
                IsActive = false,
                FirstName = "Patient 1",
                LastName = "Test",
                SSN = "123"
            };

            var patient2 = new DentHubUser
            {
                Id = "2",
                IsActive = true,
                FirstName = "Dentist 2",
                LastName = "Test2",
                SSN = null,
            };

            dbContext.DentHubUsers.Add(patient);
            dbContext.DentHubUsers.Add(patient2);
            dbContext.SaveChanges();

            var userRepository = new DbRepository<DentHubUser>(dbContext);
            var appointmentRepository = new DbRepository<Appointment>(dbContext);
            var appointmentService = new AppointmentService(appointmentRepository);
            var service = new PatientService(userRepository, appointmentService);
            var result = service.GetAllActivePatients();
            Assert.Empty(result);
        }
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

        [Fact]
        public void GetAllDentistPatients_WithValidPatients_ShouldReturnPatientsCollection()
        {
            var options = new DbContextOptionsBuilder<DentHubContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
               .Options;
            var dbContext = new DentHubContext(options);

            var patient = new DentHubUser
            {
                Id = "10",
                IsActive = true,
                FirstName = "Patient 1",
                LastName = "Test",
            };

            var patient2 = new DentHubUser
            {
                Id = "11",
                IsActive = true,
                FirstName = "Patient 2",
                LastName = "Test",
            };

            var dentist = new DentHubUser
            {
                Id = "13",
                ClinicId = 14,
                IsActive = false,
                FirstName = "Dentist",
                LastName = "Test",
                SpecialtyId = 3,
            };
            var appointment1 = new Appointment
            {
                Id = 1,
                PatientId = "10",
                DentistID = "13",
            };
            var appointment2 = new Appointment
            {
                Id = 2,
                PatientId = "11",
                DentistID = "13",
            };
            var appointment3 = new Appointment
            {
                Id = 3,
                PatientId = "11",
                DentistID = "13",
            };
            dbContext.DentHubUsers.Add(dentist);
            dbContext.DentHubUsers.Add(patient);
            dbContext.DentHubUsers.Add(patient2);
            dbContext.Appointments.Add(appointment1);
            dbContext.Appointments.Add(appointment2);
            dbContext.Appointments.Add(appointment3);
            dbContext.SaveChanges();

            var userRepository = new DbRepository<DentHubUser>(dbContext);
            var appointmentRepository = new DbRepository<Appointment>(dbContext);
            var appointmentService = new AppointmentService(appointmentRepository);
            var service = new PatientService(userRepository, appointmentService);
            var result = service.GetAllDentistPatients("13");
            Assert.Equal(new DentHubUser[] { patient, patient2 }, result);
        }

        [Fact]
        public void GetAllDentistPatients_WithInvalidPatients_ShouldReturnEmptyPatientsCollection()
        {
            var options = new DbContextOptionsBuilder<DentHubContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
               .Options;
            var dbContext = new DentHubContext(options);

            var patient = new DentHubUser
            {
                Id = "10",
                IsActive = true,
                FirstName = "Patient 1",
                LastName = "Test",
            };

            var patient2 = new DentHubUser
            {
                Id = "11",
                IsActive = false,
                FirstName = "Patient 2",
                LastName = "Test",
            };

            var dentist = new DentHubUser
            {
                Id = "13",
                ClinicId = 14,
                IsActive = false,
                FirstName = "Dentist",
                LastName = "Test",
                SpecialtyId = 3,
            };
            var appointment1 = new Appointment
            {
                Id = 1,
                PatientId = "11",
                DentistID = "14",
            };
            var appointment2 = new Appointment
            {
                Id = 2,
                PatientId = "12",
                DentistID = "15",
            };
            var appointment3 = new Appointment
            {
                Id = 3,
                PatientId = "11",
                DentistID = "16",
            };

            dbContext.DentHubUsers.Add(dentist);
            dbContext.DentHubUsers.Add(patient);
            dbContext.DentHubUsers.Add(patient2);
            dbContext.Appointments.Add(appointment1);
            dbContext.Appointments.Add(appointment2);
            dbContext.Appointments.Add(appointment3);
            dbContext.SaveChanges();

            var userRepository = new DbRepository<DentHubUser>(dbContext);
            var appointmentRepository = new DbRepository<Appointment>(dbContext);
            var appointmentService = new AppointmentService(appointmentRepository);
            var service = new PatientService(userRepository, appointmentService);
            var result = service.GetAllDentistPatients("13");
            Assert.Empty(result);
        }

        [Fact]
        public void GetPatientFullName_WithValidId_ShouldReturnFullName()
        {
            var options = new DbContextOptionsBuilder<DentHubContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
               .Options;
            var dbContext = new DentHubContext(options);
            var patient = new DentHubUser
            {
                Id = "1",
                SpecialtyId = 1,
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
                SpecialtyId = 2,
                Email = "patient2@test.com",
                FirstName = "Test2",
                LastName = "LastName2",
                IsActive = true,
                PhoneNumber = "123456",
                UserName = "patient2@test.com",
            };
            dbContext.DentHubUsers.Add(patient);
            dbContext.DentHubUsers.Add(patient2);
            dbContext.SaveChanges();

            var userRepository = new DbRepository<DentHubUser>(dbContext);
            var appointmentRepository = new DbRepository<Appointment>(dbContext);
            var appointmentService = new AppointmentService(appointmentRepository);
            var service = new PatientService(userRepository, appointmentService);
            var result = service.GetPatientFullName("1");
            Assert.Equal("Test LastName", result);
        }

        [Fact]
        public void GetAppointmentPatient_WithValidAppointmentIdAndPatient_ShouldReturnPatient()
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

            var patient1 = new DentHubUser
            {
                Id = "1",
                Email = "patient1@test.com",
                FirstName = "Test",
                LastName = "LastName",
                IsActive = true,
                PhoneNumber = "1234",
                UserName = "patient1@test.com",
            };
            var patient2 = new DentHubUser
            {
                Id = "2",
                Email = "patient2@test.com",
                FirstName = "Test2",
                LastName = "LastName2",
                IsActive = true,
                PhoneNumber = "123456",
                UserName = "patient12@test.com",
            };

            dbContext.Appointments.Add(appointment);
            dbContext.DentHubUsers.Add(patient1);
            dbContext.DentHubUsers.Add(patient2);
            dbContext.SaveChanges();

            var userRepository = new DbRepository<DentHubUser>(dbContext);
            var appointmentRepository = new DbRepository<Appointment>(dbContext);
            var appointmentService = new AppointmentService(appointmentRepository);
            var service = new PatientService(userRepository, appointmentService);
            var result = service.GetAppointmentPatient(1);
            Assert.Equal(patient1, result);
        }


    }
}
