﻿// See https://aka.ms/new-console-template for more information


using AdventOfCodeDay1b;
using BenchmarkDotNet.Running;

var summary = BenchmarkRunner.Run<SolversBenchmark>();


//var inputLoader = new InputLoader();
//var inputs = inputLoader.LoadInputs();
//var solver = new NaiveSolver();
//var result = solver.Solve(inputs);
//Console.WriteLine(result);