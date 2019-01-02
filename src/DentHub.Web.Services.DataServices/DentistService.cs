using DentHub.Data.Common;
using DentHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DentHub.Web.Services.DataServices
{
    public class DentistService : IDentistService
    {
        private readonly IRepository<DentHubUser> _userRepository;
        
        public DentistService(IRepository<DentHubUser> userRepository)
        {
            this._userRepository = userRepository;
        }

        public IEnumerable<DentHubUser> GetAllActive(int clinicId)
        {
            return this._userRepository
                     .All()
                     .Where(d => d.IsActive && d.ClinicId == clinicId)
                     .ToArray();
        }
    }
}
