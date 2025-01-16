using Dentistry.Common.Constants;
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
        private readonly IImageRepository _imageRepository;
        private readonly LoggerRepository _logs;

        public SlideRepository(DentistryDbContext context, LoggerRepository logs, IImageRepository imageRepository) : base(context)
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
                    var image = await _imageRepository.CreateAsync(slideVm.ImageFile, SystemConstants.Folder.Slides);
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
                return slide.ReturnViewModel();
            }
            catch (Exception ex)
            {
                _logs.QueueLog(ex.Message);
                throw new Exception(ex.Message);
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
                    var image = await _imageRepository.CreateAsync(slideVm.ImageFile, SystemConstants.Folder.Slides);
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
                    // delete oldImage
                    if (imageOld != null)
                    {
                        _imageRepository.DeleteFileToHostingAsync(imageOld);
                        _imageRepository.DeleteAsync(imageOld);
                    }
                }
                slide.SortOrder = slideVm.SortOrder;
                slide.Name = slideVm.Name;
                slide.SubName = slideVm.SubName;
                slide.Url = slideVm.Url;
                slide.UpdatedDate = DateTime.Now;
                slide.Description = slideVm.Description;
                slide.Caption = slideVm.Caption;
                // set image slide
                UpdateAsync(slide);
                await SaveChangesAsync();   
                return slide.ReturnViewModel();
            }
            catch (Exception ex)
            {
                _logs.QueueLog(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var slide = await GetByIdAsync(id);
                if (slide != null) {
                    if (slide.Image != null) { 
                        await _imageRepository.DeleteFile(slide.Image);
                        _imageRepository.DeleteAsync(slide.Image);
                        slide.ImageId = null;
                    }
                    DeleteAsync(slide);
                    await SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex) {
                _logs.QueueLog(ex.Message);
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<List<SlideVm>> GetActiveSlides()
        {
            var slides = await _context.Slides.Where(x => x.IsActive).Include(x => x.Image).ToListAsync();
            return slides.Select(x => x.ReturnViewModel()).OrderBy(x => x.SortOrder).ToList();
        }
    }
}
