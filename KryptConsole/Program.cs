IMode mode;

mode = args.Length switch
{
    0 => new InterActiveMode(),
    1 when args[0] == "--test" => new SelfTestMode(),
    1 when args[0] == "--benchmark" => new BenchmarkMode(),
    1 when args[0] == "--help" || args[0] == "help" || args[0] == "-h" || args[0] == "-H" => new ShowHelpMode(),
    _ => new HandleFilesMode(args),
};

mode.Run();