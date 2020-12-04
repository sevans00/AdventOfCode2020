using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    public static class Day2
    {
        public static string inputFilePath = @"C:\Users\Shaun\Documents\Projects\AdventOfCode\AdventOfCode2020\AdventOfCode2020\Inputs\inputDay2.txt";
        public static string[] testInput = new string[]
        {
            "1-3 a: abcde",
            "1-3 b: cdefg",
            "2-9 c: ccccccccc"
        };

        public static void Part1()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;

            var validCount = 0;
            foreach (var inputString in inputStrings)
            {
                var entry = PasswordEntry.Parse(inputString);
                Console.WriteLine($"Line: {inputString} isValid: {entry.IsValid()}");
                if (entry.IsValid())
                {
                    validCount++;
                }
            }
            Console.WriteLine($"Total Valid: {validCount}");
        }

        public class PasswordEntry
        {
            public int minLetters;
            public int maxLetters;
            public char letter;
            public string password;

            public bool IsValid()
            {
                var charCount = password.Count(character => (character == letter));
                return (charCount >= minLetters 
                    && charCount <= maxLetters);
            }

            public static PasswordEntry Parse(string inputString)
            {
                //"1-3 a: abcde"
                var splitInput = inputString.Split(':');
                var password = splitInput[1].Trim();
                var splitPolicy = splitInput[0].Split(' ');
                var character = splitPolicy[1].Trim()[0];
                var splitMinMax = splitPolicy[0].Split('-');
                var minLetters = int.Parse(splitMinMax[0]);
                var maxLetters = int.Parse(splitMinMax[1]);

                return new PasswordEntry()
                {
                    letter = character,
                    minLetters = minLetters,
                    maxLetters = maxLetters,
                    password = password
                };
            }
        }


        public static void Part2()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;

            var validCount = 0;
            foreach (var inputString in inputStrings)
            {
                var entry = PasswordEntry2.Parse(inputString);
                Console.WriteLine($"Line: {inputString} isValid: {entry.IsValid()}");
                if (entry.IsValid())
                {
                    validCount++;
                }
            }
            Console.WriteLine($"Total Valid: {validCount}");
        }


        public class PasswordEntry2
        {
            public int letterIndex1;
            public int letterIndex2;
            public char letter;
            public string password;

            public bool IsValid()
            {
                var index1 = letterIndex1 - 1;
                var index2 = letterIndex2 - 1;
                var charAtIndex1 = password[index1];
                var charAtIndex2 = password[index2];
                if (charAtIndex1 == charAtIndex2)
                    return false;
                return (charAtIndex1 == letter || charAtIndex2 == letter);
            }

            public static PasswordEntry2 Parse(string inputString)
            {
                //"1-3 a: abcde"
                var splitInput = inputString.Split(':');
                var password = splitInput[1].Trim();
                var splitPolicy = splitInput[0].Split(' ');
                var character = splitPolicy[1].Trim()[0];
                var splitMinMax = splitPolicy[0].Split('-');
                var letterIndex1 = int.Parse(splitMinMax[0]);
                var letterIndex2 = int.Parse(splitMinMax[1]);

                return new PasswordEntry2()
                {
                    letter = character,
                    letterIndex1 = letterIndex1,
                    letterIndex2 = letterIndex2,
                    password = password
                };
            }
        }
    }
}
