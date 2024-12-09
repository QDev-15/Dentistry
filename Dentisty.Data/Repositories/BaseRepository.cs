using Dentistry.Data.GeneratorDB.Entities;
using Dentisty.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Repositories
{
    public class BaseRepository
    {
        private readonly IRepository<Base> _repositoryBase;

        public BaseRepository(IRepository<Base> repositoryBase)
        {
            _repositoryBase = repositoryBase;
        }

        public async Task<IEnumerable<Base>> GetAll()
        {
            var bases = await _repositoryBase.GetAllAsync();
            return bases;
        }

        public async Task<Base> GetById(int id)
        {
            var item = await _repositoryBase.GetByIdAsync(id);
            return item;
        }
        
    }
}
