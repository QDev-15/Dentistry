using Dentistry.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Dac;

namespace Dentisty.Data.Storages
{
    public class DatabaseBackupFileInfo
    {
        public string FileName { get; set; } = "";
        public long FileSizeBytes { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class DatabaseBackupService
    {
        private readonly string _connectionString;

        public DatabaseBackupService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(SystemConstants.MainConnectionString)
                ?? throw new InvalidOperationException("Không tìm thấy connection string DentistryDbConnection.");
        }

        public DatabaseBackupFileInfo CreateBackup(string backupDirectory)
        {
            Directory.CreateDirectory(backupDirectory);

            var builder = new SqlConnectionStringBuilder(_connectionString);
            var dbName = builder.InitialCatalog;
            var fileName = $"{dbName}_{DateTime.Now:yyyyMMdd_HHmmss}.bacpac";
            var filePath = Path.Combine(backupDirectory, fileName);

            var dacServices = new DacServices(_connectionString);
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
            {
                dacServices.ExportBacpac(stream, dbName);
            }

            var fileInfo = new FileInfo(filePath);
            return new DatabaseBackupFileInfo
            {
                FileName = fileName,
                FileSizeBytes = fileInfo.Length,
                CreatedDate = fileInfo.CreationTime
            };
        }

        public List<DatabaseBackupFileInfo> ListBackups(string backupDirectory)
        {
            if (!Directory.Exists(backupDirectory))
            {
                return new List<DatabaseBackupFileInfo>();
            }

            return Directory.GetFiles(backupDirectory, "*.bacpac")
                .Select(path => new FileInfo(path))
                .OrderByDescending(f => f.CreationTime)
                .Select(f => new DatabaseBackupFileInfo
                {
                    FileName = f.Name,
                    FileSizeBytes = f.Length,
                    CreatedDate = f.CreationTime
                })
                .ToList();
        }

        public bool DeleteBackup(string backupDirectory, string fileName)
        {
            var safeName = Path.GetFileName(fileName);
            var filePath = Path.Combine(backupDirectory, safeName);
            if (!File.Exists(filePath))
            {
                return false;
            }
            File.Delete(filePath);
            return true;
        }
    }
}
