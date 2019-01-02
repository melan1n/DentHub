using DentHub.Data.Models;
using System;
using System.Collections.Generic;

namespace DentHub.Web.Services.DataServices
{
    public interface IClinicService
    {
        IEnumerable<Clinic> GetAllActive();

        //bool IsCategoryIdValid(int categoryId);

        //int? GetCategoryId(string name);
    }
}
