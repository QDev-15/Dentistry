using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.Data.Storages;
using Dentistry.ViewModels.Catalog;
using Dentistry.ViewModels.Utilities.Slides;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dentistry.Data.Services
{
    public class SlideService
    {
        private readonly SlideRepository _slideRepository;
        private readonly ImageRepository _imageRepository;
        private readonly LoggerRepository logger;

        public SlideService(SlideRepository slideRepository, ImageRepository imageRepository, LoggerRepository logger)
        {
            _slideRepository = slideRepository;
            _imageRepository = imageRepository;
            this.logger = logger;
        }

        public async Task<IEnumerable<SlideVm>> GetAll()
        {
            var slides = await _slideRepository.GetAll();
                
            return slides.Select(x => new SlideVm()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Image = new ImageVm
                {
                    Id = x.Image.Id,
                    FileName = x.Image.FileName,
                    FileSize = x.Image.FileSize,
                    Path = x.Image.Path,
                    Type = x.Image.Type 
                }
            });
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
                    
                };
                var image = await _imageRepository.Create(slideVm.ImageFile);
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
                slide = await _slideRepository.Create(slide);
                // set slide id
                slideVm.Id = slide.Id;
                return slideVm;
            }
            catch (Exception ex)
            {
                logger.Add(ex.Message);
                return null;
            }
            
            
            return slideVm;
        }
    }
}