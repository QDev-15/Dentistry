using Dentistry.Data.Common.Constants;
using Dentistry.Data.GeneratorDB.EF;
using Dentistry.ViewModels.Catalog.Doctors;
using Dentisty.Data.GeneratorDB.Entities;
using Dentisty.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Repositories
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        private readonly string title = "Doctor - Repository";
        private readonly IImageRepository _imageRepository;
        private readonly DentistryDbContext _context;
        private readonly LoggerRepository _loggerRepository;
        public DoctorRepository(DentistryDbContext context, IImageRepository imageRepository, LoggerRepository loggerRepository) : base(context)
        {
            _context = context;
            _imageRepository = imageRepository;
            _loggerRepository = loggerRepository;   
        }

        public async Task<DoctorVm> Create(DoctorVm vm)
        {
            try
            {
                var doctor = new Doctor()
                {
                    Name = vm.Name,
                    Alias = vm.Alias,
                    Description = vm.Description,
                    Dob = vm.Dob,
                    Position = vm.Position,
                    PositionExtent = vm.PositionExtent,
                    Profile = vm.Profile
                };
                if (vm.formFile != null)
                {
                    var image = await _imageRepository.CreateAsync(vm.formFile, SystemConstants.Folder.Doctor);
                    doctor.Avatar = image;
                }
                await AddAsync(doctor);
                await SaveChangesAsync();
                return doctor.ReturnViewModel();
            }
            catch (Exception ex) {
                _loggerRepository.QueueLog(ex.Message, title);
                throw new Exception(ex.Message);
            }
            
        }
        public async Task<bool> CheckExistsAlias(string alias, int id)
        {
            return await _context.Categories.AnyAsync(c => c.Alias == alias && c.Id != id);
        }
        /// <summary>
        /// Include avatar
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Doctor>> GetAll()
        {
            return await _context.Doctors.Include(x => x.Avatar).ToListAsync();
        }

        /// <summary>
        /// Include avatar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Doctor> GetById(int id)
        {
            return await _context.Doctors.Include(x => x.Avatar).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<DoctorVm>> GetDoctorForApplication()
        {
            var setting = await _context.AppSettings.FirstOrDefaultAsync(x => x.Id == 1);
            if (setting == null || string.IsNullOrEmpty(setting.Doctors))
            {
                return new List<DoctorVm>();
            }
            string[] ids = setting.Doctors!.Split(',');
            var docs = await _context.Doctors.Where(x => ids.Contains(x.Id.ToString())).Include(x => x.Avatar).Select(x => x.ReturnViewModel()).ToListAsync();
            return docs;
        }

        public async Task<IEnumerable<DoctorVm>> GetDoctorForAppSettings()
        {
            return await _context.Doctors.Select(x => x.ReturnViewModel()).ToListAsync();
        }

        public async Task<DoctorVm> Update(DoctorVm vm)
        {
            try
            {
                var doctor = await GetById(vm.Id);
                if (doctor != null) { 
                    doctor.Position = vm.Position;
                    doctor.PositionExtent = vm.PositionExtent;
                    doctor.Name = vm.Name;
                    doctor.Alias = vm.Alias;
                    doctor.Description = vm.Description;
                    doctor.Dob = vm.Dob;
                    if (vm.formFile != null)
                    {
                        var img = await _imageRepository.CreateAsync(vm.formFile, SystemConstants.Folder.Doctor);
                        if (doctor.Avatar != null)
                        {
                            await _imageRepository.DeleteFile(img);
                            _imageRepository.DeleteAsync(doctor.Avatar);
                        }
                        doctor.Avatar = img;
                    }
                    UpdateAsync(doctor);
                    await SaveChangesAsync();
                }
                return doctor.ReturnViewModel();
                
            }
            catch (Exception ex) { 
                _loggerRepository.QueueLog($"{ex.Message}", title);
                throw new Exception(ex.Message);
            }
        }

        public async Task<DoctorVm> GetByAlias(string alias)
        {
            try
            {
                var doctor = await _context.Doctors.Include(x => x.Avatar).FirstOrDefaultAsync(x => x.Alias.ToLower() == alias.ToLower());
                return doctor!.ReturnViewModel();
            } catch(Exception ex)
            {
                _loggerRepository.QueueLog(ex.Message, "Doctor - GetByAlias: " + alias);
                throw new Exception("Server đang bận. Làm ơn thử lại");
            }
        }
    }
}
