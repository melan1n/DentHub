using DentHub.Data;
using DentHub.Data.Common;
using DentHub.Data.Models;
using DentHub.Web.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DentHub.Web.Services.DataServices.Tests
{
	public class AppointmentServiceTests
	{
		[Fact]
		public async Task GetAppointmentById_WithValidId_ShouldReturnAppointment()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);
			var appointment = new Appointment
			{
				Id = 5,
				ClinicId = 1,
				DentistID = "2",
				PatientId = "3",
				Status = Status.Booked
			};
			var appointment2 = new Appointment
			{
				Id = 6,
				ClinicId = 1,
				DentistID = "2",
				PatientId = "3",
				Status = Status.Booked
			};
			dbContext.Appointments.Add(appointment);
			dbContext.Appointments.Add(appointment2);
			await dbContext.SaveChangesAsync();

			var repository = new DbRepository<Appointment>(dbContext);
            var ratingRepository = new DbRepository<Rating>(dbContext); 
            var service = new AppointmentService(repository, ratingRepository);
			var result = service.GetAppointmentById(5);
			Assert.Same(appointment, result);
		}

		[Fact]
		public async Task GetAppointmentById_WithInvalidId_ShouldReturnException()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);
			var appointment = new Appointment
			{
				Id = 7,
				ClinicId = 1,
				DentistID = "2",
				PatientId = "3",
				Status = Status.Booked
			};
			var appointment2 = new Appointment
			{
				Id = 8,
				ClinicId = 1,
				DentistID = "2",
				PatientId = "3",
				Status = Status.Booked
			};
			dbContext.Appointments.Add(appointment);
			dbContext.Appointments.Add(appointment2);
			await dbContext.SaveChangesAsync();

			var repository = new DbRepository<Appointment>(dbContext);
            var ratingRepository = new DbRepository<Rating>(dbContext);
            var service = new AppointmentService(repository, ratingRepository);
            Assert.Throws<ArgumentException>(() => service.GetAppointmentById(9));
		}

		[Fact]
		public void GetAppointmentById_WithEmptyAppointmentSet_ShouldReturnException()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);

			var repository = new DbRepository<Appointment>(dbContext);
            var ratingRepository = new DbRepository<Rating>(dbContext);
            var service = new AppointmentService(repository, ratingRepository);
            Assert.Throws<ArgumentException>(() => service.GetAppointmentById(7));
		}

		[Fact]
		public async Task GetAllDentistAppointments_WithValidId_ShouldReturnAppointments()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);
			var appointment = new Appointment
			{
				Id = 11,
				ClinicId = 1,
				DentistID = "1",
				PatientId = "3",
				Status = Status.Booked
			};
			var appointment2 = new Appointment
			{
				Id = 12,
				ClinicId = 1,
				DentistID = "1",
				PatientId = "4",
				Status = Status.Booked
			};
			var appointment3 = new Appointment
			{
				Id = 13,
				ClinicId = 1,
				DentistID = "2",
				PatientId = "4",
				Status = Status.Booked
			};
			dbContext.Appointments.Add(appointment);
			dbContext.Appointments.Add(appointment2);
			dbContext.Appointments.Add(appointment3);
			await dbContext.SaveChangesAsync();

			var repository = new DbRepository<Appointment>(dbContext);
            var ratingRepository = new DbRepository<Rating>(dbContext);
            var service = new AppointmentService(repository, ratingRepository);
            var result = service.GetAllDentistAppointments("1");
			Assert.Equal(new Appointment[] { appointment, appointment2 }, result);
		}

		[Fact]
		public async Task GetAllDentistAppointments_WithInvalidId_ShouldReturnEmptyCollection()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);
			var appointment = new Appointment
			{
				Id = 14,
				ClinicId = 1,
				DentistID = "1",
				PatientId = "3",
				Status = Status.Booked
			};
			var appointment2 = new Appointment
			{
				Id = 15,
				ClinicId = 1,
				DentistID = "1",
				PatientId = "4",
				Status = Status.Booked
			};

			dbContext.Appointments.Add(appointment);
			dbContext.Appointments.Add(appointment2);
			await dbContext.SaveChangesAsync();

			var repository = new DbRepository<Appointment>(dbContext);
            var ratingRepository = new DbRepository<Rating>(dbContext);
            var service = new AppointmentService(repository, ratingRepository);
            Assert.Empty(service.GetAllDentistAppointments("2"));
		}

		[Fact]
		public async Task GetAllPatientAppointments_WithValidId_ShouldReturnAppointments()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);
			var appointment = new Appointment
			{
				Id = 11,
				ClinicId = 1,
				DentistID = "1",
				PatientId = "1",
				Status = Status.Booked
			};
			var appointment2 = new Appointment
			{
				Id = 12,
				ClinicId = 1,
				DentistID = "2",
				PatientId = "1",
				Status = Status.Booked
			};
			var appointment3 = new Appointment
			{
				Id = 13,
				ClinicId = 1,
				DentistID = "2",
				PatientId = "2",
				Status = Status.Booked
			};
			dbContext.Appointments.Add(appointment);
			dbContext.Appointments.Add(appointment2);
			dbContext.Appointments.Add(appointment3);
			await dbContext.SaveChangesAsync();

			var repository = new DbRepository<Appointment>(dbContext);
            var ratingRepository = new DbRepository<Rating>(dbContext);
            var service = new AppointmentService(repository, ratingRepository);
            var result = service.GetAllPatientAppointments("1");
			Assert.Equal(new Appointment[] { appointment, appointment2 }, result);
		}

		[Fact]
		public async Task GetAllPatientAppointments_WithInvalidId_ShouldReturnEmptyCollection()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);
			var appointment = new Appointment
			{
				Id = 14,
				ClinicId = 1,
				DentistID = "1",
				PatientId = "1",
				Status = Status.Booked
			};
			var appointment2 = new Appointment
			{
				Id = 15,
				ClinicId = 1,
				DentistID = "1",
				PatientId = "2",
				Status = Status.Booked
			};

			dbContext.Appointments.Add(appointment);
			dbContext.Appointments.Add(appointment2);
			await dbContext.SaveChangesAsync();

			var repository = new DbRepository<Appointment>(dbContext);
            var ratingRepository = new DbRepository<Rating>(dbContext);
            var service = new AppointmentService(repository, ratingRepository);
            Assert.Empty(service.GetAllPatientAppointments("3"));
		}

		[Fact]
		public async Task CreateAppointment_WithParameters_ShouldCreateAppointment()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);

			var repository = new DbRepository<Appointment>(dbContext);
            var ratingRepository = new DbRepository<Rating>(dbContext);
            var service = new AppointmentService(repository, ratingRepository);

            var user = new DentHubUser()
			{
				Id = "1",
				ClinicId = 1,
				FirstName = "Test",
				LastName = "Dentist",
			};
			var timeStart = new DateTime(2019, 05, 05, 10, 0, 0);
			var timeEnd = new DateTime(2019, 05, 05, 10, 30, 0);

			Assert.NotNull(service.CreateAppointment(user,
				timeStart, timeEnd));
			var result = await dbContext.Appointments.CountAsync();
			Assert.Equal(1, result);
		}

		[Fact]
		public async Task BookAppointmentAsync_WithValidId_ShouldMakeAppointmentStatusBooked()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);

			var repository = new DbRepository<Appointment>(dbContext);
            var ratingRepository = new DbRepository<Rating>(dbContext);
            var service = new AppointmentService(repository, ratingRepository);

            var user = new DentHubUser()
			{
				Id = "1",
				SSN = "123456",
				FirstName = "Test",
				LastName = "Patient",
			};

			var appointment = new Appointment
			{
				Id = 20,
				ClinicId = 1,
				DentistID = "1",
				PatientId = null,
				Status = Status.Offering,
			};

			dbContext.Appointments.Add(appointment);
			dbContext.DentHubUsers.Add(user);
			await dbContext.SaveChangesAsync();

			await service
				.BookAppointmentAsync(20, user);

			var result = dbContext.Appointments
				.FirstOrDefaultAsync(a => a.Id == 20)
				.GetAwaiter()
				.GetResult();

			Assert.Equal("Booked", result
				.Status
				.ToString());
			Assert.Equal("1", result
				.PatientId);
		}

		[Fact]
		public void DuplicateOfferingExists_WithDuplicateExists_ShouldReturnTrue()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);

			var repository = new DbRepository<Appointment>(dbContext);
            var ratingRepository = new DbRepository<Rating>(dbContext);
            var service = new AppointmentService(repository, ratingRepository);

            var user = new DentHubUser()
			{
				Id = "1",
				FirstName = "Test",
				LastName = "Dentist",
			};

			var appointment = new Appointment
			{
				Id = 30,
				ClinicId = 1,
				DentistID = "1",
				TimeStart = new DateTime(2019, 05, 05, 10, 0, 0),
				TimeEnd = new DateTime(2019, 05, 05, 10, 30, 0),
				Status = Status.Offering,
			};

			dbContext.Appointments.Add(appointment);
			dbContext.DentHubUsers.Add(user);
			dbContext.SaveChanges();

			var timeStart = new DateTime(2019, 05, 05, 09, 30, 0);
			var timeEnd = new DateTime(2019, 05, 05, 10, 30, 0);

			var result = service.DuplicateOfferingExists
				(user, timeStart, timeEnd);
			Assert.True(result);
		}

		[Fact]
		public void DuplicateOfferingExists_WithNoDuplicates_ShouldReturnFalse()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);

			var repository = new DbRepository<Appointment>(dbContext);
            var ratingRepository = new DbRepository<Rating>(dbContext);
            var service = new AppointmentService(repository, ratingRepository);

            var user = new DentHubUser()
			{
				Id = "2",
				FirstName = "Test2",
				LastName = "Dentist",
			};

			var appointment = new Appointment
			{
				Id = 31,
				ClinicId = 1,
				DentistID = "2",
				TimeStart = new DateTime(2019, 05, 05, 10, 0, 0),
				TimeEnd = new DateTime(2019, 05, 05, 10, 30, 0),
				Status = Status.Offering,
			};

			dbContext.Appointments.Add(appointment);
			dbContext.DentHubUsers.Add(user);
			dbContext.SaveChanges();

			var timeStart = new DateTime(2019, 05, 05, 10, 30, 0);
			var timeEnd = new DateTime(2019, 05, 05, 11, 00, 0);

			var result = service.DuplicateOfferingExists
				(user, timeStart, timeEnd);
			Assert.False(result);
		}

		[Fact]
		public async Task CancelAppointmentAsync_WithValidId_ShouldDeleteAppointment()
		{
			var options = new DbContextOptionsBuilder<DentHubContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
			   .Options;
			var dbContext = new DentHubContext(options);

			var repository = new DbRepository<Appointment>(dbContext);
            var ratingRepository = new DbRepository<Rating>(dbContext);
            var service = new AppointmentService(repository, ratingRepository);

            var appointment = new Appointment
			{
				Id = 41,
				ClinicId = 1,
				DentistID = "1",
			};

			dbContext.Appointments.Add(appointment);
			dbContext.SaveChanges();

			await service.CancelAppointmentAsync(41);
			var result = dbContext.Appointments
				.CountAsync()
				.GetAwaiter()
				.GetResult();

			Assert.Equal(0, result);
		}
	}
}