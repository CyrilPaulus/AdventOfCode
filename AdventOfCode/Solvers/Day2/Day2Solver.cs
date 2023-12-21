namespace AdventOfCode.Solvers.Day2;

public class Day2Solver : ISolver
{
    public int GetDay()
    {
        return 2;
    }

    public string Solve(string[] input, bool showDebug)
    {
        var availableColors = new Dictionary<Color, int>() {
            {Color.Red, 12},
            {Color.Green, 13},
            {Color.Blue, 14}
        };

        var games = input.Select(x => ParseLine(x));
        var total = games.Sum(x => GetPower(x));

        foreach (var game in games)
            Console.WriteLine(game);

        return total.ToString();
    }

    private int IsValid(Game game, Dictionary<Color, int> availableColors)
    {
        foreach (var color in Enum.GetValues<Color>())
        {
            var total = game.Sets.Max(x => x.Colors.GetValueOrDefault(color, 0));
            var available = availableColors.GetValueOrDefault(color, 0);
            if (total > available)
                return 0;
        }

        return game.Id;
    }

    private int GetPower(Game game)
    {
        var power = 1;
        foreach (var color in Enum.GetValues<Color>())
        {
            //Min is max, deal with it
            var min = game.Sets.Max(x => x.Colors.GetValueOrDefault(color, 0));
            power *= min;
        }
        return power;
    }

    private Game ParseLine(string line)
    {
        var data = line.Split(':');
        var game = ParseGamePart(data[0]);
        game.Sets.AddRange(ParseSets(data[1]));
        return game;
    }

    private IEnumerable<GameSet> ParseSets(string setsStr)
    {
        return setsStr.Split(";").Select(x => ParseSet(x));
    }

    private GameSet ParseSet(string setStr)
    {
        var set = new GameSet();
        var colorsStr = setStr.Split(',');
        foreach (var colorStr in colorsStr)
            UpdateColor(set, colorStr.Trim());

        return set;
    }

    private void UpdateColor(GameSet set, string colorStr)
    {
        var colorData = colorStr.Split(" ");
        var colorValue = int.Parse(colorData[0]);
        var colorEnum = Enum.Parse<Color>(colorData[1], true);
        set.Colors.Add(colorEnum, colorValue);
    }

    private Game ParseGamePart(string gameStr)
    {
        var game = new Game();
        game.Id = int.Parse(gameStr.Split(' ')[1]);
        return game;
    }

    private class Game
    {
        public int Id { get; set; }

        public List<GameSet> Sets { get; set; } = new();

        public override string ToString()
        {
            return $"game {Id}: {string.Join(";", Sets)}";
        }
    }

    private class GameSet
    {
        public Dictionary<Color, int> Colors { get; set; } = new();

        public override string ToString()
        {
            return string.Join(",", Colors.Where(x => x.Value > 0).Select(x => $"{x.Value} {x.Key.ToString().ToLower()}"));
        }

    }

    private enum Color
    {
        Red,
        Green,
        Blue
    }
}
