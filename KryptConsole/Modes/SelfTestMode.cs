using Krypt2Library;
using KryptConsole.Modes;

internal class SelfTestMode : IMode
{
    const string _plainText1 = "For God so loved the world, that he gave his only Son, that whoever believes in him should not perish but have eternal life.";
    const string _cipherText1 = "YM,@N#ojuZ.0IZ@dSSB6:(C(\".50NS@6p:D$:uh\"re@\"eK?xkNF;i.L\"o;ZfdztV.Svsb4zVN2vxQYNno!kw\"u2Zm4.d+75B#Jj-u@iE6exL-YSV6aEop10Kd3Ke";

    const string _plainText2 = "En effet, Dieu a tant aimé le monde qu’il a donné son Fils unique afin que quiconque croit en lui ne périsse pas mais ait la vie éternelle.";
    const string _cipherText2 = "’éZMAom\"oX!qFZAYWéTRu;ACn.: 3’xtb1V!D$-P\")V7!CBK,z\"Le(h8g5uhlR+Cv VteQi'&64N&véZFUs&sTBq8Uj$ &-vhH’éq'%zhBTRJPO??.rgmkIv)InETxDQ8*p’GS25td9@ ";

    const string _plainText3 = "Want so lief het God die wêreld gehad, dat Hy sy eniggebore Seun gegee het, sodat elkeen wat in Hom glo, nie verlore mag gaan nie, maar die ewige lewe kan hê.";
    const string _cipherText3 = "ê'z7F;+zimUfWbM#tS5I7:1v7Lb#exW&tP!Im\"dW!IrVbUb:KV9a,ae YwgJcyyA3xTiQe5$UN2ê'-4EFzNd06rSR &aiTYj$vJpQtvw#tLARO\";8ZjEf&1)G65Gy8b?êM,#YLSG2Y$da+w6L)nP;W&2Kz'RV4Q";

    const string _plainText4 = "Graphemes test: 👩‍👩‍👧‍👦 g̈ நி";
    const string _cipherText4 = "👩‍👩‍👧‍👦g̈நி0RXBp!wIvstVNY39.fuJh";

    const string _passphrase = "Jesus";

    public void Run()
    {
        Console.CursorVisible = false;

        RunTestsAndReportResults();

        // ReportStatistics();

        Console.CursorVisible = true;
    }



    private static void RunTestsAndReportResults()
    {
        var results = 0;
        ConsoleHelpers.WriteInColor("--------\nTesting:\n--------\n", ConsoleColor.DarkBlue);

        Console.Write("1. English text: ");
        if (Test(_plainText1, _cipherText1) == true) results++;
        Console.Write("2. French text: ");
        if (Test(_plainText2, _cipherText2) == true) results++;
        Console.Write("3. Afrikaans text: ");
        if (Test(_plainText3, _cipherText3) == true) results++;
        Console.Write("4. Graphemes: ");
        if (Test(_plainText4, _cipherText4) == true) results++;

        Console.WriteLine($"\n{results}/4 tests passed.\n");
    }
    private static bool Test(string plainText, string cipherText)
    {
        bool result;

        var kryptor = new Kryptor(new Gusto());

        string testCipherText = FirstTest(plainText, kryptor);
        string testPlainText = SecondTest(cipherText, kryptor);
        result = CheckResults(plainText, cipherText, testCipherText, testPlainText);

        return result;
    }

    private static string FirstTest(string plainText, Kryptor kryptor)
    {
        var testCipherText = kryptor.Encrypt(_passphrase, plainText);

        return testCipherText;
    }
    private static string SecondTest(string cipherText, Kryptor kryptor)
    {
        var testPlainText = kryptor.Decrypt(_passphrase, cipherText);

        return testPlainText;
    }
    private static bool CheckResults(string plainText, string cipherText, string testCipherText, string testPlainText)
    {
        bool result;
        if (cipherText == testCipherText && plainText == testPlainText)
        {
            ConsoleHelpers.WriteInColor("TEST PASSED\n", ConsoleColor.DarkGreen);

            result = true;
        }
        else
        {
            ConsoleHelpers.WriteInColor("TEST FAILED\n", ConsoleColor.DarkRed);

            result = false;
        }

        return result;
    }
}