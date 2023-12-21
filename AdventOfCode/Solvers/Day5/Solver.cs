



using System.Net.NetworkInformation;
using System.Text;

namespace AdventOfCode.Solvers.Day5;

public class Solver : ISolver
{
    public int GetDay()
    {
        return 5;
    }

    public string Solve(string[] input, bool showDebug)
    {
        var seeds = ParseSeeds(input[0]);

        var seedBatchs = GetSeedBatches(seeds).ToList();

        if (showDebug)
            Console.WriteLine($"seeds: {string.Join(" ", seeds)}");

        var maps = ParseMaps(input);
        if (showDebug)
        {
            foreach (var map in maps)
                Console.WriteLine(map);
        }

        var min = long.MaxValue;
        foreach (var seedBatch in seedBatchs)
        {
            var locationBatches = GetLocationBatches(seedBatch, maps);
            min = Math.Min(min, locationBatches.Min(x => x.StartIndex));
        }

        return min.ToString();

    }

    private IEnumerable<Batch> GetSeedBatches(long[] seeds)
    {
        for (int i = 0; i < seeds.Length; i += 2)
            yield return new Batch() { StartIndex = seeds[i], EndIndex = seeds[i] + seeds[i+1] - 1 };
    }  

    private List<Batch> GetLocationBatches(Batch seedBatch, List<AlamanacMap> maps)
    {
        var currentBatches = new List<Batch> { seedBatch };
        foreach (var map in maps)
            currentBatches = GetDestinationBatches(currentBatches, map);

        return currentBatches;
    }

    private List<Batch> GetDestinationBatches(List<Batch> currentBatches, AlamanacMap map)
    {
        return currentBatches.SelectMany(x => GetDestinationBatches(x, map)).ToList();
    }

    private IEnumerable<Batch> GetDestinationBatches(Batch currentBatch, AlamanacMap map)
    {
        foreach (var rule in map.Rules)
        {
            var previousBatch = rule.SourceBatch.Before(currentBatch);
            if (previousBatch != null)
                yield return previousBatch;

            var overlappingBatch = rule.SourceBatch.Intersect(currentBatch);
            if (overlappingBatch != null)
                yield return overlappingBatch.Offset(rule.DestinationStartIndex - rule.SourceBatch.StartIndex);

            var nextBatch = rule.SourceBatch.After(currentBatch);
            if (nextBatch == null)
                yield break;

            currentBatch = nextBatch;
        }

        yield return currentBatch;
    }

    private List<AlamanacMap> ParseMaps(string[] input)
    {
        var rtn = new List<AlamanacMap>();
        var currentIndex = 2;
        while (currentIndex < input.Length)
        {
            currentIndex = ParseMaps(input, currentIndex, out var map);
            rtn.Add(map);
        }
        return rtn;
    }

    private int ParseMaps(string[] input, int currentIndex, out AlamanacMap map)
    {
        map = new AlamanacMap();
        map.Label = input[currentIndex][..^4];
        currentIndex++;

        map.Rules = new();
        do
        {
            var currentLine = input[currentIndex];
            if (string.IsNullOrWhiteSpace(currentLine))
                break;
            map.Rules.Add(ParseRule(currentLine));
            currentIndex++;
        } while (currentIndex < input.Length);

        map.Rules = map.Rules.OrderBy(x => x.SourceBatch.StartIndex).ToList();
        return currentIndex + 1;
    }

    private AlamanacConversionRule ParseRule(string currentLine)
    {
        var data = currentLine.Split(" ");

        var destinationIndex = long.Parse(data[0]);
        var sourceIndex = long.Parse(data[1]);
        var length = long.Parse(data[2]);

        return new AlamanacConversionRule()
        {
            SourceBatch = new Batch { StartIndex = sourceIndex, EndIndex = sourceIndex + length - 1},
            DestinationStartIndex = destinationIndex
        };
    }

    private long[] ParseSeeds(string line)
    {
        return line.Split(":")[1].Trim().Split(" ").Select(x => long.Parse(x)).ToArray();
    }




}

public class AlamanacMap
{
    public string Label { get; set; }
    public List<AlamanacConversionRule> Rules { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine(Label);
        foreach (var rule in Rules)
            sb.AppendLine(rule.ToString());
        return sb.ToString();
    }
}

public class AlamanacConversionRule
{
    public Batch SourceBatch { get; set; }
    public long DestinationStartIndex { get; set; }

    public override string ToString()
    {
        return $"[{SourceBatch.StartIndex}, {SourceBatch.EndIndex}] -> [{DestinationStartIndex}]";
    }
}

public class Batch
{
    public long StartIndex { get; set; }
    public long EndIndex { get; set; }
    public long Count { get => EndIndex - StartIndex + 1; }

    public override string ToString()
    {
        return $"[{StartIndex}, {EndIndex}]";
    }


    /// <summary>
    /// Return the intersection of two batch, null if no such batch exists
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public Batch? Intersect(Batch other)
    {
        var rtn = new Batch()
        {
            StartIndex = Math.Max(StartIndex, other.StartIndex),
            EndIndex = Math.Min(EndIndex, other.EndIndex)
        };

        return rtn.Count >= 1 ? rtn : null;
    }

    /// <summary>
    /// Return the batch from other before current
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public Batch? Before(Batch other)
    {
        var rtn = new Batch()
        {
            StartIndex = other.StartIndex,
            EndIndex = Math.Min(StartIndex - 1, other.EndIndex)
        };

        return rtn.Count >= 1 ? rtn : null;
    }

    /// <summary>
    /// Return the batch from other after current
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public Batch? After(Batch other)
    {
        var rtn = new Batch()
        {
            StartIndex = Math.Max(EndIndex + 1, other.StartIndex),
            EndIndex = other.EndIndex
        };

        return rtn.Count >= 1 ? rtn : null;
    }

    internal Batch Offset(long offset)
    {
        return new Batch()
        {
            StartIndex = StartIndex + offset,
            EndIndex = EndIndex + offset
        };
    }
}
