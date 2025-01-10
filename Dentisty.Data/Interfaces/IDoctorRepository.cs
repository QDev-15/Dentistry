using Dentistry.ViewModels.Catalog.Doctors;
using Dentisty.Data.GeneratorDB.Entities;
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
        Task<DoctorVm> Create(DoctorVm doctor);
        Task<DoctorVm> Update(DoctorVm doctor);
        Task<IEnumerable<DoctorVm>> GetDoctorForAppSettings();
        Task<IEnumerable<DoctorVm>> GetDoctorForApplication();
    }
}
