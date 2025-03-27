using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.Catalog.Doctors;
using Dentisty.Data.GeneratorDB.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Interfaces
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        Task<IEnumerable<Doctor>> GetAll();
        
        Task<Doctor> GetById(int id);
        Task<DoctorVm> GetByAlias(string alias);
        Task<DoctorVm> Create(DoctorVm doctor);
        Task<DoctorVm> Update(DoctorVm doctor);
        Task<DoctorVm> UpLoadFile(int id, IFormFile file);
        Task<IEnumerable<DoctorVm>> GetDoctorByIds(string ids);
        Task<IEnumerable<DoctorVm>> GetDoctorForAppSettings();
        Task<IEnumerable<DoctorVm>> GetDoctorForApplication();
        Task<bool> CheckExistsAlias(string alias, int id);
    }
}
