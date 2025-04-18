﻿using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog.Slide;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Interfaces
{
    public interface ISlideRepository : IRepository<Slide>
    {
        Task<SlideVm> Create(SlideVm slideVm);
        Task<SlideVm> UpdateSlide(SlideVm slideVm);
        Task<SlideVm> UpLoadFile(int id, IFormFile file);
        Task<List<SlideVm>> GetActiveSlides();
        Task<bool> Delete(int id);
    }
}
