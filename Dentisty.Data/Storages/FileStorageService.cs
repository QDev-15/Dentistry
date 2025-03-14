using Dentistry.Common;
using Dentistry.ViewModels.Common;
using Dentisty.Common;
using Dentisty.Data.Repositories;
using Dentisty.Data.Storages;
using FluentFTP;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace Dentistry.Data.Storages
{
    public class FileStorageService : IStorageService
    {
        private readonly string _userContentFolder;
        private readonly LoggerRepository logger;
        private readonly IConfiguration _configuration;
        private readonly HostingConfig _config;

        private readonly FtpUploader _ftpUploader;

        private readonly string Content_folder = SystemConstants.USER_CONTENT_FOLDER_NAME;

        public FileStorageService(IWebHostEnvironment webHostEnvironment, IOptions<HostingConfig> config, IConfiguration configuration, LoggerRepository loggerRepository)
        {
            _ftpUploader = new FtpUploader(config);
            _config = config.Value;
            _configuration = configuration;
            logger = loggerRepository;
            _userContentFolder = Path.Combine(webHostEnvironment.WebRootPath, SystemConstants.USER_CONTENT_FOLDER_NAME);
        }

        public string GetFileUrl(string fileName)
        {
            return $"{_config.Domain}/{_config.UploadDirectory}/{fileName}";
        }
       
        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName)
        {
            try
            {
                var filePath = Path.Combine(_userContentFolder, fileName);

                if (!Directory.Exists(_userContentFolder))
                {
                    Directory.CreateDirectory(_userContentFolder); // Ensure folder exists
                }
                using (var output = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await mediaBinaryStream.CopyToAsync(output);
                }
                //using var output = new filestream(filepath, filemode.create);
                //await mediabinarystream.copytoasync(output);
            }
            catch (Exception ex)
            {
                logger.Add(ex.Message, "Upload file Error, fileName: " + fileName);
                // Log the error or rethrow as needed
                throw new IOException($"An error occurred while saving the file {fileName}.", ex);
            }
        }          
        public async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file.Length > 10485760) // Limit: 10 MB
                throw new ArgumentException("File size exceeds the limit of 10 MB.");

            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName!.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await SaveFileAsync(file.OpenReadStream(), fileName);
            
            return fileName;
        }

        public async Task<FileUploadResult> SaveFileToHostingAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File không hợp lệ.");

            // Kiểm tra định dạng file
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!_config.AllowedExtensions.Contains(extension))
            {
                throw new ArgumentException("Loại file không được phép.");
            }

            // Kiểm tra kích thước file
            var maxFileSize = _config.MaxFileSizeMB * 1024 * 1024; // MB to bytes
            if (file.Length > maxFileSize)
            {
                throw new ArgumentException("Kích thước file vượt quá giới hạn cho phép.");
            }

            try
            {
                var fileName = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + file.FileName;
                // Upload ảnh lên FTP và lấy URL
                string imageUrl = _ftpUploader.UploadImage(file, _config.UploadDirectory);

                return new FileUploadResult() { FileName = fileName, FilePath = imageUrl, FileSize = file.Length };
            }
            catch (Exception ex)
            {
                logger.QueueLog(ex.Message, "Upload file");
                return null;
            }
        }
        public async Task<FileUploadResult> SaveFileToHostingAsync(IFormFile file, string remoteDirectory)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File không hợp lệ.");

            // Kiểm tra định dạng file
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!_config.AllowedExtensions.Contains(extension))
            {
                throw new ArgumentException("Loại file không được phép.");
            }

            // Kiểm tra kích thước file
            var maxFileSize = _config.MaxFileSizeMB * 1024 * 1024; // MB to bytes
            if (file.Length > maxFileSize)
            {
                throw new ArgumentException("Kích thước file vượt quá giới hạn cho phép.");
            }

            try
            {
                var fileName = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + file.FileName;
                // Upload ảnh lên FTP và lấy URL
                string imageUrl = _ftpUploader.UploadImage(file, remoteDirectory);

                return new FileUploadResult() { FileName = fileName, FilePath = imageUrl, FileSize = file.Length };
            }
            catch (Exception ex)
            {
                logger.QueueLog(ex.Message, "Upload file");
                return null;
            }
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }

        public bool DeleteFileToHostingAsync(string urlImage)
        {
            // Xóa file từ URL
            bool isDeleted = _ftpUploader.DeleteFileFromUrl(urlImage);
            return isDeleted;
        }
    }
}
