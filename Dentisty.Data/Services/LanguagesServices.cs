using Dentistry.Data.GeneratorDB.Entities;
using Dentisty.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Services
{
    public class LanguagesServices
    {
        private readonly IRepository<Language> _languagesRepository;
        public LanguagesServices(IRepository<Language> languageRepository)
        {
            _languagesRepository = languageRepository;
        }
        public async Task<IEnumerable<Language>> GetAllLanguagesAsync()
        {
            return await _languagesRepository.GetAllAsync();
        }
    }
}
