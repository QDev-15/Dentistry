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
        /// Create Image entity, need call to savechage
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<Image> CreateAsync(IFormFile file);
        /// <summary>
        /// Create Image entity in directory
        /// </summary>
        /// <param name="file">IFormFile need to save</param>
        /// <param name="directory">Save to folder</param>
        /// <returns></returns>
        Task<Image> CreateAsync(IFormFile file, string directory);
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
