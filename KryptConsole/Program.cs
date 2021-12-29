using Krypt2Library;

IMode mode;

if (args.Length == 0)
{
    mode = new InterActiveMode();
}
else
{
    mode = new HandleFilesMode(args);
}

mode.Run();