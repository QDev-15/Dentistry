using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.Data.Interfaces;
using Dentistry.ViewModels.Utilities.Slides;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.Data.Services
{
    public class SlideService
    {
        private readonly IRepository<Slide> _repository;
        private readonly DentistryDbContext _context;

        public SlideService(DentistryDbContext context, IRepository<Slide> repository)
        {
            _context = context;
            _repository = repository;
        }

        public async Task<IEnumerable<SlideVm>> GetAll()
        {
            var slides = await _context.Slides.OrderBy(x => x.SortOrder)
                .Select(x => new SlideVm()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Url = x.Url,
                    ImageId = x.Image.Id
                }).ToListAsync();

            return slides;
        }
    }
}