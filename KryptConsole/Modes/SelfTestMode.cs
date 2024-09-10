using Krypt2Library;
using KryptConsole.Modes;

internal class SelfTestMode : IMode
{
    private static List<(string Name, string Passphrase, string PlainText, string CipherText)> _testCases = new()
    {
        ("English text", "Jesus", "For God so loved the world, that he gave his only Son, that whoever believes in him should not perish but have eternal life.", "N xd+V&;i.@qb3+1!Rtl5'Id*i6D6mNVvt?clm9HNU:w)(1zF%t*SI+&7&ova@'VIS;mT3SJHD'x60x6Cgn6GJ+f1VmIPhrGEQi9%bZll2Klu8 6BZ@PJiDuKw5:"),
        ("French", "Jesus", "En effet, Dieu a tant aimé le monde qu’il a donné son Fils unique afin que quiconque croit en lui ne périsse pas mais ait la vie éternelle.", "’éUV ui(PJMm8p52CndxOpé#a%c kOéACn;’tXx:LSMPJ3a 5f’,5,R49GéFZdcELv3qJ&u3YoJa7k71FSj?t2HdOrxg\"IB;*Sw;Vmo5GP%aXaK\"qu\"2u8’kW970Jl,&fXVL)O7;KQbn;"),
        ("Afrikaans", "Jesus", "Want so lief het God die wêreld gehad, dat Hy sy eniggebore Seun gegee het, sodat elkeen wat in Hom glo, nie verlore mag gaan nie, maar die ewige lewe kan hê.", "êgYbQY?BO%#6),D)l+ZMZYI-bn;7?frJ?6y@W7OOsA1êg.lJW-'y:b. v:hP\"!$&a1y;Yun%Z9êRH:iK%,EYOZ*d0w!racUyNA:GvJrG Iyh5e!kB;fD)\"RemH,DGEz%;mj6l.wJ$Xj.?qK6+tLT;FtEHF-zCPr"),
        ("Graphemes", "Jesus", "Graphemes test: 👩‍👩‍👧‍👦 g̈ நி", "👩‍👩‍👧‍👦g̈நிqVQ%vR(s7a5v#?d,g̈Dw*T"),
        ("Linebreaks", "Jesus", $"MultipleLines{Environment.NewLine}are being tested{Environment.NewLine}here.", "\n*;\nQ* y,u#\"(uv'jv\nz0*S*700*\"fjFk4L;D"),
    };

    public void Run()
    {
        Console.CursorVisible = false;

        RunTestsAndReportResults();

        Console.CursorVisible = true;
    }

    private static void RunTestsAndReportResults()
    {
        var results = 0;
        ConsoleHelpers.WriteInColor("--------\nTesting:\n--------\n", ConsoleColor.DarkCyan);

        for (int i = 0; i < _testCases.Count; i++)
        {
            (string Name, string Passphrase, string PlainText, string CipherText) @case = _testCases[i];
            Console.Write($"{i + 1}. {@case.Name}: ");
            if (Test(@case)) 
                results++;
        }

        Console.WriteLine($"\n{results}/{_testCases.Count} tests passed.\n");
    }

    private static bool Test((string Name, string Passphrase, string PlainText, string CipherText) testCase)
    {
        var kryptor = new Kryptor<Gusto>();

        string encrypted = kryptor.Encrypt(testCase.Passphrase, testCase.PlainText);
        string decrypted = kryptor.Decrypt(testCase.Passphrase, encrypted);

        if (testCase.PlainText == decrypted)
        {
            if (testCase.CipherText == encrypted)
                ConsoleHelpers.WriteInColor("TEST PASSED\n", ConsoleColor.DarkGreen);
            else
                ConsoleHelpers.WriteInColor("Encrypt-decrypt OK, but cipher has changed\n", ConsoleColor.DarkYellow);

            return true;
        }
        else
        {
            ConsoleHelpers.WriteInColor("TEST FAILED\n", ConsoleColor.DarkRed);

            return false;
        }
    }
}