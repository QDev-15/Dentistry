using Dentisty.Data.Interfaces;
using Dentistry.Data.Storages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dentistry.Data.GeneratorDB.Entities;
using Microsoft.AspNetCore.Http;

namespace Dentisty.Data.Repositories
{
    public class ImageRepository
    {
        private readonly IRepository<Image> _imageRepository;
        private readonly IStorageService _storageService;

        public ImageRepository(IRepository<Image> imageRepository, IStorageService storageService)
        {
            _storageService = storageService;
            _imageRepository = imageRepository;
        }
        public async Task<Image> GetById(int id)
        {
            var image = await _imageRepository.GetByIdAsync(id);
            return image;
        }

        public async Task<Image> Create(IFormFile file)
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

            await _imageRepository.AddAsync(image);
            await _imageRepository.SaveChangesAsync();
            return image;
        }
        public async Task<bool> Delete(Image image)
        {
            await _storageService.DeleteFileAsync($"{image.Path}");
            _imageRepository.Delete(image);
            await _imageRepository.SaveChangesAsync();
            return true;
        }     
        public async Task<bool> DeleteRange(List<Image> images)
        {
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync($"{image.Path}");
            }
            _imageRepository.DeleteRange(images);
            await _imageRepository.SaveChangesAsync();
            return true;
        }
    }
}
