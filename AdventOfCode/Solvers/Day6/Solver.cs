namespace AdventOfCode.Solvers.Day6;

public class Solver : ISolver
{
    public int GetDay()
    {
        return 6;
    }

    public string Solve(string[] input, bool showDebug)
    {
        var races = ParseRaces(input).ToList();
        if (showDebug)
        {
            foreach (var race in races)
                Console.WriteLine(race.ToString());
        }

        var numberOfWaysToWin = races.Select(x => GetNumberOfWaysToWin(x));
        var finalScore = numberOfWaysToWin.Aggregate((a, b) => a * b);
        return finalScore.ToString();

    }

    private int GetNumberOfWaysToWin(Race race)
    {
        //I DO IT BECAUSE I CAN
        return LongRange(1, race.Time + 1)
            .AsParallel()
            .Select(holdTime =>
            {
                var remainingTime = race.Time - holdTime;
                var speed = holdTime;
                var score = remainingTime * speed;
                return score > race.Score ? 1 : 0;
            })
            .Sum();
    }

    private IEnumerable<Race> ParseRaces(string[] input)
    {
        var times = input[0]["Time:".Length..].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)).ToArray();
        var scores = input[1]["Distance:".Length..].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)).ToArray();
        for (int i = 0; i < times.Length; i++)
        {
            yield return new Race
            {
                Time = times[i],
                Score = scores[i]
            };
        }
    }

    private IEnumerable<long> LongRange(long startIndex, long count)
    {
        for (long i = 0; i < count; i++)
            yield return startIndex + i;
    }
}

public class Race
{
    public long Time { get; internal set; }
    public long Score { get; internal set; }

    public override string ToString()
    {
        return $"{Time}: {Score}";
    }
}
