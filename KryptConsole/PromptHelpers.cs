using Krypt2Library;

public static class PromptHelpers
{
    public static string PromptForMessage()
    {
        return Prompt("\nMessage:\n");
    }
    public static string PromptTwiceForPassword()
    {
        string output = "";

        var firstInput = PromptForPassword("\nPassphrase: ");
        var secondInput = PromptForPassword("\nEnter it again: ");

        if (firstInput == secondInput)
        {
            output = firstInput;
        }
        else
        {
            Console.WriteLine("\nSecond input did not match the first. Try again...");
            output = PromptTwiceForPassword();
        }

        return output;
    }

    public static string PromptForCipherText()
    {
        return Prompt("\nEnter text to decrypt:\n");
    }
    public static string PromptOnceForPassword(CryptType type)
    {
        string output = "";

        switch (type)
        {
            case CryptType.Encryption:
                output = PromptTwiceForPassword();
                break;
            case CryptType.Decryption:
                output = PromptForPassword("\nPassphrase: ");
                break;
        }

        return output;
    }

    public static string Prompt(string promptMessage)
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
    public static string PromptForPassword(string promptMessage)
    {
        string output = "";

        Console.Write(promptMessage);
        var input = GetInputWhileHidingCharacters();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Invalid input. Try again...");
            output = PromptForPassword(promptMessage);
        }
        else
        {
            output = input;
        }

        return output;
    }
    public static string GetInputWhileHidingCharacters()
    {
        var output = "";

        ConsoleKeyInfo key;
        while (true)
        {
            key = Console.ReadKey(true);

            if (key.Key != ConsoleKey.Enter)
            {
                if (key.Key == ConsoleKey.Backspace && output.Length > 0)
                {
                    output = output.Substring(0, output.Length - 1);
                }
                else
                {
                    output += key.KeyChar;
                }
            }
            else break;
        }

        return output;
    }


    public static string PromptIfWantToSaveToFile()
    {
        var output = "";
        
        var shouldSaveToFile = PromptHelpers.Prompt("\nDo you want to save this to a file (y/n)? ").ToLower();
        if (shouldSaveToFile == "y" || shouldSaveToFile == "yes")
        {
            output = PromptForFilename();
        }

        return output;
    }
    public static string PromptForFilename()
    {
        var output = PromptHelpers.Prompt("Enter filename: ");

        if (File.Exists(output) == true)
        {
            Console.WriteLine("That file already exists. Enter a filename for a file that does not exist.");
            PromptForFilename();
        }

        return output;
    }
}
