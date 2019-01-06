using DentHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DentHub.Web.Services.DataServices
{
    public interface IClinicService
    {
        IEnumerable<Clinic> GetAllActive();

        Clinic GetClinicById(int id);

		Task AddAsync(Clinic clinic);

		Task SaveChangesAsync();

		void Update(Clinic clinic);
	}
}
