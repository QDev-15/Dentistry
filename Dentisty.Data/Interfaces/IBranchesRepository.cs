using Dentistry.ViewModels.Catalog.Branches;
using Dentisty.Data.GeneratorDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Interfaces
{
    public interface IBranchesRepository : IRepository<Branches>
    {
        Task<Branches> CreateNew(BranchesVm model);
        Task<Branches> Update(BranchesVm model);
        Task<bool> ActiveBranches(int id);
        Task<bool> DeActiveBranches(int id);
    }
}
