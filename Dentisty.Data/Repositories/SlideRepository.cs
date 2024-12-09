using Dentistry.Data.GeneratorDB.Entities;
using Dentisty.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Repositories
{
    public class SlideRepository
    {
        private readonly IRepository<Slide> _repositorySlide;
        public SlideRepository(IRepository<Slide> repositorySlide)
        {
            _repositorySlide = repositorySlide;
        }
        public async Task<Slide> GetById(int id)
        {
            var slide = await _repositorySlide.GetByIdAsync(id);
            return slide;
        }
        public async Task<IEnumerable<Slide>> GetAll()
        {
            return await _repositorySlide.GetAllAsync();
        }
        public async Task<Slide> Create(Slide slide)
        {
            await _repositorySlide.AddAsync(slide); 
            await _repositorySlide.SaveChangesAsync();
            return slide;
        }
        public async Task<Slide> Update(Slide slide)
        {
            _repositorySlide.Update(slide);
            await _repositorySlide.SaveChangesAsync();
            return slide;
        }
    }
}
