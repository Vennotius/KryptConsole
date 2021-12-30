using Krypt2Library;
using System.ComponentModel;

internal class InterActiveMode : IMode
{
    private string _message = "";
    private string _passphrase = "";
    private string _cipherText = "";

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
        _passphrase = PromptHelpers.PromptOnceForPassword(CryptType.Encryption);
        _cipherText = EncyptMessage(_passphrase, _message);
        
        ShowResultsOnScreen();

        SaveToFile();
    }

    private string EncyptMessage(string passphrase, string message)
    {
        var backgroundWorker = CreateBackgroundWorker();
        var kryptor = new Kryptor(new Betor(), backgroundWorker);

        _cipherText = kryptor.Encrypt(passphrase, message);
        
        return _cipherText;
    }


    private void Decryption()
    {
        _cipherText = PromptHelpers.PromptForCipherText();
        _passphrase = PromptHelpers.PromptOnceForPassword(CryptType.Decryption);

        _message = DecryptMessage(_passphrase, _cipherText);

        Console.WriteLine("\n\n-----------\nDecrypted Text:\n-----------");
        Console.WriteLine(_message);
    }

    private string DecryptMessage(string passphrase, string cipherText)
    {
        var backgroundWorker = CreateBackgroundWorker();
        var kryptor = new Kryptor(new Betor(), backgroundWorker);

        _message = kryptor.Decrypt(passphrase, cipherText);

        return _message;
    }
    

    private void ShowResultsOnScreen()
    {
        Console.WriteLine("\n\n-----------\nCipherText:\n-----------");
        Console.WriteLine(_cipherText);
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


    private BackgroundWorker CreateBackgroundWorker()
    {
        var backgroundWorker = new BackgroundWorker();
        backgroundWorker.WorkerReportsProgress = true;
        backgroundWorker.ProgressChanged += ReportProgress;

        return backgroundWorker;
    }

    private void ReportProgress(object? sender, ProgressChangedEventArgs e)
    {
        Console.Write('.');
    }
}

