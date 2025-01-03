
using System;
using System.IO;
using Dentisty.Data.Common;
using FluentFTP;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Dentisty.Data.Storages
{
    public class FtpUploader
    {
        private readonly HostingConfig _config;
        private FtpClient _client;

        public FtpUploader(IOptions<HostingConfig> config)
        {
            _config = config.Value;
            _client = new FtpClient(_config.HostAddress)
            {
                Credentials = new System.Net.NetworkCredential(_config.UserFTP, _config.PassWordFTP)
            };
        }

        // Kết nối đến FTP
        private void Connect()
        {
            if (!_client.IsConnected)
            {
                _client.Connect();
            }
        }

        // Ngắt kết nối FTP
        private void Disconnect()
        {
            if (_client.IsConnected)
            {
                _client.Disconnect();
            }
        }

        // Kiểm tra và tạo thư mục nếu chưa tồn tại
        private void CreateDirectoryIfNotExists(string remoteDirectory)
        {
            Connect();

            // Kiểm tra xem thư mục đã tồn tại trên server chưa
            if (!_client.DirectoryExists(remoteDirectory))
            {
                _client.CreateDirectory(remoteDirectory);
            }

            Disconnect();
        }

        // Upload ảnh lên FTP
        public string UploadImage(string localFilePath, string remoteDirectory)
        {
            try
            {
                // Tạo thư mục trên server nếu chưa tồn tại
                CreateDirectoryIfNotExists(remoteDirectory);

                // Lấy tên file từ đường dẫn local
                string remoteFilePath = Path.Combine(remoteDirectory, Path.GetFileName(localFilePath));

                Connect();

                // Upload ảnh lên server
                _client.UploadFile(localFilePath, remoteFilePath, FtpRemoteExists.Overwrite);

                // Trả về URL của ảnh sau khi upload
                string imageUrl = $"{_config.WebHost}/{remoteFilePath.Replace("\\", "/")}";

                Disconnect();

                return imageUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                return null;  // Hoặc ném lỗi ra ngoài
            }
        }

        public string UploadImage(IFormFile file, string? remoteDirectory)
        {
            try
            {
                if (string.IsNullOrEmpty(remoteDirectory))
                {
                    remoteDirectory = _config.UploadDirectory;
                }
                // Kiểm tra file hợp lệ
                if (file == null || file.Length <= 0)
                {
                    throw new ArgumentException("File không hợp lệ.");
                }

                string fullRemoteDirectory = "wwwroot/" + remoteDirectory;
                // Tạo thư mục trên server nếu chưa tồn tại
                CreateDirectoryIfNotExists(fullRemoteDirectory);

                // Lấy tên file
                string fileName = Path.GetFileName(file.FileName);
                // file path upload
                string remoteFilePath = Path.Combine(fullRemoteDirectory, fileName);
                // file path return
                string returnFilePath = Path.Combine(remoteDirectory, fileName);

                // Lưu file tạm thời
                string tempFilePath = Path.GetTempFileName();
                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                Connect();

                // Upload file từ tạm lên FTP
                _client.UploadFile(tempFilePath, remoteFilePath, FtpRemoteExists.Overwrite);

                // Trả về URL file
                string imageUrl = $"{_config.WebHost}/{returnFilePath.Replace("\\", "/")}";

                // Xóa file tạm
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }

                Disconnect();

                return imageUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                return null;
            }
        }
    }

}



