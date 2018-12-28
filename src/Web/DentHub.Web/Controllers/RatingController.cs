using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentHub.Data.Common;
using DentHub.Data.Models;
using DentHub.Web.Models.Rating;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DentHub.Web.Controllers
{
    public class RatingController : Controller
    {

		private readonly IRepository<Appointment> _appointmentRepository;
		private readonly IRepository<Rating> _ratingRepository;
		private readonly UserManager<DentHubUser> _userManager;
		private readonly IRepository<DentHubUser> _userRepository;

		public RatingController(IRepository<DentHubUser> userRepository,
			UserManager<DentHubUser> userManager,
			IRepository<Rating> _ratingRepository,
			IRepository<Appointment> appointmentRepository)
		{
			this._userManager = userManager;
			this._userRepository = userRepository;
			this._appointmentRepository = appointmentRepository;
			this._ratingRepository = _ratingRepository;
		}

		public IActionResult Index()
        {
            return View();
        }

		[Authorize(Roles = "Patient,Dentist")]
		public IActionResult RateAppointment(int id)
		{
			var appointment = this._appointmentRepository
					.All()
					.FirstOrDefault(a => a.Id == id);

			var ratingInputModel = new RatingInputModel
			{
				DentistId = appointment.DentistID,
				PatientId = appointment.PatientId,
				ForAppointmentId = id,
			};

			return View(ratingInputModel);
		}

		[HttpPost]
		[Authorize(Roles="Patient,Dentist")]
		public async Task<IActionResult> RateAppointment(int id, int rating)
		{
			var ratingRecord = this._ratingRepository
					.All()
					.FirstOrDefault(r => r.Appointment.Id == id);

			var appointment = this._appointmentRepository
					.All()
					.FirstOrDefault(a => a.Id == id);

			var dentist = this._userRepository
						.All()
						.FirstOrDefault(u => u.Id == appointment.DentistID);

			var patient = this._userRepository
						.All()
						.FirstOrDefault(u => u.Id == appointment.PatientId);

			if (ratingRecord == null)
			{
				ratingRecord = new Rating
				{
					Appointment = appointment,
					DentistId = dentist.Id,
					PatientId = patient.Id,
				};

				await this._ratingRepository.AddAsync(ratingRecord);
				await this._ratingRepository.SaveChangesAsync();
			}

			if (User.IsInRole("Dentist"))
			{
				ratingRecord.RatingByDentist = rating;
				ratingRecord.Appointment.IsRatedByDentist = true;
			}
			else if (User.IsInRole("Patient"))
			{
				ratingRecord.RatingByPatient = rating;
				ratingRecord.Appointment.IsRatedByPatient = true;
			}

			appointment.Status = Status.Completed;

			this._appointmentRepository.Update(appointment);
			this._ratingRepository.Update(ratingRecord);

			await this._appointmentRepository.SaveChangesAsync();
			await this._ratingRepository.SaveChangesAsync();

			return RedirectToAction("../Appointment/Index");
		}
    }
}