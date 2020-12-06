using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    public class Day6
    {
        public static string inputFilePath = @"C:\Users\Shaun\Documents\Projects\AdventOfCode\AdventOfCode2020\AdventOfCode2020\Inputs\inputDay6.txt";
        public static string[] testInput = new string[]
        {
            "abc",
            "",
            "a",
            "b",
            "c",
            "",
            "ab",
            "ac",
            "",
            "a",
            "a",
            "a",
            "a",
            "",
            "b",
        };


        public static void Part1()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;

            var groupAnswers = new List<GroupAnswer>() { new GroupAnswer() };
            foreach (var line in inputStrings)
            {
                if(line == "")
                {
                    groupAnswers.Add(new GroupAnswer());
                    continue;
                }
                groupAnswers.Last().AddAnswer(line);
            }

            var sum = 0;
            foreach (var answer in groupAnswers)
            {
                answer.RemoveDuplicates();
                Console.WriteLine($"Answer: {answer.answers}");
                sum += answer.answers.Length;
            }

            Console.WriteLine($"Sum: {sum}");
        }

        public static void Part2()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;

            var groupAnswers = new List<GroupAnswer>() { new GroupAnswer() };
            foreach (var line in inputStrings)
            {
                if (line == "")
                {
                    groupAnswers.Add(new GroupAnswer());
                    continue;
                }
                groupAnswers.Last().AddAnswer(line);
            }

            var sum = 0;
            foreach (var answer in groupAnswers)
            {
                answer.CountSimilarAnswers();
                Console.WriteLine($"Answer: {answer.answers}");
                sum += answer.answers.Length;
            }
            Console.WriteLine($"Sum: {sum}");


        }

        public class GroupAnswer
        {
            public string answers;
            public int count = 0;

            public void AddAnswer(string newAnswers)
            {
                count++;
                answers += newAnswers;
            }

            public void RemoveDuplicates()
            {
                answers = string.Join("", answers.Distinct());
            }

            public void CountSimilarAnswers()
            {
                var answerCount = new Dictionary<char, int>();
                foreach (var character in answers)
                {
                    if (!answerCount.ContainsKey(character))
                    {
                        answerCount[character] = 0;
                    }
                    answerCount[character]++;
                }
                var answersEqualToCount = answerCount.Where(keyValue => keyValue.Value == count).Select(keyValue => keyValue.Key);
                answers = string.Join("", answersEqualToCount);
                //answers = string.Join("", answers.Distinct());
            }

            public override string ToString()
            {
                return answers;
            }

        }

    }
}
