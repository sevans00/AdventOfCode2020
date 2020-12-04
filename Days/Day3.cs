using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    public class Day3
    {
        public static string inputFilePath = @"C:\Users\Shaun\Documents\Projects\AdventOfCode\AdventOfCode2020\AdventOfCode2020\Inputs\inputDay3.txt";
        public static string[] testInput = new string[]
        {
            "..##.......",
            "#...#...#..",
            ".#....#..#.",
            "..#.#...#.#",
            ".#...##..#.",
            "..#.##.....",
            ".#.#.#....#",
            ".#........#",
            "#.##...#...",
            "#...##....#",
            ".#..#...#.#",
        };

        public static void Part1()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;

            //Naive approach: //They used 1,1, i'm going to use 0,0.  Why?  'Cause I'm lazy and don't want any off-by-one-errors
            var xCoordinate = 0;
            var yCoordinate = 0;
            var treesHit = 0;
            foreach (var line in inputStrings)
            {
                //Did we make it?
                if(yCoordinate == inputStrings.Length)
                {
                    break;
                }

                //Test our location:
                var charAtLocation = line[xCoordinate];
                if(charAtLocation == '#')
                {
                    treesHit++;
                }
                
                //Move down one and right three:
                xCoordinate += 3;
                yCoordinate += 1;
                if(xCoordinate >= line.Length)
                {
                    xCoordinate -= line.Length;
                }
            }
            Console.WriteLine($"Trees hit: {treesHit}");
        }


        public static void Part2()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;

            var treesHitResults = new int[]
            {
                GetTreesHit(inputStrings, 1, 1),
                GetTreesHit(inputStrings, 3, 1),
                GetTreesHit(inputStrings, 5, 1),
                GetTreesHit(inputStrings, 7, 1),
                GetTreesHit(inputStrings, 1, 2),
            };

            var treesHitString = string.Join(", ", treesHitResults.Select(hits => hits.ToString()).ToArray());
            var treesHitMultiplication = treesHitResults.Aggregate(1, (x, y) => x * y);
            long product = 1;
            foreach (var number in treesHitResults)
            {
                product *= (long)number;
            }
            Console.WriteLine($"Trees hit: {treesHitString} multiplied = {product}");
        }

        private static int GetTreesHit(string[] inputStrings, int rightDistance, int downDistance)
        {
            var xCoordinate = 0;
            var treesHit = 0;
            for (int y = 0; y < inputStrings.Length; y++)
            {
                string line = inputStrings[y];
                //Test our location:
                var charAtLocation = line[xCoordinate];
                if (charAtLocation == '#')
                {
                    treesHit++;
                }

                //Move down one and right three:
                xCoordinate += rightDistance;
                y += downDistance - 1;
                if (xCoordinate >= line.Length)
                {
                    xCoordinate -= line.Length;
                }
            }
            return treesHit;
        }
    }
}
