using Dentistry.Data.GeneratorDB.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Interfaces
{
    public interface IImageRepository : IRepository<Image>
    {
        Task<Image> CreateAsync(IFormFile file);
    }
}
