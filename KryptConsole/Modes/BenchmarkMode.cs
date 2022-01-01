using Krypt2Library;
using System.Diagnostics;
using System.Text;

internal class BenchmarkMode : IMode
{
    private string _justALongString = "";
    private readonly Stopwatch _stopwatch = new();
    private string _results = "";

    public BenchmarkMode()
    {
        
    }

    private static string GenerateLongString(int length)
    {
        var output = new StringBuilder();

        for (int i = 0; i < length/10; i++)
        {
            output.Append("0123456789");
        }

        return output.ToString();
    }

    public void Run()
    {
        Console.CursorVisible = false;
        Console.WriteLine();

        RunBenchmarks();

        Console.WriteLine($"\n{_results}");

        Console.CursorVisible = true;
    }

    private void RunBenchmarks()
    {
        Console.WriteLine("Benchmarking Shuffle...\n");
        BenchmarkForShuffle();

        Console.WriteLine("\n\nBenchmarking Shift...\n");
        BenchmarkForShift();
    }

    private void BenchmarkForShuffle()
    {
        _justALongString = GenerateLongString(1000);

        var backgroundWorker = BackgroundWorkerHelpers.CreateBackgroundWorker();
        backgroundWorker.ProgressChanged += ReportTimeRemaining;

        var kryptor = new Kryptor(new Betor(Betor.EncryptCharacterUsingShuffledAlphabet, Betor.DecryptCharacterUsingShuffledAlphabet), backgroundWorker);

        kryptor.Encrypt("benchmark", _justALongString);
        _results += ReportResults("Shuffle");
    }

    private void BenchmarkForShift()
    {
        _justALongString = GenerateLongString(2000000);

        var backgroundWorker = BackgroundWorkerHelpers.CreateBackgroundWorker();
        backgroundWorker.ProgressChanged += ReportTimeRemaining;

        var kryptor = new Kryptor(new Betor(Betor.EncryptCharacterUsingShift, Betor.DecryptCharacterUsingShift), backgroundWorker);

        kryptor.Encrypt("benchmark", _justALongString);
        _results += ReportResults("Shift");
    }

    private string ReportResults(string cipher)
    {
        var totalChars = _justALongString.Length;
        var time = _stopwatch.Elapsed.TotalSeconds;
        var rate = _justALongString.Length / _stopwatch.Elapsed.TotalSeconds;

        _stopwatch.Reset();
        
        return $"\nResults for {cipher}:\n{totalChars} characters in {time:0.00} seconds = {rate:0} characters/second.\n";
    }

    private void ReportTimeRemaining(object? sender, System.ComponentModel.ProgressChangedEventArgs e)
    {
        if (e.ProgressPercentage == 0)
        {
            _stopwatch.Start();
            var (Left, Top) = Console.GetCursorPosition();
            Console.SetCursorPosition(Left, Top + 1);
            return;
        }

        var elapsed = _stopwatch.Elapsed.TotalSeconds;
        var projected = elapsed * (100 / (double)e.ProgressPercentage);
        var remaining = TimeSpan.FromSeconds(projected - elapsed + 1);

        Console.WriteLine($"Time Remaining: {remaining.PrettyHours()}");

        if (e.ProgressPercentage == 100) _stopwatch.Stop();
    }
}
