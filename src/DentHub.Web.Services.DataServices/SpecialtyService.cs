using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentHub.Data.Common;
using DentHub.Data.Models;

namespace DentHub.Web.Services.DataServices
{
    public class SpecialtyService : ISpecialtyService
    {
        private IRepository<Specialty> _specialtyRepository;

        public SpecialtyService(IRepository<Specialty> specialtyRepository)
        {
            this._specialtyRepository = specialtyRepository;
        }

        public IEnumerable<Specialty> GetAll()
        {
            return this._specialtyRepository
                 .All()
                 .Select(s => s)
                 .ToArray();
        }
    }
}
