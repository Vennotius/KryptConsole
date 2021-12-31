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
        Console.CursorVisible = false;
        
        var cursorPosition = Console.GetCursorPosition();
        var progress = $"Progress: {e.ProgressPercentage}%";
        
        Console.Write(progress);
        Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
    }
}