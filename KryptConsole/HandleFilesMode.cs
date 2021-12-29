internal class HandleFilesMode: IMode
{
    public List<string> FileNames { get; set; } = new List<string>();
    public HandleFilesMode(string[] args)
    {
        foreach (var arg in args)
        {
            if (isValidFile(arg))
            {
                FileNames.Add(arg);
            }
        }
    }

    public void Run()
    {
        throw new NotImplementedException();
    }

    private bool isValidFile(string arg)
    {
        bool output = true;
        
        if (File.Exists(arg) == false) output = false;

        return output;
    }
}