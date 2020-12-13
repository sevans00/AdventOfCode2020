using AdventOfCode2020.Days;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    class Program
    {
        public static void Main(string[] args)
        {
            var startTime = DateTime.Now;
            Day13.Part3();
            var duration = DateTime.Now - startTime;

            Console.WriteLine($"---\n{duration}");
            Console.ReadLine();
        }
    }
}
