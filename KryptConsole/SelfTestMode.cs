using Krypt2Library;
using System.Diagnostics;

internal class SelfTestMode : IMode
{
    string _plainText1 = "Therefore God has highly exalted him and bestowed on him the name that is above every name, so that at the name of Jesus every knee should bow, in heaven and on earth and under the earth, and every tongue confess that Jesus Christ is Lord, to the glory of God the Father.";
    string _cipherText1 = "9ID+V!0IutU!La?Q)+S.H-W@&ya4Hd*ZVUN6uKMW+Dr\"r-va33cO%u:iygBxI3jWIG4UHzZu?+baRl8G@\"z*jtT,a.D9)xn%7oL@CF2pqPK@fYarpPp50@XSXiA(jKe)! ?N;N$vs&Ov0Y9@VZ6GAn8kv?&GsS3R:Ip?'6v-P1w%Z9W)C0+!rSiPLgJ!YTgj1@+Pjjh8j0-fcYDiRbG6s-%8M\"K9j\"ENMU'5Q9T.Hi)1PU-3@sI-:.Z94N(gwis5YDe\"Y$eT;i3Wuu6";

    string _plainText2 = "For God so loved the world, that he gave his only Son, that whoever believes in him should not perish but have eternal life.";
    string _cipherText2 = "JwY:d,-tP,A5)v-noaS68y'!-isulI2eVUID\"K-@+7;\"X-ozY3Ob8GVg&7*nOUc%ynm.4%;u,#5JS@tG2QbIxrs#qu+&8inh*1PLMvle'67wUev-+Q7w\"fGo5$:O";

    string _plainText3 = "Behold, I am sending you out as sheep in the midst of wolves, so be wise as serpents and innocent as doves. Beware of men, for they will deliver you over to courts and flog you in their synagogues, and you will be dragged before governors and kings for my sake, to bear witness before them and the Gentiles. When they deliver you over, do not be anxious how you are to speak or what you are to say, for what you are to say will be given to you in that hour. For it is not you who speak, but the Spirit of your Father speaking through you. Brother will deliver brother over to death, and the father his child, and children will rise against parents and have them put to death, and you will be hated by all for my name's sake. But the one who endures to the end will be saved.";
    string _cipherText3 = "lM0jJoSt5t8)!*-tym(;8d'M&%u.&-#EvUI-MVoK+*V)Xdt*c ub'(ZhlH,CnBXz!SG.Pv5O(q#$u#m4@9w NWTd'vM&8-F7XqCLxyLIGygn,emk@QpVJEN0: 95Q?e(- ZNU$z2i&cjCbf6O;gA.28kY?3khf3u!vOS94Lv'3v,r43qCe8 )ap$MSjHU;w5Dt+R!jiWJOvsP2vS+3w))Lw?un 9HWiv1@'y@?;NB1;1P6tKZIIJqS@H u.8wj75\"39,g+WkwCL!*,TjlI*2IV!Z97:Qy'&06yqi\"Wy8m,fsb1YvJ;ieqe4?.KG+CB@J&v;im3DW;r3.er.Q3q!K zOCD'gCIr!lw'McRZrW7,6w+53h8YN6F:1ZH@\"J:P+WjvV%$3HQ'xcQj+C*L(Ub-Habf#zNoNU46en'UBdhlKNRb0?nSa9J:kApjf\"$VNINirl$;bKQG!:S,rA; KBTa8&(4fmpw74y6bq7Fq-EJSXlDrLx,hW4H$fc1 i@h!5K%kBrWt(zy3D310XLr.sU+Z*B% *vrA.E' U4ap1ry.&(yf.dEKmRp2z*(Nn0*A*Qo!2Rr 33Qe7h )Z1-DDuu;6mS:k45;@1YjT:7mH1D\"AN9fYTi(H3vRcFg6f6DzW9@P4g,7GXJXXk(j.Ij :r:u\"zHx&9*Y)WeWdUsILd#9d6u%ehMGhYxo0?&WYikN3C@G88amy$.&&*8Awx1nyJAsn*#PZm3q5(MLC1hfXMMAz+#He6l:SKfPwSr gUiAoN.zDifw";

