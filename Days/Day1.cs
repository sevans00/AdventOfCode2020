using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    public static class Day1
    {
        public static string inputFilePath = @"C:\Users\Shaun\Documents\Projects\AdventOfCode\AdventOfCode2020\AdventOfCode2020\Day1\input.txt";
        public static int[] testInput = new int[]
        {
            1721,
            979,
            366,
            299,
            675,
            1456
        };

        public static void Part1()
        {
            var input = getInputsFromFile(inputFilePath);

            //Naive implementation:
            foreach (var number1 in input)
            {
                foreach (var number2 in input)
                {
                    if (number1 == number2)
                        continue;

                    var sum = number1 + number2;
                    if (sum == 2020)
                    {
                        Console.WriteLine($"Numbers: {number1}+{number2}={sum}, and their product is {number1 * number2}");
                        return;
                    }
                }
            }
        }
        public static void Part2()
        {
            var input = getInputsFromFile(inputFilePath);

            //Naive implementation:
            foreach (var number1 in input)
            {
                foreach (var number2 in input)
                {
                    if (number1 == number2)
                        continue;
                    foreach (var number3 in input)
                    {
                        if (number1 == number3) //mmmm... premature optimization...
                            continue;

                        var sum = number1 + number2 + number3;
                        if (sum == 2020)
                        {
                            Console.WriteLine($"Numbers: {number1}+{number2}+{number3}={sum}, and their product is {number1 * number2 * number3}");
                            return;
                        }
                    }
                }
            }
        }

        public static int[] getInputsFromFile(string filepath)
        {
            var lines = File.ReadAllLines(filepath);
            return lines.Select(line => int.Parse(line)).ToArray();
        }


    }
}
