using System.ComponentModel;

namespace KryptConsole
{
    public static class BackgroundWorkerHelpers
    {
        public static BackgroundWorker CreateBackgroundWorker()
        {
            var backgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            backgroundWorker.ProgressChanged += ReportProgress;

            return backgroundWorker;
        }
        private static void ReportProgress(object? sender, ProgressChangedEventArgs e)
        {
            var (Left, Top) = Console.GetCursorPosition();
            var progress = $"Progress: {e.ProgressPercentage}%";

            Console.Write(progress);
            if (Top > 0)
            {
                Console.SetCursorPosition(Left, Top - 1);
            }
            else
            {
                Console.SetCursorPosition(Left, Top);
            }
        }
    }
}