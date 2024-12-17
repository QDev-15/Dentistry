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
        /// <summary>
        /// Create Image entiry, need call to savechage
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<Image> CreateAsync(IFormFile file);
        /// <summary>
        /// Delete a file in location
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        Task<bool> DeleteFile(Image image);
        /// <summary>
        /// Delete files in location
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        Task<bool> DeleteRangeFiles(List<Image> image);

    }
}
