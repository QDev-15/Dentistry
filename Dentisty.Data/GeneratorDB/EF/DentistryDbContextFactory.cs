using Dentistry.Common.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.Data.GeneratorDB.EF
{
    public class DentistryDbContextFactory : IDesignTimeDbContextFactory<DentistryDbContext>
    {
        public DentistryDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString(Constants.MainConnectionString);

            var optionsBuilder = new DbContextOptionsBuilder<DentistryDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new DentistryDbContext(optionsBuilder.Options);
        }
    }
}
