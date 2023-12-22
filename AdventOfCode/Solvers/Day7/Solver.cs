using System.Security.Cryptography;

namespace AdventOfCode.Solvers.Day7;

public class Solver : ISolver
{
    public int GetDay()
    {
        return 7;
    }

    public string Solve(string[] input, bool showDebug)
    {
        var hands = ParseHands(input).Order().ToList();
        if (showDebug)
            hands.ForEach(x => Console.WriteLine(x));

        var score = hands.Select((hand, index) => (index + 1) * hand.Bid).Sum();
        return score.ToString();
    }

    private List<Hand> ParseHands(string[] input)
    {
        return input.Select(x => ParseHand(x)).ToList();
    }

    private Hand ParseHand(string line)
    {
        var data = line.Split(" ");
        return new Hand
        {
            Cards = ParseCards(data[0]),
            Bid = int.Parse(data[1])
        };
    }

    private List<Card> ParseCards(string v)
    {
        return v.Select(x => new Card() { Label = x.ToString() }).ToList();
    }


}

public class Hand : IComparable<Hand>
{
    public List<Card> Cards { get; set; }
    public int Bid { get; set; }

    public override string ToString()
    {
        return $"{string.Join("", Cards)} {Bid}";
    }

    private HandType GetHandType()
    {
        var cardCounts = Cards
            .GroupBy(x => x.Label)
            .Select(x => new CardCount
            {
                Label = x.Key,
                Count = x.Count()
            })
            .OrderByDescending(x => x.Count)
            .ToList();

        var jCardCount = cardCounts.FirstOrDefault(x => x.Label == "J");
        if (jCardCount != null && cardCounts.Count > 1)
        {
            cardCounts.Remove(jCardCount);
            cardCounts[0].Count += jCardCount.Count;
        }

        return (cardCounts.Count, cardCounts[0].Count) switch
        {
            (1, _) => HandType.FiveOfAKind,
            (2, 4) => HandType.FourOfAKind,
            (2, 3) => HandType.FullHouse,
            (3, 3) => HandType.ThreeOfAKind,
            (3, 2) => HandType.TwoPairs,
            (4, 2) => HandType.OnePair,
            (5, _) => HandType.HighCard,
            _ => throw new InvalidOperationException("This hand shouldn't exits")
        };
    }

    public int CompareTo(Hand? other)
    {
        var myScore = GetHandType();
        var otherScore = other.GetHandType();

        if (myScore < otherScore)
            return -1;

        if (myScore > otherScore)
            return 1;

        for (int i = 0; i < Cards.Count; i++)
        {
            var myCard = Cards[i];
            var otherCard = other.Cards[i];

            if (myCard.Label == otherCard.Label)
                continue;

            return myCard.Score - otherCard.Score;

        }

        return 0;
    }
}

public class CardCount
{
    public string Label { get; set; }
    public int Count { get; set; }
}

public class Card
{
    public string Label { get; set; }
    public int Score
    {
        get
        {
            if (int.TryParse(Label, out var score))
                return score;

            return Label switch
            {
                "T" => 10,
                "J" => 0,
                "Q" => 12,
                "K" => 13,
                "A" => 14,
                _ => throw new NotImplementedException()
            };
        }
    }

    public override string ToString()
    {
        return Label;
    }
}

public enum HandType
{
    HighCard = 0,
    OnePair = 1,
    TwoPairs = 2,
    ThreeOfAKind = 3,
    FullHouse = 4,
    FourOfAKind = 5,
    FiveOfAKind = 6
}
