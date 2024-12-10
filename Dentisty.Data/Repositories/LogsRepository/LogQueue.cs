using Dentistry.Data.GeneratorDB.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Repositories
{
    public class Logs
    {
        private readonly ConcurrentQueue<Logger> _logQueue = new();
        private readonly SemaphoreSlim _signal = new(0);

        public void Enqueue(Logger log)
        {
            _logQueue.Enqueue(log);
            _signal.Release();
        }

        public async Task<Logger?> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);

            if (_logQueue.TryDequeue(out var log))
            {
                return log;
            }

            return null;
        }


    }
}
