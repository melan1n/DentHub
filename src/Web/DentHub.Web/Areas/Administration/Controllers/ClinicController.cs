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


        public ClinicController(IClinicService clinicService,
            IDentistService dentistService,
            IAppointmentService appointmentService,
            ISpecialtyService specialtyService)
        {
            this._clinicService = clinicService;
            this._dentistService = dentistService;
            this._appointmentService = appointmentService;
            this._specialtyService = specialtyService;
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
                                    .GetAllActive(c.Id)
                                     .Select(
                                    d => new DentistViewModel
                                    {
                                        Id = d.Id,
                                        FirstName = d.FirstName,
                                        LastName = d.LastName,
                                        Specialty = this._specialtyService
                                                    .GetAll()
                                                    .FirstOrDefault(s => s.Id == d.SpecialtyId).Name,
                                        ImageUrl = d.ImageUrl,
                                        Offerrings = this._appointmentService
                                                    .GetAllDentistAppointments(d.Id)
                                                    .Select(
                                                a => new AppointmentViewModel
                                                {
                                                    DentistName = a.Dentist.FirstName + a.Dentist.LastName,
                                                    ClinicName = a.Clinic.Name,
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


        //// Delete after exposing Data services
        //private void GetAllActiveClinics(ClinicsViewModel clinicsViewModel)
        //{

        //	clinicsViewModel.Clinics = this._clinicRepository
        //										.All()
        //										.Where(c => c.IsActive && c.Dentists.Any(d => d.IsActive))
        //										.Select(
        //						c => new ClinicViewModel
        //						{
        //							Id = c.Id,
        //							City = c.City,
        //							Country = c.Country,
        //							Name = c.Name,
        //							PostalCode = c.PostalCode,
        //							Street = c.Street,
        //							WorkingHours = c.WorkingHours,
        //							Dentists = this._dentistRepository
        //										.All()
        //										.Where(d => d.IsActive && d.ClinicId == c.Id)
        //										.Select(
        //							d => new DentistViewModel
        //							{
        //								Id = d.Id,
        //								FirstName = d.FirstName,
        //								LastName = d.LastName,
        //								Specialty = d.Specialty.Name,
        //								ImageUrl = d.ImageUrl,
        //								Offerrings = this._appointmentRepository
        //											.All()
        //											.Where(a => a.DentistID == d.Id)
        //											.Select(
        //										a => new AppointmentViewModel
        //										{
        //											DentistName = a.Dentist.FirstName + a.Dentist.LastName,
        //											ClinicName = a.Clinic.Name,
        //											TimeStart = a.TimeStart,
        //											TimeEnd = a.TimeEnd,
        //											Status = a.Status.ToString()
        //										}).ToArray()
        //							}).ToList(),
        //						})
        //							.ToArray();
        //}

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        //public IActionResult Details(int id)
        //{
        //    var clinic = GetClinic(id);

        //    var clinicViewModel = new ClinicViewModel
        //    {
        //        Id = clinic.Id,
        //        City = clinic.City,
        //        Country = clinic.Country,
        //        Name = clinic.Name,
        //        PostalCode = clinic.PostalCode,
        //        Street = clinic.Street,
        //        WorkingHours = clinic.WorkingHours,
        //        Dentists = this._dentistRepository
        //                                        .All()
        //                                        .Where(d => d.IsActive && d.ClinicId == clinic.Id)
        //                                        .Select(
        //                            d => new DentistViewModel
        //                            {
        //                                Id = d.Id,
        //                                FirstName = d.FirstName,
        //                                LastName = d.LastName,
        //                                Specialty = d.Specialty.Name,
        //                                ImageUrl = d.ImageUrl,
        //                            }).ToList(),
        //    };

        //    return View(clinicViewModel);
        //}

        //[Authorize(Roles = "Administrator")]
        //[HttpPost]
        //public async Task<IActionResult> Create(ClinicViewModel model)
        ////public async Task<IActionResult> Create([FromBody] string content)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var newClinic = new Clinic
        //        {
        //            Name = model.Name,
        //            Street = model.Street,
        //            City = model.City,
        //            PostalCode = model.PostalCode,
        //            Country = model.Country,
        //            WorkingHours = model.WorkingHours,
        //        };

        //        var result = _clinicRepository.AddAsync(newClinic);
        //        await result;

        //        await _clinicRepository.SaveChangesAsync();
        //    }

        //    return RedirectToAction("All");
        //}

        //[Authorize(Roles = "Administrator")]
        //public async Task<IActionResult> Deactivate(int id)
        //{
        //    Clinic clinic = GetClinic(id);

        //    if (clinic != null)
        //    {
        //        var clinicDentists = _dentistRepository
        //                    .All()
        //                    .Where(d => d.Clinic == clinic)
        //                    .Select(d => d)
        //                    .ToArray();
        //        foreach (var dentist in clinicDentists)
        //        {
        //            dentist.IsActive = false;
        //            this._dentistRepository.Update(dentist);
        //        }

        //        await _dentistRepository.SaveChangesAsync();

        //        clinic.IsActive = false;

        //        this._clinicRepository.Update(clinic);

        //        await _clinicRepository.SaveChangesAsync();
        //    }

        //    //this._clinicRepository.Delete(clinic);
        //    //_clinicRepository.SaveChangesAsync().GetAwaiter().GetResult();

        //    return RedirectToAction("All");
        //}

        //private Clinic GetClinic(int id)
        //{
        //    return this._clinicRepository
        //                            .All()
        //                            .FirstOrDefault(c => c.Id == id);
        //}

        //[Authorize(Roles = "Administrator")]
        //public IActionResult Edit(int id)
        //{
        //    Clinic clinic = GetClinic(id);

        //    var clinicViewModel = new ClinicViewModel
        //    {
        //        Id = clinic.Id,
        //        Name = clinic.Name,
        //        Street = clinic.Street,
        //        City = clinic.City,
        //        Country = clinic.Country,
        //        PostalCode = clinic.PostalCode,
        //        WorkingHours = clinic.WorkingHours
        //    };

        //    return View("Edit", clinicViewModel);
        //}

        //[Authorize(Roles = "Administrator")]
        //[HttpPost]
        //public async Task<IActionResult> EditAsync(int id, ClinicViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var clinic = GetClinic(id);

        //        clinic.Name = model.Name;
        //        clinic.Street = model.Street;
        //        clinic.City = model.City;
        //        clinic.Country = model.Country;
        //        clinic.WorkingHours = model.WorkingHours;
        //        clinic.PostalCode = model.PostalCode;

        //        this._clinicRepository.Update(clinic);

        //        await _clinicRepository.SaveChangesAsync();
        //    }

        //    //var clinicsViewModel = new ClinicsViewModel();

        //    //GetAllClinics(clinicsViewModel);

        //    return RedirectToAction("All");
        //}
    }
}
