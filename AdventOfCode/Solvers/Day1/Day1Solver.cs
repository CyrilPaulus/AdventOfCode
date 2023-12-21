namespace AdventOfCode.Solvers.Day1;

public class Day1Solver : ISolver
{
    private readonly TreeNode _forwardTree;
    private readonly TreeNode _backwardTree;

    public int GetDay()
    {
        return 1;
    }

    public Day1Solver()
    {
        _forwardTree = new TreeNode();
        _backwardTree = new TreeNode();
        foreach (var word in GetWords())
        {
            var span = word.Item1.AsSpan();
            var backwardSpan = word.Item1.Reverse().ToArray().AsSpan();
            AddWord(_forwardTree, span, word.Item2);
            AddWord(_backwardTree, backwardSpan, word.Item2);
        }

        ComputeFailures(_forwardTree);
        ComputeFailures(_backwardTree);
    }

    public string Solve(string[] inputs, bool showDebug)
    {
        var total = 0;
        foreach (var line in inputs)
        {
            total += SolveLine(line, showDebug);
        }

        return total.ToString();
    }

    private int SolveLine(string line, bool showDebug)
    {
        var first = Solve(line, _forwardTree);
        var last = Solve(line.Reverse(), _backwardTree);

        if (showDebug)
            Console.WriteLine($"{new string(line)}: {first} {last}");

        return first * 10 + last;
    }

    private int Solve(IEnumerable<char> line, TreeNode root)
    {
        var currentNode = root;
        foreach (var c in line)
        {
            TreeNode trans = null;
            while (trans == null)
            {
                trans = currentNode.GetChild(c);
                if (currentNode == root)
                    break;

                if (trans == null)
                    currentNode = currentNode.Failure;
            }

            if (trans != null)
                currentNode = trans;

            if (currentNode.Results.Any())
                return currentNode.Results.First();
        }

        return -1;
    }



    private IEnumerable<Tuple<string, int>> GetWords()
    {
        for (int i = 1; i < 10; i++)
            yield return Tuple.Create($"{i}", i);
        yield return Tuple.Create("one", 1);
        yield return Tuple.Create("two", 2);
        yield return Tuple.Create("three", 3);
        yield return Tuple.Create("four", 4);
        yield return Tuple.Create("five", 5);
        yield return Tuple.Create("six", 6);
        yield return Tuple.Create("seven", 7);
        yield return Tuple.Create("eight", 8);
        yield return Tuple.Create("nine", 9);
    }

    private void AddWord(TreeNode root, ReadOnlySpan<char> str, int value)
    {
        var currentNode = root;
        foreach (var c in str)
        {
            var nextNode = currentNode.GetChild(c);
            if (nextNode == null)
                nextNode = currentNode.AddChild(c);

            currentNode = nextNode;
        }

        currentNode.Results.Add(value);
    }

    private void ComputeFailures(TreeNode root)
    {
        var queue = new Queue<TreeNode>();
        queue.Enqueue(root);
        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            ComputeFailures(root, currentNode);

            foreach (var childNode in currentNode.GetAllChildren())
                queue.Enqueue(childNode);
        }
        root.Failure = root;
    }

    private void ComputeFailures(TreeNode root, TreeNode currentNode)
    {
        // LEVEL 0
        if (currentNode == root)
            return;

        // LEVEL 1 Fail to root
        if (currentNode.Parent == root)
        {
            currentNode.Failure = root;
            return;
        }

        var r = currentNode.Parent!.Failure;
        var c = currentNode.Char;

        while (r != null && r.GetChild(c) == null)
            r = r.Failure;

        if (r == null)
        {
            currentNode.Failure = root;
            return;
        }

        currentNode.Failure = r.GetChild(c);
        currentNode.Results.AddRange(currentNode.Failure!.Results);
    }

}

public class TreeNode()
{
    private readonly Dictionary<char, TreeNode> _children = new();

    public char Char { get; set; }
    public List<int> Results { get; set; } = new();
    public TreeNode? Failure { get; internal set; }
    public TreeNode? Parent { get; private set; }

    public TreeNode? GetChild(char letter)
    {
        if (!_children.TryGetValue(letter, out var node))
            return null;

        return node;
    }

    public TreeNode AddChild(char letter)
    {
        var node = new TreeNode()
        {
            Char = letter,
            Parent = this
        };
        _children.Add(letter, node);
        return node;
    }

    public IEnumerable<TreeNode> GetAllChildren()
    {
        return _children.Values;
    }

}
