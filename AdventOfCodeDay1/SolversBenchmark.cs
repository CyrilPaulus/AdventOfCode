using AdventOfCodeDay1;
using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeDay1b
{
    public class SolversBenchmark
    {
        public SolversBenchmark()
        {
            
        }

        [Benchmark]
        public int SolveNaive()
        {
            var inputLoader = new InputLoader();
            var inputs = inputLoader.LoadInputs();
            var solver = new NaiveSolver();
            return solver.Solve(inputs);
        }

        [Benchmark]
        public int SolveNaiveParallel()
        {
            var inputLoader = new InputLoader();
            var inputs = inputLoader.LoadInputs();
            var solver = new NaiveSolver();
            return solver.SolveParallel(inputs);
        }

        [Benchmark]
        public int SolveTree()
        {
            var inputLoader = new InputLoader();
            var inputs = inputLoader.LoadInputs();
            var solver = new TreeSolver();
            return solver.Solve(inputs);
        }

        [Benchmark]
        public int SolveTreeParallel()
        {
            var inputLoader = new InputLoader();
            var inputs = inputLoader.LoadInputs();
            var solver = new TreeSolver();
            return solver.SolveParallel(inputs);
        }

        [Benchmark]
        public int SolveRegex()
        {
            var inputLoader = new InputLoader();
            var inputs = inputLoader.LoadInputs();
            var solver = new RegexSolver();
            return solver.Solve(inputs);
        }

        [Benchmark]
        public int SolveRegexParallel()
        {
            var inputLoader = new InputLoader();
            var inputs = inputLoader.LoadInputs();
            var solver = new RegexSolver();
            return solver.SolveParallel(inputs);
        }

    }
}
