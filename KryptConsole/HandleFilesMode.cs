using Krypt2Library;

internal class HandleFilesMode: IMode
{
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

        var cipherText = EncyptMessage(passphrase, message);

        ShowResultsOnScreen(cipherText);
        PromptToSaveFile(cipherText);
    }

    private string EncyptMessage(string passphrase, string message)
    {
        var backgroundWorker = BackgroundWorkerHelpers.CreateBackgroundWorker();
        var kryptor = new Kryptor(new Betor(), backgroundWorker);

        var cipherText = kryptor.Encrypt(passphrase, message);

        return cipherText;
    }
    private void PromptToSaveFile(string cipherText)
    {
        string filename = PromptHelpers.PromptIfWantToSaveToFile();

        if (string.IsNullOrWhiteSpace(filename) == false)
        {
            SaveToFile(filename, cipherText);
        }
    }
    private void SaveToFile(string filename, string text)
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

        var plainText = DecyptMessage(passphrase, cipherText);

        ShowResultsOnScreen(plainText);
        PromptToSaveFile(plainText);
    }

    private string DecyptMessage(string passphrase, string cipherText)
    {
        var backgroundWorker = BackgroundWorkerHelpers.CreateBackgroundWorker();
        var kryptor = new Kryptor(new Betor(), backgroundWorker);

        var plainText = kryptor.Decrypt(passphrase, cipherText);

        return plainText;
    }

    private bool IsValidFile(string arg)
    {
        bool output = true;
        
        if (File.Exists(arg) == false) output = false;

        return output;
    }

    private void ShowResultsOnScreen(string results)
    {
        Console.WriteLine("\n\n-------\nResult:\n-------");
        Console.WriteLine(results);
    }
}