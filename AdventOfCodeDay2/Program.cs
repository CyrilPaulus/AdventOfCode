using AdventOfCodeDay2;

var inputLoader = new InputLoader();
var solver = new NaiveSolver();

var inputs = inputLoader.LoadInputs();
var result = solver.Solve(inputs);

Console.WriteLine($"Day 2 solution: {result}");
