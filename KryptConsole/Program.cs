using Krypt2Library;

IMode mode;

switch (args.Length)
{
    case 0:
        mode = new InterActiveMode();
        break;
    case 1 when args[0] == "--test":
        mode = new SelfTestMode();
        break;
    case 1 when args[0] == "--benchmark":
        mode = new BenchmarkMode();
        break;
    case 1 when args[0] == "--help" || args[0] == "help" || args[0] == "-h" || args[0] == "-H":
        mode = new ShowHelpMode();
        break;
    default:
        mode = new HandleFilesMode(args);
        break;
}

mode.Run();