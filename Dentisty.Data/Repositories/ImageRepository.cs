using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.Data.Storages;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Dentisty.Data.Repositories
{
    public class ImageRepository : Repository<ImageFile>, IImageRepository
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

        public async Task<ImageFile> CreateAsync(IFormFile file)
        {
            try
            {
                var fileUpload = await _storageService.SaveFileToHostingAsync(file);
                var image = new ImageFile()
                {
                    FileSize = file.Length,
                    Type = file.ContentType,
                    FileName = fileUpload.FileName,
                    Path = fileUpload.FilePath,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };
                await AddAsync(image);
                return image;
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<ImageFile> CreateAsync(IFormFile file, string directory)
        {
            try
            {
                var fileUpload = await _storageService.SaveFileToHostingAsync(file, directory);
                var image = new ImageFile()
                {
                    FileSize = file.Length,
                    Type = file.ContentType,
                    FileName = fileUpload.FileName,
                    Path = fileUpload.FilePath,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };
                await AddAsync(image);
                return image;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteFileToHostingAsync(ImageFile image)
        {
            try
            {
                if (image == null) return false;
                bool v = _storageService.DeleteFileToHostingAsync(image.Path);
                return v;
            }
            catch (Exception ex)
            {
                this._loggerRepository.QueueLog(ex.Message);
                throw new Exception(ex.Message);
            }
            
        }             
        public async Task<bool> DeleteFile(ImageFile image)
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
                throw new Exception(ex.Message);
            }
            
        }     
        public async Task<bool> DeleteRangeFiles(List<ImageFile> images)
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
                throw new Exception(ex.Message);
            }
           
        }
    }
}
