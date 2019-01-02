using DentHub.Data.Common;
using DentHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentHub.Web.Services.DataServices
{
    public class DentistService : IDentistService
    {
        private readonly IRepository<DentHubUser> _userRepository;
        
        public DentistService(IRepository<DentHubUser> userRepository)
        {
            this._userRepository = userRepository;
        }

		public IEnumerable<DentHubUser> GetAllActiveDentists()
		{
			return this._userRepository
						.All()
						.Where(u => u.Specialty != null && u.IsActive);
		}

		public IEnumerable<DentHubUser> GetAllActiveClinicDentists(int clinicId)
        {
            return this._userRepository
                     .All()
                     .Where(d => d.IsActive && d.ClinicId == clinicId)
                     .ToArray();
        }

		public DentHubUser GetDentistById(string id)
		{
			return this._userRepository
				.All()
				.Where(d => d.Specialty != null)
				.FirstOrDefault(d => d.Id == id);
		}

		public Task SaveChangesAsync()
		{
			return this._userRepository.SaveChangesAsync();
		}

		public void Update(DentHubUser dentist)
		{
			this._userRepository.Update(dentist);
		}

	}
}
