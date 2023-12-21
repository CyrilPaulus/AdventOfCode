namespace AdventOfCode;

public interface ISolver
{
    int GetDay();
    string Solve(string[] input, bool showDebug);
}
