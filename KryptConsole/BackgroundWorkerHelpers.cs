using System.ComponentModel;

public static class BackgroundWorkerHelpers
{
    public static BackgroundWorker CreateBackgroundWorker()
    {
        var backgroundWorker = new BackgroundWorker();
        backgroundWorker.WorkerReportsProgress = true;
        backgroundWorker.ProgressChanged += ReportProgress;

        return backgroundWorker;
    }
    private static void ReportProgress(object? sender, ProgressChangedEventArgs e)
    {
        Console.WriteLine($"Pass {e.ProgressPercentage}/8.");
    }
}