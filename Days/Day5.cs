using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    public class Day5
    {
        public static string inputFilePath = @"C:\Users\Shaun\Documents\Projects\AdventOfCode\AdventOfCode2020\AdventOfCode2020\Inputs\inputDay5.txt";
        public static string[] testInput = new string[]
        {
            "BFFFBBFRRR",
            "FFFBBBFRRR",
            "BBFFBBFRLL",

        };


        public static void Part1()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;

            var boardingPasses = new List<BoardingPass>();
            foreach (var inputString in inputStrings)
            {
                boardingPasses.Add(new BoardingPass(inputString));
            }
            var maxSeatId = boardingPasses.Max(pass => pass.SeatId);
            Console.WriteLine(maxSeatId);
        }


        public static void Part2()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;

            var boardingPasses = new List<BoardingPass>();
            foreach (var inputString in inputStrings)
            {
                boardingPasses.Add(new BoardingPass(inputString));
            }

            boardingPasses = boardingPasses.OrderBy(pass => pass.SeatId).ToList();

            var lastSeatId = boardingPasses[0].SeatId;
            foreach (var pass in boardingPasses)
            {
                if (lastSeatId == pass.SeatId) //This is dumb, but i don't care
                    continue;
                if (lastSeatId == pass.SeatId - 2)
                {
                    Console.WriteLine($"Gap found between {lastSeatId} and {pass.SeatId}!");
                }
                lastSeatId = pass.SeatId;
            }
        }



        public class BoardingPass
        {
            public string SeatInput;
            public int Row;
            public int Column;
            public int SeatId;

            public BoardingPass(string seatInput)
            {
                SeatInput = seatInput;
                //Now to parse everything else:
                var rowCharacters = SeatInput.Substring(0, 7);
                Row = ParseBinaryFromCharacters(rowCharacters, 'B');
                var columnCharacters = SeatInput.Substring(7, 3);
                Column = ParseBinaryFromCharacters(columnCharacters, 'R');
                SeatId = Row * 8 + Column;
            }

            private static int ParseBinaryFromCharacters(string characters, char onesChar)
            {
                var num = 0;
                foreach (var character in characters)
                {
                    num <<= 1;
                    if (character == onesChar)
                    {
                        num += 1;
                    }
                }
                return num;
            }

            public override string ToString()
            {
                return $"{SeatInput}: row {Row}, column {Column}, sead ID {SeatId}";
            }
        }
    }
}
