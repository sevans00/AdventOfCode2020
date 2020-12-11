using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    public class Day10
    {
        public static string inputFilePath = @"C:\Users\Shaun\Documents\Projects\AdventOfCode\AdventOfCode2020\AdventOfCode2020\Inputs\inputDay10.txt";

        /*
           With these adapters, your device's built-in joltage adapter would be rated for 19 + 3 = 22 jolts, 3 higher than the highest-rated adapter.

           Because adapters can only connect to a source 1-3 jolts lower than its rating, in order to use every adapter, you'd need to choose them.

            In this example, when using every adapter, there are 7 differences of 1 jolt and 5 differences of 3 jolts.
        */
        public static string[] testInput = new string[]
        {
            "16",
            "10",
            "15",
            "5",
            "1",
            "11",
            "7",
            "19",
            "6",
            "12",
            "4",
        };
        public static string[] testInput2 = new string[]
        {
            "28",
            "33",
            "18",
            "42",
            "31",
            "14",
            "46",
            "20",
            "48",
            "47",
            "24",
            "23",
            "49",
            "45",
            "19",
            "38",
            "39",
            "11",
            "1 ",
            "32",
            "25",
            "35",
            "8 ",
            "17",
            "7 ",
            "9 ",
            "4 ",
            "2 ",
            "34",
            "10",
            "3 ",
        };


        public static void Part1()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;
            //var inputStrings = testInput2;

            var inputNumbers = inputStrings.Select(input => int.Parse(input)).ToList();

            inputNumbers.Sort();
            inputNumbers.Insert(0, 0);
            inputNumbers.Add(inputNumbers.Max() + 3);
            var differences = new List<int>();
            for (int ii = 0; ii < inputNumbers.Count; ii++)
            {
                if (ii == inputNumbers.Count - 1)
                {
                    continue;
                }
                var number = inputNumbers[ii];
                var nextNumber = inputNumbers[ii + 1];
                differences.Add(nextNumber - number);
            }

            differences.Sort();
            Console.WriteLine($"{string.Join(", ", differences)}");

            var differenceCounts = from difference in differences
                                   group difference by difference into newGroup
                                   orderby newGroup.Key
                                   select newGroup;

            var oneDiffCounts = 0;
            var threeDiffCounts = 0;
            foreach (var result in differenceCounts)
            {
                var countOfResult = differences.Count(ii => ii == result.Key);
                if (result.Key == 1)
                    oneDiffCounts = countOfResult;
                if (result.Key == 3)
                    threeDiffCounts = countOfResult;
                Console.WriteLine($"{result.Key}: {countOfResult}");
            }

            Console.WriteLine($"{string.Join(", ", differenceCounts)}");

            Console.WriteLine($"1-jolts * 3-jolts = {oneDiffCounts * threeDiffCounts}");

        }

        public static void Part2()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;
            //var inputStrings = testInput2;

            var inputNumbers = inputStrings.Select(input => int.Parse(input)).ToList();

            inputNumbers.Sort();
            inputNumbers.Insert(0, 0);
            inputNumbers.Add(inputNumbers.Max() + 3);

            //Figure out permutations:
            //Max diff = 3
            //Other diff = 2
            //Min diff = 1

            var differences = new List<int>();
            for (int ii = 0; ii < inputNumbers.Count; ii++)
            {
                if (ii == inputNumbers.Count - 1)
                {
                    continue;
                }
                var number = inputNumbers[ii];
                var nextNumber = inputNumbers[ii + 1];
                differences.Add(nextNumber - number);
            }
            differences.Sort();
            Console.WriteLine($"{string.Join(", ", differences)}");

            var differenceCounts = from difference in differences
                                   group difference by difference into newGroup
                                   orderby newGroup.Key
                                   select newGroup;

            var oneDiffCounts = 0;
            var twoDiffCounts = 0;
            var threeDiffCounts = 0;
            foreach (var result in differenceCounts)
            {
                var countOfResult = differences.Count(ii => ii == result.Key);
                if (result.Key == 1)
                    oneDiffCounts = countOfResult;
                if (result.Key == 2)
                    twoDiffCounts = countOfResult;
                if (result.Key == 3)
                    threeDiffCounts = countOfResult;
                Console.WriteLine($"{result.Key}: {countOfResult}");
            }



            //Permutations:
            /*
            Default case:
            (0),1,(4) => 1

            (0),1,2,(5) => 2 (could have just the 2 or the 1 and the 2)

            (0),1,3,(6) => 2

            (0),1,4,(7) => 1  (Any 3 gap contributes only 1 path?)

            (0),2,3,(6) => 2

            (0),1,2,3,(6) => 3

            (0),1,2,3,4,(7) => 4

            (0),1,2,3,4,5,(8) => 11
                    (0),1,2,3,4,5,(8)
                    (0),1,3,4,5,(8)
                    (0),1,3,5,(8)
                    (0),1,2,3,5,(8)
                    (0),1,4,5,(8)
                    (0),2,3,4,5,(8)
                    (0),2,4,5,(8)
                    (0),2,5,(8)
                    (0),3,4,5,(8)
                    (0),3,5,(8)
                    (0),1,3,4,5,(8)
            
            */

            //Assume we have one of every jolt-number of adaptor:
            /*
            Huh?
            This was a struggle for me to figure out.
            */

            var inputMax = inputNumbers.Max();
            var permutationsPerNumber = new Dictionary<int, long>() { {0, 1} };
            long lastPermutation = 0;
            for (var ii = 1; ii < inputMax; ii++)
            {
                var target = ii;
                var targetM1 = ii - 1;
                var targetM2 = ii - 2;
                var targetM3 = ii - 3;

                long permutationsOfM1 = 0;
                long permutationsOfM2 = 0;
                long permutationsOfM3 = 0;

                if(!inputNumbers.Contains(target))
                {
                    continue;
                }

                if (targetM1 >= 0 && inputNumbers.Contains(targetM1) && permutationsPerNumber.TryGetValue(targetM1, out var permutation1))
                {
                    permutationsOfM1 = permutation1;
                }
                if (targetM2 >= 0 && inputNumbers.Contains(targetM2) && permutationsPerNumber.TryGetValue(targetM2, out var permutation2))
                {
                    permutationsOfM2 = permutation2;
                }
                if (targetM3 >= 0 && inputNumbers.Contains(targetM3) && permutationsPerNumber.TryGetValue(targetM3, out var permutation3))
                {
                    permutationsOfM3 = permutation3;
                }
                permutationsPerNumber[target] = permutationsOfM1 + permutationsOfM2 + permutationsOfM3;
                lastPermutation = permutationsPerNumber[target];
            }

            Console.WriteLine($"Permutations: {lastPermutation}?");
        }

    }
}