    Stopwatch _stopwatchForStats = new Stopwatch();
    Stopwatch _stopwatchForEstimate = new Stopwatch();

    public void Run()
    {
        Console.CursorVisible = false;
        RunTestsAndReportResults();

        ReportStatistics();
    }

    private void RunTestsAndReportResults()
    {
        var results = 0;

        Console.WriteLine("------------------------------");
        if (Test(_plainText1, _cipherText1) == true) results++;
        Console.WriteLine("\n------------------------------");
        if (Test(_plainText2, _cipherText2) == true) results++;
        Console.WriteLine("\n------------------------------");
        if (Test(_plainText3, _cipherText3) == true) results++;
        Console.WriteLine("\n------------------------------");

        Console.WriteLine($"\n{results}/3 tests passed.");
    }
    private bool Test(string plainText, string cipherText)
    {
        bool result;

        var backgroundWorker = BackgroundWorkerHelpers.CreateBackgroundWorker();
        backgroundWorker.ProgressChanged += ReportTimeRemaining;

        var kryptor = new Kryptor(new Betor(), backgroundWorker);

        string testCipherText = FirstTest(plainText, cipherText, kryptor);
        string testPlainText = SecondTest(cipherText, kryptor);
        result = CheckResults(plainText, cipherText, testCipherText, testPlainText);

        return result;
    }

    private string FirstTest(string plainText, string cipherText, Kryptor kryptor)
    {
        Console.WriteLine($"\nChecking:\n\n{plainText}\n\n");

        _stopwatchForStats.Start();
        var testCipherText = kryptor.Encrypt("Jesus", plainText);
        _stopwatchForStats.Stop();

        Console.WriteLine($"\n\nEncrypted as:\n\n{testCipherText}\n");
        Console.WriteLine($"Which should match:\n\n{cipherText}\n");
        return testCipherText;
    }
    private string SecondTest(string cipherText, Kryptor kryptor)
    {
        _stopwatchForStats.Start();
        var testPlainText = kryptor.Decrypt("Jesus", cipherText);
        _stopwatchForStats.Stop();

        Console.WriteLine($"\n\nDecrypted as:\n\n{testPlainText}\n");
        return testPlainText;
    }
    private static bool CheckResults(string plainText, string cipherText, string testCipherText, string testPlainText)
    {
        bool result;
        if (cipherText == testCipherText && plainText == testPlainText)
        {
            Console.WriteLine("TEST PASSED");
            result = true;
        }
        else
        {
            Console.WriteLine("TEST FAILED");
            result = false;
        }

        return result;
    }

    private void ReportStatistics()
    {
        var totalLength = _plainText1.Length;
        totalLength += _plainText2.Length;
        totalLength += _plainText3.Length;
        totalLength += _cipherText1.Length;
        totalLength += _cipherText2.Length;
        totalLength += _cipherText3.Length;

        var totalTime = _stopwatchForStats.Elapsed.TotalSeconds;

        Console.WriteLine("\nStatistics:\n-----------");
        Console.WriteLine($"\nEncrypted & Decrypted {totalLength} characters in {totalTime:0.00} seconds.");
        Console.WriteLine($"\n{(totalLength / totalTime):0.0} characters per second.");
        Console.WriteLine($"{(totalLength / totalTime) * 60:0} characters per minute.");
        Console.WriteLine($"{(totalLength / totalTime) * 60 * 60:0} characters per hour.\n");
    }
    private void ReportTimeRemaining(object? sender, System.ComponentModel.ProgressChangedEventArgs e)
    {
        if (e.ProgressPercentage == 0)
        {
            _stopwatchForEstimate.Start();
            var cursorPosition = Console.GetCursorPosition();
            Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top + 1);
            return;
        }

        var elapsed = _stopwatchForEstimate.Elapsed.TotalSeconds;
        var projected = elapsed * (100 / (double)e.ProgressPercentage);
        var remaining = TimeSpan.FromSeconds(projected - elapsed + 1);

        Console.WriteLine($"Time Remaining: {remaining.PrettyHours()}");

        if (e.ProgressPercentage == 100) _stopwatchForEstimate.Reset();
    }
}