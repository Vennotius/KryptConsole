using Krypt2Library;

IMode mode;

if (args.Length == 0)
{
    mode = new InterActiveMode();
}
else if (args.Length == 1 && args[0] == "--test")
{
    mode = new SelfTestMode();
}
else
{
    mode = new HandleFilesMode(args);
}

mode.Run();