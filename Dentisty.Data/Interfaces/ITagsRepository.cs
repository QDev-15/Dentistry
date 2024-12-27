using Dentisty.Data.GeneratorDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Interfaces
{
    public interface ITagsRepository : IRepository<Tags>
    {
        Task<List<Tags>> GetAll();
    }
}
