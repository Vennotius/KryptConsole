using Krypt2Library;
using System.Diagnostics;
using System.Text;

internal class BenchmarkMode : IMode
{
    private string _justALongString = "";
    private readonly Stopwatch _stopwatch = new();

    private static string GenerateLongString(int length)
    {
        var output = new StringBuilder();

        for (int i = 0; i < length / 10; i++)
        {
            output.Append("0123456789");
        }

        return output.ToString();
    }

    public void Run()
    {
        Console.CursorVisible = false;

        RunBenchmarks();
        Console.WriteLine();
        Console.CursorVisible = true;
    }

    private void RunBenchmarks()
    {
        ConsoleHelpers.WriteInColor("----------\nBenchmark:\n----------\n", ConsoleColor.DarkBlue);
        BenchmarkForGusto(10000);
        BenchmarkForGusto(100000);
        BenchmarkForGusto(1000000);
    }

    private void BenchmarkForGusto(int length)
    {
        _justALongString = GenerateLongString(length);

        var kryptor = new Kryptor(new Gusto());
        _stopwatch.Start();
        kryptor.Encrypt("benchmark", _justALongString);
        _stopwatch.Stop();

        var time = _stopwatch.Elapsed.TotalSeconds;
        var totalChars = _justALongString.Length;
        var rate = totalChars / time;

        Console.WriteLine($"{totalChars} characters in {time:0.00} seconds = {rate:0} characters/second.");
    }
}
