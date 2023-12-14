
namespace AdventOfCodeDay3;

public class NaiveSolver
{
    public int Solve(string[] lines)
    {
        var width = lines[0].Length;
        var height = lines.Length;

        var total = 0;

        var currentNumber = 0;
        int? adjacentGearIndex = null;

        var gearDict = new Dictionary<int, int>();

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (int.TryParse(lines[j][i].ToString(), out var digit))
                {
                    currentNumber *= 10;
                    currentNumber += digit;

                    adjacentGearIndex ??= GetAdjacentGearIndex(i, j, lines);
                    continue;
                }

                //No longer parsing a number
                if (adjacentGearIndex != null)
                {

                    if (gearDict.TryGetValue(adjacentGearIndex.Value, out var previousNumber))
                        total += currentNumber * previousNumber;
                    else
                        gearDict.Add(adjacentGearIndex.Value, currentNumber);
                }

                adjacentGearIndex = null;
                currentNumber = 0;
            }
        }

        return total;
    }

    private int? GetAdjacentGearIndex(int i, int j, string[] lines)
    {
        var width = lines[0].Length;
        var height = lines.Length;
        for (var jj = j - 1; jj <= j + 1; jj++)
        {
            for (var ii = i - 1; ii <= i + 1; ii++)
            {
                if (jj < 0 || jj >= height || ii < 0 || ii >= width)
                    continue;

                var c = lines[jj][ii];
                if (c == '*')
                    return jj * width + ii;
            }
        }

        return null;
    }
}
