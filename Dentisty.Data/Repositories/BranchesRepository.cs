using Dentistry.Data.GeneratorDB.EF;
using Dentistry.ViewModels.Catalog.Branches;
using Dentisty.Data.GeneratorDB.Entities;
using Dentisty.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Repositories
{
    public class BranchesRepository : Repository<Branches>, IBranchesRepository
    {
        private readonly DentistryDbContext _dbContext;
        private readonly LoggerRepository _loggerRepository;
        public BranchesRepository(DentistryDbContext context, LoggerRepository loggerRepository) : base(context)
        {
            _dbContext = context;
            _loggerRepository = loggerRepository;   
        }

        public async Task<bool> ActiveBranches(int id)
        {
            try
            {
                var branch = await GetByIdAsync(id);
                if (branch == null)
                {
                    throw new Exception("not found branches by id: " + id);
                }
                branch.IsActive = true;
                UpdateAsync(branch);
                await SaveChangesAsync();
                return true;
            }
            catch (Exception ex) { 
                _loggerRepository.QueueLog(ex.Message, "Active Branches by Id: " + id);
                return false;            
            }
        }

        public async Task<Branches> CreateNew(BranchesVm model)
        {
            try
            {
                var newBranches = new Branches()
                {
                    Address = model.Address,
                    Code = model.Code,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true,
                    Name = model.Name
                };
                await AddAsync(newBranches);
                await SaveChangesAsync();
                return newBranches;
            }
            catch (Exception ex) {
                _loggerRepository.QueueLog(ex.Message, "Create new Branches");
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeActiveBranches(int id)
        {
            try
            {
                var branch = await GetByIdAsync(id);
                if (branch != null)
                {
                    branch.IsActive = false;
                    UpdateAsync(branch);
                    await SaveChangesAsync();
                    return true;
                }
                throw new Exception("Branches not found by id " + id);
            }
            catch (Exception ex) {
                _loggerRepository.QueueLog(ex.Message, "DeActive Branches");
                throw new Exception(ex.Message);
            }

        }

        public async Task<Branches> Update(BranchesVm model)
        {
            try
            {
                var branches = await GetByIdAsync(model.Id);
                if (branches != null) {
                    throw new Exception("Branches not found by id " + model.Id);
                }
                branches.Name = model.Name;
                branches.UpdatedAt = DateTime.UtcNow;
                branches.Code = model.Code;
                branches.Address = model.Address;
                branches.IsActive = model.IsActive;
                UpdateAsync(branches);
                await SaveChangesAsync();


                return branches;
            } catch(Exception ex)
            {
                _loggerRepository.QueueLog(ex.Message, "Update Branchese");
                throw new Exception(ex.Message);
            }
        }
    }
}
