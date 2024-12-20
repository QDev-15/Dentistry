using Dentistry.Common.Constants;
using Dentisty.Data.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Http.Headers;

namespace Dentistry.Data.Storages
{
    public class FileStorageService : IStorageService
    {
        private readonly string _userContentFolder;
        private readonly string _hostAdmin;
        private readonly LoggerRepository logger;
        private readonly IConfiguration _configuration;

        private readonly string Content_folder = SystemConstants.USER_CONTENT_FOLDER_NAME;

        public FileStorageService(IWebHostEnvironment webHostEnvironment, IConfiguration configuration, LoggerRepository loggerRepository)
        {

            _configuration = configuration;
            logger = loggerRepository;
            _userContentFolder = Path.Combine(webHostEnvironment.WebRootPath, SystemConstants.USER_CONTENT_FOLDER_NAME);
        }

        public string GetFileUrl(string fileName)
        {
            return $"{_configuration["hostAdmin"]}/{Content_folder}/{fileName}";
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
                //using var output = new FileStream(filePath, FileMode.Create);
                //await mediaBinaryStream.CopyToAsync(output);
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

        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
    }
}
