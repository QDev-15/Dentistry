using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog;
using Dentistry.ViewModels.Catalog.Slide;
using Dentisty.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

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
                    SubName = slideVm.SubName,
                    Description = slideVm.Description,
                    Url = slideVm.Url,
                    SortOrder = slideVm.SortOrder,
                    IsActive = slideVm.IsActive,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    UserId = new Guid(_logs.GetCurrentUserId() ?? "")
                };
                if (slideVm.ImageFile != null)
                {
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
                    slide.Image = image;
                }
                // set image slide
                await AddAsync(slide);
                await SaveChangesAsync();       
                // update imageId
                slide.ImageId = slide.Image.Id;
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
        public async Task<SlideVm> UpdateSlide(SlideVm slideVm)
        {
            try
            {
                var slide = await GetByIdAsync(slideVm.Id);
                if (slide == null)
                {
                    return slideVm;
                }
                var imageOld = slide.Image;
                if (slideVm.ImageFile != null)
                {
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
                    
                    slide.Image = image;
                }
                // set image slide
                Update(slide);
                await SaveChangesAsync();       
                // update imageId
                slide.ImageId = slide.Image.Id;
                await SaveChangesAsync();
                // delete oldImage
                if (imageOld != null)
                {
                    await _imageRepository.Delete(imageOld);
                    await SaveChangesAsync();
                }

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

        public async Task<bool> Delete(int id)
        {
            try
            {
                var slide = await GetByIdAsync(id);
                if (slide != null) {
                    if (slide.Image != null) { 
                        await _imageRepository.Delete(slide.Image);
                        slide.ImageId = null;
                    }
                    Delete(slide);
                    await SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex) {
                _logs.QueueLog(ex.Message);
                return false;
            }
            
        }
    }
}
