using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		public Task AddAsync(Clinic clinic)
		{
			return this._clinicRepository.AddAsync(clinic);
		}

		public IEnumerable<Clinic> GetAllActive()
        {
            return this._clinicRepository
                .All()
                .Where(c => c.IsActive && c.Dentists.Any(d => d.IsActive))
                .ToArray();
        }

		public Clinic GetClinicById(int id)
		{
			var clinic = this._clinicRepository
									.All()
									.FirstOrDefault(c => c.Id == id);
			if (clinic == null)
			{
				throw new ArgumentException("No such clinic exists");
			}
			return clinic;
		}

		public Task SaveChangesAsync()
		{
			return this._clinicRepository.SaveChangesAsync();
		}

		public void Update(Clinic clinic)
		{
			this._clinicRepository.Update(clinic);
		}
	}
}
