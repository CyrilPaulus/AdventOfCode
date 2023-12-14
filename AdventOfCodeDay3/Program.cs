using AdventOfCodeDay3;

var inputLoader = new InputLoader();
var solver = new NaiveSolver();

var inputs = inputLoader.LoadInputs();
var result = solver.Solve(inputs);

Console.WriteLine($"Day 3 solution: {result}");