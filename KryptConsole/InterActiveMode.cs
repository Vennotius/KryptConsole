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

        Console.WriteLine("\n\n-----------\nCipherText:\n-----------");
        OutputCipherText(Console.WriteLine, _cipherText);
    }
    private string EncyptMessage(string passphrase, string message)
    {
        var backgroundWorker = new BackgroundWorker();
        backgroundWorker.WorkerReportsProgress = true;
        var kryptor = new Kryptor(new Betor(), backgroundWorker);

        _cipherText = kryptor.Encrypt(passphrase, message);
        
        return _cipherText;
    }
    

    private void OutputCipherText(Action<string> outputMethod, string cipherText)
    {
        outputMethod(cipherText);
    }

    private void Decryption()
    {
        _cipherText = PromptHelpers.PromptForCipherText();
        _passphrase = PromptHelpers.PromptOnceForPassword(CryptType.Decryption);

        _message = DecryptMessage(_passphrase, _cipherText);
    }
}