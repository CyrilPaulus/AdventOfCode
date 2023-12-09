using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeDay1b
{
    public class InputLoader
    {
        public List<char[]> LoadInputs()
        {
            return File.ReadAllLines("input.txt").Select(x => x.ToCharArray()).ToList();
        }
    }
}
