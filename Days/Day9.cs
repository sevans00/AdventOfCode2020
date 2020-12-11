using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    public class Day9
    {
        public static string inputFilePath = @"C:\Users\Shaun\Documents\Projects\AdventOfCode\AdventOfCode2020\AdventOfCode2020\Inputs\inputDay9.txt";
        public static string[] testInput = new string[]
        {
            "35",
            "20",
            "15",
            "25",
            "47",
            "40",
            "62",
            "55",
            "65",
            "95",
            "102",
            "117",
            "150",
            "182",
            "127",
            "219",
            "299",
            "277",
            "309",
            "576",
        };


        public static void Part1()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;

            var inputNumbers = inputStrings.Select(input => Int64.Parse(input)).ToList();

            var preambleLength = 25;

            var sumRange = inputNumbers.GetRange(0, preambleLength);
            inputNumbers.RemoveRange(0, preambleLength);

            for (int ii = 0; ii < inputNumbers.Count; ii++)
            {
                var inputNumber = inputNumbers[ii];
                var isInSums = IsNumberInSums(sumRange, inputNumber);

                Console.WriteLine($"Number {inputNumber}: {isInSums}");
                if (!isInSums)
                    break;

                sumRange.RemoveAt(0);
                sumRange.Add(inputNumber);
            }
        }


        public static void Part2()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;
            var preambleLength = 25;

            var inputNumbers = inputStrings.Select(input => Int64.Parse(input)).ToList();


            var preambleRange = inputNumbers.GetRange(0, preambleLength);
            inputNumbers.RemoveRange(0, preambleLength);

            Int64 invalidNumber = 0;
            for (int ii = 0; ii < inputNumbers.Count; ii++)
            {
                invalidNumber = inputNumbers[ii];
                var isInSums = IsNumberInSums(preambleRange, invalidNumber);

                if (!isInSums)
                {
                    break;
                }

                preambleRange.RemoveAt(0);
                preambleRange.Add(invalidNumber);
            }



            inputNumbers = inputStrings.Select(input => Int64.Parse(input)).ToList();

            Console.WriteLine($"Number {invalidNumber}");
            var contiguousNumbers = GetContiguousNumbersThatSumTo(inputNumbers, invalidNumber);
            if(contiguousNumbers.Count == 0)
            {
                Console.WriteLine($"Didn't find any!");
                return;
            }

            var smallest = contiguousNumbers.OrderBy(x => x).First();
            var biggest = contiguousNumbers.OrderBy(x => x).Last();
            var sum = smallest + biggest;

            Console.WriteLine($"Contiguous Numbers: {string.Join(", ", contiguousNumbers)}");

            Console.WriteLine($"Sum of First And Last: {sum}");
        }


        private static List<Int64> GetContiguousNumbersThatSumTo(List<Int64> inputNumbers, Int64 sumNumber)
        {
            Int64 partialSum = 0;
            var contiguousNumbers = new List<Int64>();
            for (var ii = 0; ii < inputNumbers.Count; ii++)
            {
                contiguousNumbers = new List<Int64>();
                partialSum = 0;

                for (var jj = ii; jj < inputNumbers.Count; jj++)
                {
                    partialSum += inputNumbers[jj];
                    contiguousNumbers.Add(inputNumbers[jj]);
                    if (contiguousNumbers.Count == 1)
                        continue;
                    if (partialSum > sumNumber)
                    {
                        Console.WriteLine($"Sum {partialSum} Too big for {sumNumber}!");
                        break;
                    }
                    if (partialSum == sumNumber)
                    {
                        return contiguousNumbers;
                    }
                }
            }
            return new List<Int64>();
        }


        private static bool IsNumberInSums(List<Int64> sumRange, Int64 nextNum)
        {
            for (var ii = 0; ii < sumRange.Count; ii++)
            {
                var number1 = sumRange[ii];
                for (var jj = 0; jj < sumRange.Count; jj++)
                {
                    if (ii == jj)
                        continue;
                    var number2 = sumRange[jj];
                    var sum = number1 + number2;
                    if (sum == nextNum)
                        return true;
                }
            }
            return false;
        }

        private static List<int> GetSumsIn(List<int> sumRange)
        {
            var sums = new List<int>();
            for (var ii = 0; ii < sumRange.Count; ii++)
            {
                var number1 = sumRange[ii];
                for (var jj = 0; jj < sumRange.Count; jj++)
                {
                    if (ii == jj)
                        continue;
                    var number2 = sumRange[jj];
                    var sum = number1 + number2;
                    
                }
            }
            return sums;
        }
    }
}
