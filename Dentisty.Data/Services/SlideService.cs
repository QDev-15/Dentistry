using Azure.Core;
using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.Data.Interfaces;
using Dentistry.Data.Storages;
using Dentistry.ViewModels.Utilities.Slides;
using Dentisty.Data.Repositories;
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
        private readonly IStorageService _storageService;

        public SlideService(DentistryDbContext context, IRepository<Slide> repository, IStorageService storageService)
        {
            _storageService = storageService;
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
        public async Task<SlideVm> Create(SlideVm slideVm)
        {
            var slide = new Slide()
            {
                Name = slideVm.Name,
                Active = slideVm.Active,
                Description = slideVm.Description,
                CreatedDate = DateTime.Now,

                //DateCreated = DateTime.Now,
                //FileSize = request.ThumbnailImage.Length,
                //ImagePath = await this.SaveFile(request.ThumbnailImage),
                //IsDefault = true,
                //SortOrder = 1
            };
            var image = new Image()
            {
                CreatedDate = DateTime.Now,
                FileSize = slideVm.Image.Length,
                Path = await _storageService.SaveFileAsync(slideVm.Image),
            };
            _context.Images.Add(image);
            await _context.SaveChangesAsync();
            slide.Image = image;
            slide.ImageId = image.Id;
            await _repository.AddAsync(slide);
            slideVm.Id = slide.Id;
            return slideVm;
        }
    }
}