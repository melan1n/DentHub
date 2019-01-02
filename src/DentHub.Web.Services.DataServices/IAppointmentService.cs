using DentHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DentHub.Web.Services.DataServices
{
    public interface IAppointmentService
    {
        IEnumerable<Appointment> GetAllDentistAppointments(string dentistId);
    }
}
