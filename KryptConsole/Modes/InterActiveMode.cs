using Krypt2Library;
using KryptConsole;
using System.Diagnostics;

internal class InterActiveMode : IMode
{
    private string _message = "";
    private string _passphrase = "";
    private string _cipherText = "";
    private readonly Stopwatch _stopwatch = new();

    public void Run()
    {
        Console.Write("What would you like to do?\n\n1. Encrypt\n2. Decrypt\n\n1 or 2? ");
        var activity = Console.ReadLine();

        switch (activity)
        {
            case "1":
                Encryption();
                break;
            case "2":
                Decryption();
                break;
            default:
                Console.WriteLine("Invalid option. Exiting.");
                break;
        }
    }


    private void Encryption()
    {
        _message = PromptHelpers.PromptForMessage();
        _passphrase = PromptHelpers.PromptForPassword(CryptType.Encryption);
        Console.WriteLine();

        Console.CursorVisible = false;
        _cipherText = EncyptMessage(_passphrase, _message);
        Console.CursorVisible = true;
        ShowResultsOnScreen(_cipherText);

        SaveToFile();
    }
    private string EncyptMessage(string passphrase, string message)
    {
        var backgroundWorker = BackgroundWorkerHelpers.CreateBackgroundWorker();
        backgroundWorker.ProgressChanged += ReportTimeRemaining;

        var kryptor = new Kryptor(new Betor(Betor.EncryptCharacterUsingShift, Betor.DecryptCharacterUsingShift), backgroundWorker);

        _cipherText = kryptor.Encrypt(passphrase, message);

        return _cipherText;
    }


    private void Decryption()
    {
        _cipherText = PromptHelpers.PromptForCipherText();
        _passphrase = PromptHelpers.PromptForPassword(CryptType.Decryption);
        Console.WriteLine();

        Console.CursorVisible = false;
        _message = DecryptMessage(_passphrase, _cipherText);
        Console.CursorVisible = true;

        ShowResultsOnScreen(_message);

        SaveToFile();
    }
    private string DecryptMessage(string passphrase, string cipherText)
    {
        var backgroundWorker = BackgroundWorkerHelpers.CreateBackgroundWorker();
        backgroundWorker.ProgressChanged += ReportTimeRemaining;

        var kryptor = new Kryptor(new Betor(Betor.EncryptCharacterUsingShift, Betor.DecryptCharacterUsingShift), backgroundWorker);

        _message = kryptor.Decrypt(passphrase, cipherText);

        return _message;
    }
    

    private static void ShowResultsOnScreen(string results)
    {
        Console.WriteLine("\n\n-------\nResult:\n-------");
        Console.WriteLine(results);
    }
    private void SaveToFile()
    {
        var filename = PromptHelpers.PromptIfWantToSaveToFile();
        if (string.IsNullOrWhiteSpace(filename) == false)
        {
            File.WriteAllText(filename, _cipherText);
            Console.WriteLine($"Written to file '{filename}'.");
        }
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

        if (e.ProgressPercentage == 100) _stopwatch.Reset();
    }
}
