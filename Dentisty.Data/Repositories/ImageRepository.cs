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
        private readonly IHttpClientFactory _httpClientFactory;

        public ImageRepository(DentistryDbContext context, IConfiguration configuration, IStorageService storageService, LoggerRepository loggerRepository, IHttpClientFactory httpClientFactory) : base(context)
        {
            _loggerRepository = loggerRepository;
            _context = context;
            _storageService = storageService;
            _httpClientFactory = httpClientFactory;
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
                    ThumbPath = fileUpload.ThumbPath,
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
                    ThumbPath = fileUpload.ThumbPath,
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

        public async Task<ImageFile> CreateFromUrlAsync(string imageUrl, string directory)
        {
            if (string.IsNullOrWhiteSpace(imageUrl) ||
                !Uri.TryCreate(imageUrl, UriKind.Absolute, out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                throw new ArgumentException("URL ảnh không hợp lệ.");
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                client.Timeout = TimeSpan.FromSeconds(15);
                using var response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                var contentType = response.Content.Headers.ContentType?.MediaType ?? "";
                if (!contentType.StartsWith("image/"))
                {
                    throw new ArgumentException("Đường dẫn không phải là ảnh hợp lệ.");
                }

                var bytes = await response.Content.ReadAsByteArrayAsync();
                if (bytes.Length == 0 || bytes.Length > 10 * 1024 * 1024)
                {
                    throw new ArgumentException("Ảnh không hợp lệ hoặc vượt quá dung lượng cho phép (10MB).");
                }

                // Ảnh cuối cùng luôn được lưu jpg (trừ png/gif giữ nguyên) qua UploadImageV2,
                // nên webp/các định dạng khác cũng gắn đuôi .jpg để qua được kiểm tra AllowedExtensions.
                var extension = contentType switch
                {
                    "image/png" => ".png",
                    "image/gif" => ".gif",
                    _ => ".jpg"
                };
                var fileName = $"{Guid.NewGuid()}{extension}";

                using var stream = new MemoryStream(bytes);
                var formFile = new FormFile(stream, 0, stream.Length, "file", fileName)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = contentType
                };

                var fileUpload = await _storageService.SaveFileToHostingAsync(formFile, directory);
                var image = new ImageFile()
                {
                    FileSize = bytes.Length,
                    Type = contentType,
                    FileName = fileUpload.FileName,
                    Path = fileUpload.FilePath,
                    ThumbPath = fileUpload.ThumbPath,
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
                bool vThumb = _storageService.DeleteFileToHostingAsync(image.ThumbPath);
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
                await Task.Run(() => DeleteFileToHostingAsync(image));
                //await _storageService.DeleteFileAsync($"{image.Path}");
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
                    await Task.Run(() => DeleteFileToHostingAsync(image));
                    //await Task.Run(() => _storageService.DeleteFileToHostingAsync($"{image.Path}"));
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
