using Krypt2Library;
using KryptConsole;
using System.Diagnostics;

internal class HandleFilesMode : IMode
{
    private readonly Stopwatch _stopwatch = new();
    List<string> FileNames { get; set; } = new List<string>();
    CryptType? WhatToDo { get; set; } = null;

    public HandleFilesMode(string[] args)
    {
        foreach (var arg in args)
        {
            switch (arg)
            {
                case "--encrypt":
                    WhatToDo = CryptType.Encryption;
                    break;
                case "--decrypt":
                    WhatToDo = CryptType.Decryption;
                    break;
                default:
                    if (IsValidFile(arg)) FileNames.Add(arg);
                    break;
            }
        }
    }

    public void Run()
    {
        if (FileNames.Count == 0)
        {
            Console.WriteLine("No valid filenames given.");
            return;
        }

        ListFilesOnConsole();

        if (WhatToDo == null)
        {
            WhatToDo = PromptForDesiredActivity();
        }

        switch (WhatToDo)
        {
            case CryptType.Encryption:
                foreach (var filename in FileNames)
                {
                    var message = File.ReadAllText(filename);
                    Encrypt(filename, message);
                }
                break;
            case CryptType.Decryption:
                foreach (var filename in FileNames)
                {
                    var cipherText = File.ReadAllText(filename);
                    Decrypt(filename, cipherText);
                }
                break;
        }
    }

    private void ListFilesOnConsole()
    {
        Console.WriteLine("Files:");
        foreach (var filename in FileNames)
        {
            Console.WriteLine(filename);
        }
        Console.WriteLine();
    }

    private CryptType PromptForDesiredActivity()
    {
        Console.Write("What would you like to do?\n\n1. Encrypt\n2. Decrypt\n\n1 or 2? ");
        var activity = Console.ReadLine();

        switch (activity)
        {
            case "1":
                return CryptType.Encryption;
            case "2":
                return CryptType.Decryption;
            default:
                Console.WriteLine("Invalid Option.");
                return PromptForDesiredActivity();
        }
    }

    private void Encrypt(string filename, string message)
    {
        var passphrase = PromptHelpers.PromptForPassword(CryptType.Encryption);
        Console.WriteLine($"\nEncrypting '{filename}':");
        Console.WriteLine();

        Console.CursorVisible = false;
        var cipherText = EncyptMessage(passphrase, message);
        Console.CursorVisible = true;

        ShowResultsOnScreen(cipherText);
        PromptToSaveFile(cipherText);
    }

    private string EncyptMessage(string passphrase, string message)
    {
        var kryptor = new Kryptor(new Gusto());

        var cipherText = kryptor.Encrypt(passphrase, message);

        return cipherText;
    }
    private static void PromptToSaveFile(string cipherText)
    {
        string filename = PromptHelpers.PromptIfWantToSaveToFile();

        if (string.IsNullOrWhiteSpace(filename) == false)
        {
            SaveToFile(filename, cipherText);
        }
    }
    private static void SaveToFile(string filename, string text)
    {
        //if (File.Exists($"{filename}{newExtension}"))
        //{
        //    filename = filename + $"({DateTime.Now.ToString("yyyy-MM-dd HHmm")})";
        //}

        File.WriteAllText(filename, text);
        Console.WriteLine($"Written to file '{filename}'.");
    }


    private void Decrypt(string filename, string cipherText)
    {
        var passphrase = PromptHelpers.PromptForPassword(CryptType.Decryption);
        Console.WriteLine($"\nDecrypting '{filename}':");
        Console.WriteLine();

        Console.CursorVisible = false;
        var plainText = DecyptMessage(passphrase, cipherText);
        Console.CursorVisible = true;

        ShowResultsOnScreen(plainText);
        PromptToSaveFile(plainText);
    }

    private string DecyptMessage(string passphrase, string cipherText)
    {
        var kryptor = new Kryptor(new Gusto());

        var plainText = kryptor.Decrypt(passphrase, cipherText);

        return plainText;
    }

    private static bool IsValidFile(string arg)
    {
        bool output = true;

        if (File.Exists(arg) == false) output = false;

        return output;
    }

    private static void ShowResultsOnScreen(string results)
    {
        Console.WriteLine("\n\n-------\nResult:\n-------");
        Console.WriteLine(results);
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