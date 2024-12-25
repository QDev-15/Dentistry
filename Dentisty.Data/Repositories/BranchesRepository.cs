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
                Update(branch);
                await SaveChangesAsync();
                return true;
            }
            catch (Exception ex) { 
                _loggerRepository.QueueLog(ex.Message, "Active Branches by Id: " + id);
                return false;            
            }
        }

        public Task<Branches> CreateNew(BranchesVm model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeActiveBranches(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Branches> Update(BranchesVm model)
        {
            throw new NotImplementedException();
        }
    }
}
