using KryptConsole.Modes;

internal class ShowHelpMode : IMode
{
    public void Run()
    {
        Console.WriteLine("\nKrypt encryptor & decryptor");
        Console.WriteLine("---------------------------\n");

        Console.WriteLine("Options:");
        Console.WriteLine("--test     \tTest if program works as expected");
        Console.WriteLine("--benchmark\tBenchmark");
        Console.WriteLine("--help     \tShow this help\n");

        Console.WriteLine("Usage examples:");
        Console.WriteLine("KryptConsole");
        Console.WriteLine("KryptConsole file.txt");
        Console.WriteLine("KryptConsole file1.txt file2.txt file3.txt");
        Console.WriteLine("KryptConsole --benchmark\n");
    }
}
