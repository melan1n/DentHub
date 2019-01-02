using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentHub.Data.Common;
using DentHub.Data.Models;

namespace DentHub.Web.Services.DataServices
{
	public class PatientService : IPatientService
	{
		private IRepository<DentHubUser> _userRepository;

		public PatientService(IRepository<DentHubUser> userRepository)
		{
			this._userRepository = userRepository;
		}

		public IEnumerable<DentHubUser> GetAllActivePatients()
		{
			return this._userRepository
						.All()
						.Where(u => u.SSN != null && u.IsActive);
		}

		public DentHubUser GetPatientById(string id)
		{
			return  this._userRepository
				.All()
				.Where(p => p.SSN != null)
				.FirstOrDefault(p => p.Id == id);
		}

		public Task SaveChangesAsync()
		{
			return this._userRepository.SaveChangesAsync();
		}

		public void Update(DentHubUser patient)
		{
			this._userRepository.Update(patient); 
		}
	}
}
