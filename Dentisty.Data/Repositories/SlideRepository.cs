using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog;
using Dentistry.ViewModels.Utilities.Slides;
using Dentisty.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Repositories
{
    public class SlideRepository : Repository<Slide>, ISlideRepository
    {
        private readonly DentistryDbContext _context;
        private readonly ImageRepository _imageRepository;
        private readonly LoggerRepository _logs;

        public SlideRepository(DentistryDbContext context, LoggerRepository logs, ImageRepository imageRepository) : base(context)
        {
            _logs = logs;
            _context = context;
            _imageRepository = imageRepository;
        }
        public async Task<IEnumerable<Slide>> GetAllAsync()
        {
            return await _context.Slides.Include(i => i.Image).ToListAsync();
        }
        public async Task<Slide> GetByIdAsync(int id)
        {
            return await _context.Slides.Include(i => i.Image).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<SlideVm> Create(SlideVm slideVm)
        {
            try
            {
                var slide = new Slide()
                {
                    Name = slideVm.Name,
                    IsActive = slideVm.IsActive,
                    Description = slideVm.Description,
                    CreatedDate = DateTime.Now,
                    UserId = new Guid(_logs.GetCurrentUserId() ?? "")
                };
                var image = await _imageRepository.CreateAsync(slideVm.ImageFile);
                // set image to slideVm
                slideVm.Image = new ImageVm()
                {
                    Id = image.Id,
                    FileName = image.FileName,
                    Type = image.Type,
                    Path = image.Path,
                    FileSize = image.FileSize,
                };
                // set image slide
                slide.Image = image;
                slide.ImageId = image.Id;
                await AddAsync(slide);
                await SaveChangesAsync();
                // set slide id
                slideVm.Id = slide.Id;
                slideVm.Image.Id = slide.Image.Id;
                return slideVm;
            }
            catch (Exception ex)
            {
                _logs.QueueLog(ex.Message);
                return new SlideVm();
            }
        }
    }
}
