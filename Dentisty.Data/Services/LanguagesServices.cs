using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.System.Languages;
using Dentisty.Data.Interfaces;
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
            var languges = await _languagesRepository.GetAllAsync();

            return languges;
        }
    }
}
