using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentHub.Data.Common;
using DentHub.Data.Models;

namespace DentHub.Web.Services.DataServices
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository<Appointment> _appointmentRepository;

        public AppointmentService(IRepository<Appointment> appointmentRepository)
        {
            this._appointmentRepository = appointmentRepository;
        }

        public IEnumerable<Appointment> GetAllDentistAppointments(string dentistId)
        {
            return this._appointmentRepository
                .All()
                .Where(a => a.DentistID == dentistId)
                .ToArray();
        }
    }
}
