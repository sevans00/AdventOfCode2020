using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    public class Day15
    {

        public static string input1 = "6,13,1,15,2,0";
        public static string testInput0 = "0,3,6";
        public static string testInput1 = "1,3,2";
        public static string testInput2 = "2,1,3";
        public static string testInput3 = "2,3,1";
        public static string testInput4 = "3,2,1";
        public static string testInput5 = "3,1,2";

        public static void Part1()
        {
            var input = input1.Split(',').Select(num => int.Parse(num)).ToList();

            var lastSaidNumberToIndex = new Dictionary<int, List<int>>();
            var lastSaidNumber = 0;
            //Initialize:
            for (int ii = 0; ii < input.Count; ii++)
            {
                lastSaidNumber = input[ii];
                Console.WriteLine($"Turn {ii+1}:\t{lastSaidNumber}");
                if (lastSaidNumberToIndex.ContainsKey(lastSaidNumber))
                    lastSaidNumberToIndex[lastSaidNumber].Insert(0, ii);
                else
                    lastSaidNumberToIndex[lastSaidNumber] = new List<int>() { ii };
            }

            Console.WriteLine($"New numbers now!");

            for (int ii = input.Count; ii < 2020; ii++)
            {

                if (lastSaidNumberToIndex.TryGetValue(lastSaidNumber, out var lastSaidIndexes))
                {
                    var numTimesSpoken = lastSaidIndexes.Count();
                    if(numTimesSpoken == 1)
                    {
                        //New last said number:
                        lastSaidNumber = 0;
                        Console.WriteLine($"Turn {ii+1}:\t{lastSaidNumber}");
                        //Insert
                        if (lastSaidNumberToIndex.ContainsKey(lastSaidNumber))
                            lastSaidNumberToIndex[lastSaidNumber].Insert(0, ii);
                        else
                            lastSaidNumberToIndex[lastSaidNumber] = new List<int>() { ii };
                        continue;
                    }
                    var lastTimeSpoken = lastSaidIndexes[0];
                    var lastLastTimeSpoken = lastSaidIndexes[1];
                    lastSaidNumber = lastTimeSpoken - lastLastTimeSpoken;
                    Console.WriteLine($"Turn {ii + 1}:\t{lastSaidNumber}");
                    //Insert
                    if (lastSaidNumberToIndex.ContainsKey(lastSaidNumber))
                        lastSaidNumberToIndex[lastSaidNumber].Insert(0, ii);
                    else
                        lastSaidNumberToIndex[lastSaidNumber] = new List<int>() { ii };
                }
                else
                {
                    //first time speaking that number!
                    Console.WriteLine($"Turn {ii+1}:\t{lastSaidNumber} WHAT?  Why isn't this recorded?");
                    lastSaidNumberToIndex[lastSaidNumber] = new List<int>() { ii };
                    lastSaidNumber = 0;
                }
            }

            Console.WriteLine($"Last Spoken {lastSaidNumber}");

        }

        public static void Part2()
        {
            var input = input1.Split(',').Select(num => int.Parse(num)).ToList();

            var lastSaidNumberToIndex = new Dictionary<int, List<int>>();
            var lastSaidNumber = 0;
            //Initialize:
            for (int ii = 0; ii < input.Count; ii++)
            {
                lastSaidNumber = input[ii];
                Console.WriteLine($"Turn {ii + 1}:\t{lastSaidNumber}");
                if (lastSaidNumberToIndex.ContainsKey(lastSaidNumber))
                    lastSaidNumberToIndex[lastSaidNumber].Insert(0, ii);
                else
                    lastSaidNumberToIndex[lastSaidNumber] = new List<int>() { ii };
            }

            Console.WriteLine($"New numbers now!");

            for (int ii = input.Count; ii < 30000000; ii++)
            {
                if(ii % 1000000 == 0)
                    Console.WriteLine($"Turn {ii + 1}:\t{lastSaidNumber}");

                if (lastSaidNumberToIndex.TryGetValue(lastSaidNumber, out var lastSaidIndexes))
                {
                    var numTimesSpoken = lastSaidIndexes.Count();
                    if (numTimesSpoken == 1)
                    {
                        //New last said number:
                        lastSaidNumber = 0;
                        //Console.WriteLine($"Turn {ii + 1}:\t{lastSaidNumber}");
                        //Insert
                        if (lastSaidNumberToIndex.ContainsKey(lastSaidNumber))
                            lastSaidNumberToIndex[lastSaidNumber].Insert(0, ii);
                        else
                            lastSaidNumberToIndex[lastSaidNumber] = new List<int>() { ii };
                        continue;
                    }
                    var lastTimeSpoken = lastSaidIndexes[0];
                    var lastLastTimeSpoken = lastSaidIndexes[1];
                    lastSaidNumber = lastTimeSpoken - lastLastTimeSpoken;
                   // Console.WriteLine($"Turn {ii + 1}:\t{lastSaidNumber}");
                    //Insert
                    if (lastSaidNumberToIndex.ContainsKey(lastSaidNumber))
                        lastSaidNumberToIndex[lastSaidNumber].Insert(0, ii);
                    else
                        lastSaidNumberToIndex[lastSaidNumber] = new List<int>() { ii };
                }
                else
                {
                    //first time speaking that number!
                   // Console.WriteLine($"Turn {ii + 1}:\t{lastSaidNumber} WHAT?  Why isn't this recorded?");
                    lastSaidNumberToIndex[lastSaidNumber] = new List<int>() { ii };
                    lastSaidNumber = 0;
                }
            }

            Console.WriteLine($"Last Spoken {lastSaidNumber}");

        }

    }
}
