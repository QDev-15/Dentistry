using Dentistry.Data.GeneratorDB.Entities;

namespace Dentistry.Data.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<AppRole>> GetAll();
    }
}