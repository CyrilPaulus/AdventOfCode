using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode.Solvers.Day4;

public class Solver : ISolver
{
    public int GetDay()
    {
        return 4;
    }

    public string Solve(string[] input, bool showDebug)
    {
        var cards = input.Select(x => ParseCard(x)).ToArray();
        var queue = new Queue<Card>();
        foreach (var card in cards)
            queue.Enqueue(card);

        var total = cards.Length;

        while (queue.Count > 0)
        {
            var card = queue.Dequeue();
            var intersectingCount = GetMatchingCount(card);
            if (intersectingCount == 0)
                continue;

            if (showDebug)
                Console.WriteLine($"Card {card.Id}: {intersectingCount}");

            total += intersectingCount;
            for (int i = 0; i < intersectingCount; i++)
            {
                var nextCard = cards[card.Id + i];
                if (showDebug)
                    Console.WriteLine($"\t->{nextCard.Id}");
                queue.Enqueue(nextCard);
            }
        }


        return total.ToString();
    }

    public string SolveA(string[] input, bool showDebug)
    {
        var total = 0;
        foreach (var line in input)
        {
            var card = ParseCard(line);
            var score = GetScore(card);
            if (showDebug)
                Console.WriteLine($"{card} -> {score}");

            total += score;
        }

        return total.ToString();
    }

    private Card ParseCard(string line)
    {
        var rtn = new Card();
        var cardParts = line.Split(":");
        rtn.Id = int.Parse(cardParts[0]["Card ".Length..]);

        var numberSeries = cardParts[1].Split("|");
        rtn.WinningNumbers = ParseNumbers(numberSeries[0]);
        rtn.OwnNumbers = ParseNumbers(numberSeries[1]);

        return rtn;
    }

    private int GetScore(Card card)
    {
        var intersectingCount = GetMatchingCount(card);
        if (intersectingCount == 0)
            return 0;

        return (int)Math.Pow(2, intersectingCount - 1);
    }

    private int GetMatchingCount(Card card)
    {
        return card.WinningNumbers.Intersect(card.OwnNumbers).Count();
    }

    private int[] ParseNumbers(string v)
    {
        return v.Trim()
        .Split(" ")
        .Where(x => !string.IsNullOrWhiteSpace(x))
        .Select(x => int.Parse(x.Trim()))
        .ToArray();
    }

    public class Card
    {
        public int Id { get; set; }
        public int[] WinningNumbers { get; set; }
        public int[] OwnNumbers { get; set; }

        public override string ToString()
        {
            return $"Card {Id}: {string.Join(" ", WinningNumbers)} | {string.Join(" ", OwnNumbers)}";
        }
    }
}
