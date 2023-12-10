using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeDay1b
{
    public class NaiveSolver
    {
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

        int ExtractTotal(string line)
        {
            var searchStrings = new List<string>()
            {
                "0","1","2","3","4","5","6","7","8","9",
                "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"
            };

            var firstDigit = FirstIndexOf(line, searchStrings);
            var lastDigit = new string(FirstIndexOf(new string(line.Reverse().ToArray()), searchStrings.Select(x => new string(x.Reverse().ToArray())).ToArray()).Reverse().ToArray());

#if DEBUG
            Console.WriteLine($"{line}: {parseDigit(firstDigit)} {parseDigit(lastDigit)}");
#endif

            return parseDigit(firstDigit) * 10 + parseDigit(lastDigit);
        }

        int parseDigit(string digit)
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

        string FirstIndexOf(string input, IEnumerable<string> searchItems)
        {
            var currentIndex = -1;
            string matchingString = null;

            foreach (var searchItem in searchItems)
            {
                var indexOfItem = input.IndexOf(searchItem);
                if (indexOfItem == -1)
                    continue;

                if (indexOfItem >= currentIndex && currentIndex != -1)
                    continue;

                currentIndex = indexOfItem;
                matchingString = searchItem;
            }

            return matchingString;

        }
    }
}
