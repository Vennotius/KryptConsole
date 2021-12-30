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
    default:
        mode = new HandleFilesMode(args);
        break;
}
mode.Run();