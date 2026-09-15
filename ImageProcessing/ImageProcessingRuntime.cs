namespace ImageProcessing
{
    /// <summary>
    /// Process-wide tuning knobs for hosts that call into this library under their own
    /// concurrency (a web app serving concurrent requests, a batch worker with its own task
    /// pool, ...).
    /// </summary>
    public static class ImageProcessingRuntime
    {
        /// <summary>
        /// Sets how many worker threads the underlying OpenCV routines (Resize, MedianBlur,
        /// morphology, ...) may use internally. Left at OpenCV's own default, each of a host's
        /// own concurrent worker threads ALSO fans out to OpenCV's internal pool, oversubscribing
        /// the CPU once the host already runs several of these calls at once (e.g. one per
        /// concurrent web request) - this was measured to roughly halve throughput under load
        /// (see the ImageProcessing.Benchmarks concurrency section). Call
        /// <c>SetWorkerThreads(1)</c> once at host startup when you drive your own concurrency
        /// (a web app, a Parallel.For batch job); leave the default only for a single-threaded,
        /// one-image-at-a-time host.
        /// </summary>
        public static void SetWorkerThreads(int threadCount)
        {
            OpenCvSharp.Cv2.SetNumThreads(threadCount);
        }

        /// <summary>The worker-thread count OpenCV routines currently use internally.</summary>
        public static int GetWorkerThreads()
        {
            return OpenCvSharp.Cv2.GetNumThreads();
        }
    }
}
