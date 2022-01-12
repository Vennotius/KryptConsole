public static class ConsoleHelpers
{
    public static void WriteInColor(string text, ConsoleColor color)
    {
        var consoleColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = consoleColor;
    }
}
