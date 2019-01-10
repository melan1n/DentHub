using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using DentHub.Data;
using DentHub.Data.Common;
using DentHub.Data.Models;
using DentHub.Web.Areas.Administration.Models;
using DentHub.Web.Models.Appointment;
using DentHub.Web.Services.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentHub.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class ClinicController : Controller
    {
        private readonly IClinicService _clinicService;
        private readonly IDentistService _dentistService;
        private readonly IAppointmentService _appointmentService;
        private readonly ISpecialtyService _specialtyService;
        private readonly IRatingService _ratingService;


        public ClinicController(IClinicService clinicService,
            IDentistService dentistService,
            IAppointmentService appointmentService,
            ISpecialtyService specialtyService,
			IRatingService ratingService)
        {
            this._clinicService = clinicService;
            this._dentistService = dentistService;
            this._appointmentService = appointmentService;
            this._specialtyService = specialtyService;
			this._ratingService = ratingService;
        }

        public IActionResult All()
        {
            var allActiveClinics = this._clinicService.GetAllActive();

            var clinicsViewModel = new ClinicsViewModel
            {
                Clinics = allActiveClinics
                .Select(c => new ClinicViewModel
                {
                    Id = c.Id,
                    City = c.City,
                    Country = c.Country,
                    Name = c.Name,
                    PostalCode = c.PostalCode,
                    Street = c.Street,
                    WorkingHours = c.WorkingHours,
                    Dentists = this._dentistService
                                    .GetAllActiveClinicDentists(c.Id)
                                     .Select(
                                    d => new DentistViewModel
                                    {
                                        Id = d.Id,
                                        FirstName = d.FirstName,
                                        LastName = d.LastName,
                                        Specialty = this._specialtyService
                                                    .GetSpecialtyNameById((int)d.SpecialtyId),
                                        ImageUrl = d.ImageUrl,
                                        Offerrings = this._appointmentService
                                                    .GetAllDentistAppointments(d.Id)
                                                    .Select(
                                                a => new AppointmentViewModel
                                                {
                                                    DentistName = a.Dentist.FirstName + a.Dentist.LastName,
                                                    ClinicName = this._clinicService
														.GetClinicById(a.ClinicId).Name,
                                                    TimeStart = a.TimeStart,
                                                    TimeEnd = a.TimeEnd,
                                                    Status = a.Status.ToString()
                                                }).ToArray()
                                    }).ToList(),
                })
                .ToArray()
            };

            return View(clinicsViewModel);

        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

		public IActionResult Details(int id)
		{
			Clinic clinic;

			try
			{
				clinic = this._clinicService.GetClinicById(id);
			}
			catch (Exception)
			{
				ViewBag.ErrorMessage = "Clinic not found.";
				return View("ErrorMessage");
			}
			
			var clinicViewModel = new ClinicViewModel
			{
				Id = clinic.Id,
				City = clinic.City,
				Country = clinic.Country,
				Name = clinic.Name,
				PostalCode = clinic.PostalCode,
				Street = clinic.Street,
				WorkingHours = clinic.WorkingHours,
				Dentists = this._dentistService.GetAllActiveClinicDentists(clinic.Id)
								.Select(
									d => new DentistViewModel
									{
										Id = d.Id,
										FirstName = d.FirstName,
										LastName = d.LastName,
										Specialty = this._specialtyService
												.GetSpecialtyNameById((int)d.SpecialtyId),
										AverageRating = this._ratingService.GetAverageDentistRating(d.Id),
										ImageUrl = d.ImageUrl,
									}).ToList(),
			};

			return View(clinicViewModel);
		}

		[Authorize(Roles = "Administrator")]
		[HttpPost]
		public async Task<IActionResult> Create(ClinicViewModel model)
		{
			if (ModelState.IsValid)
			{
				var newClinic = new Clinic
				{
					Name = model.Name,
					Street = model.Street,
					City = model.City,
					PostalCode = model.PostalCode,
					Country = model.Country,
					WorkingHours = model.WorkingHours,
				};

				var result = _clinicService.AddAsync(newClinic);
				await result;

				await _clinicService.SaveChangesAsync();
			}

			return RedirectToAction("All");
		}

		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> Deactivate(int id)
		{
			Clinic clinic;
			try
			{
				clinic = this._clinicService.GetClinicById(id);
			}
			catch (Exception)
			{
				ViewBag.ErrorMessage = "Clinic not found.";
				return View("ErrorMessage");
			}

			if (clinic != null)
			{
				var clinicDentists = _dentistService
							.GetAllActiveClinicDentists(id);

				foreach (var dentist in clinicDentists)
				{
					dentist.IsActive = false;
					this._dentistService.Update(dentist);
				}

				await _dentistService.SaveChangesAsync();

				clinic.IsActive = false;

				this._clinicService.Update(clinic);

				await _clinicService.SaveChangesAsync();
			}

			return RedirectToAction("All");
		}



		[Authorize(Roles = "Administrator")]
		public IActionResult Edit(int id)
		{
			Clinic clinic;
			try
			{
				clinic = this._clinicService.GetClinicById(id);
			}
			catch (Exception)
			{
				ViewBag.ErrorMessage = "Clinic not found.";
				return View("ErrorMessage");
			}
			

			var clinicViewModel = new ClinicViewModel
			{
				Id = clinic.Id,
				Name = clinic.Name,
				Street = clinic.Street,
				City = clinic.City,
				Country = clinic.Country,
				PostalCode = clinic.PostalCode,
				WorkingHours = clinic.WorkingHours
			};

			return View("Edit", clinicViewModel);
		}

		[Authorize(Roles = "Administrator")]
		[HttpPost]
		public async Task<IActionResult> EditAsync(int id, ClinicViewModel model)
		{
			if (ModelState.IsValid)
			{
				Clinic clinic;
				try
				{
					clinic = this._clinicService.GetClinicById(id);
				}
				catch (Exception)
				{
					ViewBag.ErrorMessage = "Clinic not found.";
					return View("ErrorMessage");
				}

				clinic.Name = model.Name;
				clinic.Street = model.Street;
				clinic.City = model.City;
				clinic.Country = model.Country;
				clinic.WorkingHours = model.WorkingHours;
				clinic.PostalCode = model.PostalCode;

				this._clinicService.Update(clinic);

				await _clinicService.SaveChangesAsync();
			}

			return RedirectToAction("All");
		}
	}
}
