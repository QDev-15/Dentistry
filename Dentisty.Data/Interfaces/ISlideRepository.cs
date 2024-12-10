using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Utilities.Slides;
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
    }
}
