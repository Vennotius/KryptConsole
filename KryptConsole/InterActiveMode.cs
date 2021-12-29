using Krypt2Library;

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
        _message = PromptForMessage();
        _passphrase = PromptOnceForPassphrase(CryptType.Encryption);
    }
    private string PromptForMessage()
    {
        return Prompt("\nMessage:\n");
    }
    private string PromptTwiceForPassphrase()
    {
        string output = "";

        var firstInput = Prompt("\nPassphrase: ");
        var secondInput = Prompt("Enter it again: ");

        if (firstInput == secondInput)
        {
            output = firstInput;
        }
        else
        {
            Console.WriteLine("Second input did not match the first. Try again...");
            PromptTwiceForPassphrase();
        }

        return output;
    }

    private void Decryption()
    {
        _cipherText = PromptForCipherText();
        _passphrase = PromptOnceForPassphrase(CryptType.Decryption);
    }
    private string PromptForCipherText()
    {
        return Prompt("\nEnter text to decrypt:\n");
    }
    private string PromptOnceForPassphrase(CryptType type)
    {
        string output = "";
        
        switch (type)
        {
            case CryptType.Encryption:
                output = PromptTwiceForPassphrase();
                break;
            case CryptType.Decryption:
                output = Prompt("\nPassphrase: ");
                break;
        }

        return output;
    }
    
    private string Prompt(string promptMessage)
    {
        string output = "";

        Console.Write(promptMessage);
        var input = Console.ReadLine();

        if (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Invalid input. Try again...");
            output = Prompt(promptMessage);
        }
        else
        {
            output = input;
        }

        return output;
    }
}