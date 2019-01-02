using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentHub.Data.Common;
using DentHub.Data.Models;


namespace DentHub.Web.Services.DataServices
{
    public class ClinicService : IClinicService
    {
        private readonly IRepository<Clinic> _clinicRepository;

        public ClinicService(IRepository<Clinic> clinicRepository)
        {
        this._clinicRepository = clinicRepository;
        }
        public IEnumerable<Clinic> GetAllActive()
        {
            return this._clinicRepository
                .All()
                .Where(c => c.IsActive && c.Dentists.Any(d => d.IsActive))
                .ToArray();
        }
    }
}
