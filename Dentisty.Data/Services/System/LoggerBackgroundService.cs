using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentisty.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Services.System
{
    public class LoggerBackgroundService : BackgroundService
    {
        private readonly Logs _logQueue;
        private readonly IServiceProvider _serviceProvider;

        public LoggerBackgroundService(Logs logQueue, IServiceProvider serviceProvider)
        {
            _logQueue = logQueue;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var log = await _logQueue.DequeueAsync(stoppingToken);

                if (log != null)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<DentistryDbContext>();

                    try
                    {
                        dbContext.Set<Logger>().Add(log);
                        await dbContext.SaveChangesAsync(stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error saving log: {ex.Message}");
                    }
                }
            }
        }
    }
}
