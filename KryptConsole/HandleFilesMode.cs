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
                    if (isValidFile(arg)) FileNames.Add(arg);
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
                Decrypt();
                break;
        }
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
        Console.WriteLine($"Encrypting '{filename}':");
        var passphrase = PromptHelpers.PromptForPassword(CryptType.Encryption);
        Console.WriteLine();

        var cipherText = EncyptMessage(passphrase, message);

        SaveCipherTextToFile(filename, ".krypt", cipherText);
    }

    private void SaveCipherTextToFile(string filename, string newExtension, string cipherText)
    {
        filename = $"{filename}{newExtension}";
        {
            File.WriteAllText(filename, cipherText);
            Console.WriteLine($"Written to file '{filename}'.");
        }
    }

    private void SavePlainTextToFile(string filename, string plainText)
    {
        filename = Path.GetFileNameWithoutExtension(filename);
        {
            File.WriteAllText(filename, plainText);
            Console.WriteLine($"Written to file '{filename}'.");
        }
    }

    private string EncyptMessage(string passphrase, string message)
    {
        var backgroundWorker = BackgroundWorkerHelpers.CreateBackgroundWorker();
        var kryptor = new Kryptor(new Betor(), backgroundWorker);

        var cipherText = kryptor.Encrypt(passphrase, message);

        return cipherText;
    }

    private void Decrypt()
    {
        foreach (var item in FileNames)
        {
            Console.WriteLine(item);
        }
    }


    private bool isValidFile(string arg)
    {
        bool output = true;
        
        if (File.Exists(arg) == false) output = false;

        return output;
    }
}