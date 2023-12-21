namespace AdventOfCode;

public class InputFileService
{
    public string[] ReadInputFile(string path)
    {
        return File.ReadAllLines(path);
    }
}
