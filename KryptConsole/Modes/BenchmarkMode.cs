using Krypt2Library;
using KryptConsole.Modes;
using System.Diagnostics;

internal class BenchmarkMode : IMode
{
    private static string GenerateLongString(int length)
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789,.!"; // just some chars
        char[] output = new char[length];

        for (int i = 0; i < length; i++)
            output[i] = chars[i % chars.Length];

        return new string(output);
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
        // JIT warmup.
        for (int i = 0; i < 10; i++)
            _ = Benchmark<Gusto>(10_000, true);

        ConsoleHelpers.WriteInColor("----------\nBenchmark:\n----------\n", ConsoleColor.DarkCyan);
        _ = Benchmark<Gusto>(1_000, false);
        _ = Benchmark<Gusto>(10_000, false);
        _ = Benchmark<Gusto>(100_000, false);
    }

    private string Benchmark<T>(int length, bool dryRun) where T : ICipher, new()
    {
        var kryptor = new Kryptor<T>();
        var justALongString = GenerateLongString(length);

        if (dryRun)
        {
            return kryptor.Encrypt("benchmark", justALongString);
        }
        else
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            string result = kryptor.Encrypt("benchmark", justALongString);
            stopwatch.Stop();
            var time = stopwatch.Elapsed.TotalSeconds;
            var totalChars = justALongString.Length;
            var rate = totalChars / time;

            Console.WriteLine($"{totalChars, 8} characters in {time:0.0000} seconds = {rate, 10:0} characters/second.");
            return result;
        }
    }
}
