using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.Data.Storages;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Dentisty.Data.Repositories
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        private readonly DentistryDbContext _context;
        private readonly IStorageService _storageService;
        private readonly LoggerRepository _loggerRepository;

        public ImageRepository(DentistryDbContext context, IConfiguration configuration, IStorageService storageService, LoggerRepository loggerRepository) : base(context)
        {
            _loggerRepository = loggerRepository;
            _context = context;
            _storageService = storageService;
        }

        public async Task<Image> CreateAsync(IFormFile file)
        {
            try
            {
                var _fileName = await _storageService.SaveFileAsync(file);
                var image = new Image()
                {
                    FileSize = file.Length,
                    Type = file.ContentType,
                    FileName = _fileName,
                    Path = _storageService.GetFileUrl(_fileName),
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };
                await AddAsync(image);
                return image;
            } catch(Exception ex)
            {
                return null;
            }
            
        }
        public async Task<bool> DeleteFile(Image image)
        {
            try
            {
                if (image == null) return false;
                await _storageService.DeleteFileAsync($"{image.Path}");
                return true;
            }
            catch (Exception ex)
            {
                this._loggerRepository.QueueLog(ex.Message);
                return false;
            }
            
        }     
        public async Task<bool> DeleteRangeFiles(List<Image> images)
        {
            try
            {
                foreach (var image in images)
                {
                    await _storageService.DeleteFileAsync($"{image.Path}");
                }
                return true;
            }
            catch (Exception ex)
            {
                this._loggerRepository.QueueLog(ex.Message);
                return false;
            }
           
        }
    }
}
