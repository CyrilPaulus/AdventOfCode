using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCodeDay1
{
    public partial class RegexSolver
    {
        private readonly Regex _forwardRegex;
        private readonly Regex _backwardRegex;

        public RegexSolver()
        {
            _forwardRegex = ForwardRegex();
            _backwardRegex = BackwardRegex();            
        }

        public int Solve(string[] inputs)
        {
            var total = 0;
            foreach (var line in inputs)
                total += ExtractTotal(line);

            return total;
        }

        public int SolveParallel(string[] inputs)
        {
            return inputs.AsParallel().Sum(x => ExtractTotal(x));
        }

        private int ExtractTotal(string line)
        {
            var firstNumber = _forwardRegex.Match(line).Value;
            var lastNumber = _backwardRegex.Match(new string(line.Reverse().ToArray())).Value;

            return ParseDigit(firstNumber) * 10 + ParseDigit(new string(lastNumber.Reverse().ToArray()));
        }

        int ParseDigit(string digit)
        {
            return digit switch
            {
                "1" or "2" or "3" or "4" or "5" or "6" or "7" or "8" or "9" => int.Parse(digit),
                "one" => 1,
                "two" => 2,
                "three" => 3,
                "four" => 4,
                "five" => 5,
                "six" => 6,
                "seven" => 7,
                "eight" => 8,
                "nine" => 9,
                _ => throw new NotImplementedException()
            };
        }

        [GeneratedRegex("[1-9]|one|two|three|four|five|six|seven|eight|nine")]
        private static partial Regex ForwardRegex();
        [GeneratedRegex("[1-9]|eno|owt|eerht|ruof|evif|xis|neves|thgie|enin")]
        private static partial Regex BackwardRegex();
    }
}
