using Dentistry.Data.GeneratorDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Interfaces
{
    public interface ICategoryReposiroty : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetByParentId(int parentId);
    }
}
