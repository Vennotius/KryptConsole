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
        var cursorPosition = Console.GetCursorPosition();
        var progress = $"Progress: {e.ProgressPercentage}%";
        
        Console.Write(progress);
        if (cursorPosition.Top > 0)
        {
            Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top-1);
        }
        else
        {
            Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
        }
    }
}