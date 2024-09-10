using Krypt2Library;

namespace KryptConsole
{
    public static class PromptHelpers
    {
        public static string PromptForMessage()
        {
            return Prompt("\nMessage:\n");
        }
        public static string PromptTwiceForPassword()
        {
            var firstInput = PromptOnceForPassword("\nPassphrase: ");
            var secondInput = PromptOnceForPassword("\nEnter it again: ");

            string output;
            if (firstInput == secondInput)
            {
                output = firstInput;
            }
            else
            {
                ConsoleHelpers.WriteInColor("\nSecond input did not match the first. Try again...\n", ConsoleColor.DarkRed);
                output = PromptTwiceForPassword();
            }

            return output;
        }

        public static string PromptForCipherText()
        {
            return Prompt("\nEnter text to decrypt:\n");
        }
        public static string PromptForPassword(CryptType type)
        {
            string output = "";

            switch (type)
            {
                case CryptType.Encryption:
                    output = PromptTwiceForPassword();
                    break;
                case CryptType.Decryption:
                    output = PromptOnceForPassword("\nPassphrase: ");
                    break;
            }

            return output;
        }

        public static string Prompt(string promptMessage)
        {
            ConsoleHelpers.WriteInColor(promptMessage, ConsoleColor.DarkCyan);
            var input = Console.ReadLine();

            string output;
            if (string.IsNullOrEmpty(input))
            {
                ConsoleHelpers.WriteInColor("Invalid input. Try again...\n", ConsoleColor.DarkRed);
                output = Prompt(promptMessage);
            }
            else
            {
                output = input;
            }

            return output;
        }
        public static string PromptOnceForPassword(string promptMessage)
        {
            ConsoleHelpers.WriteInColor(promptMessage, ConsoleColor.DarkCyan);
            var input = GetInputWhileHidingCharacters();

            string output;
            if (string.IsNullOrWhiteSpace(input))
            {
                ConsoleHelpers.WriteInColor("Invalid input. Try again...\n", ConsoleColor.DarkRed);
                output = PromptOnceForPassword(promptMessage);
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
                        output = output[0..^1];
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

            var shouldSaveToFile = Prompt("\nDo you want to save this to a file (y/n)? ").ToLower();
            if (shouldSaveToFile == "y" || shouldSaveToFile == "yes")
            {
                output = PromptForFilename();
            }

            return output;
        }
        public static string PromptForFilename()
        {
            var output = Prompt("Enter filename: ");

            if (File.Exists(output) == true)
            {
                Console.WriteLine("That file already exists. Enter a filename for a file that does not exist.");
                output = PromptForFilename();
            }

            return output;
        }
    }
}