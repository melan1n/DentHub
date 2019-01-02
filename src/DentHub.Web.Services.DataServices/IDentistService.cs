using DentHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DentHub.Web.Services.DataServices
{
    public interface IDentistService
    {
        IEnumerable<DentHubUser> GetAllActive(int clinicId);
    }
}
