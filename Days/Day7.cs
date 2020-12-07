using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    public class Day7
    {
        public static string inputFilePath = @"C:\Users\Shaun\Documents\Projects\AdventOfCode\AdventOfCode2020\AdventOfCode2020\Inputs\inputDay7.txt";
        public static string[] testInput = new string[]
        {
            "light red bags contain 1 bright white bag, 2 muted yellow bags.",
            "dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
            "bright white bags contain 1 shiny gold bag.",
            "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
            "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
            "dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
            "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
            "faded blue bags contain no other bags.",
            "dotted black bags contain no other bags.",
        };
        public static string[] testInput2 = new string[]
        {
            "shiny gold bags contain 2 dark red bags.",
            "dark red bags contain 2 dark orange bags.",
            "dark orange bags contain 2 dark yellow bags.",
            "dark yellow bags contain 2 dark green bags.",
            "dark green bags contain 2 dark blue bags.",
            "dark blue bags contain 2 dark violet bags.",
            "dark violet bags contain no other bags.",
        };




        public static void Part1()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;

            var rules = new List<BagRule>();
            foreach (var ruleString in inputStrings)
            {
                var bagRule = new BagRule(ruleString);
                Console.WriteLine(bagRule);
                rules.Add(bagRule);
            }

            //Okay, now we've got the rules:
            var goalBag = "shiny gold";

            var goalBags = new List<string>() { goalBag };
            var bagsThatCouldContainGoalBag = new HashSet<string>();
            while (goalBags.Count > 0)
            {
                var currentGoalBag = goalBags[0];
                goalBags.RemoveAt(0);
                var bagsContainingGoalBag = GetBagsContainingGoalBag(rules, currentGoalBag);
                foreach (var bagContainingGoalBag in bagsContainingGoalBag)
                {
                    bagsThatCouldContainGoalBag.Add(bagContainingGoalBag);
                }
                goalBags.AddRange(bagsContainingGoalBag);
            }

            Console.WriteLine("");
            Console.WriteLine($"Could Contain Bags: {string.Join(", ", bagsThatCouldContainGoalBag)}");
            Console.WriteLine($"Count: {bagsThatCouldContainGoalBag.Count()}");

        }

        public static IEnumerable<string> GetBagsContainingGoalBag(List<BagRule> rules, string goalBag)
        {
            return rules
                .Where(rule => rule.Contains.Contains(goalBag))
                .Select(rule => rule.BagName);
        }



        public class BagRule
        {
            public string BagName;

            public string[] Contains = new string[0];
            public int[] ContainsCount = new int[0];

            public BagRule(string ruleString)
            {
                var ruleSplit = ruleString.Split(new string[] { "contain" }, StringSplitOptions.RemoveEmptyEntries);
                BagName = ruleSplit[0].Replace("bags", "").Replace("bag", "").Trim();
                var containsStrings = ruleSplit[1].Split(',');

                Contains = containsStrings.Select(bag => bag.Trim(' ', '.')).ToArray();
                if (Contains.Length == 1)
                {
                    if (Contains[0] == "no other bags")
                    {
                        Contains = new string[0];
                    }
                }
                //This is uuuuuuuuugly:
                var contains = new List<string>();
                var containsCounts = new List<int>();
                foreach (var contain in Contains)
                {
                    var containSplit = contain.Split(new char[] { ' ' }, 2);
                    var containString = containSplit[1];
                    containString = containString.Replace("bags", "").Replace("bag", "").Trim();
                    var containInt = int.Parse(containSplit[0].Trim());

                    contains.Add(containString);
                    containsCounts.Add(containInt);
                }
                Contains = contains.ToArray();
                ContainsCount = containsCounts.ToArray();
            }

            public override string ToString()
            {
                var bagContainsString = "";
                for (int ii = 0; ii < Contains.Length; ii++)
                {
                    bagContainsString += $"{ContainsCount[ii]}=>{Contains[ii]} | ";
                }
                return $"{BagName} contain {bagContainsString}";
            }
        }




        public static void Part2()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;
            //var inputStrings = testInput2;

            var rules = new List<BagRule>();
            foreach (var ruleString in inputStrings)
            {
                var bagRule = new BagRule(ruleString);
                Console.WriteLine(bagRule);
                rules.Add(bagRule);
            }

            //Okay, now we've got the rules:
            var currentBagString = "shiny gold";
            var currentRule = rules.First(rule => rule.BagName == currentBagString);
            var bagCount = GetBagsInsideBag(rules, currentRule);
            Console.WriteLine("");
            Console.WriteLine($"Final Count: {bagCount}");
        }

        public static int GetBagsInsideBag(List<BagRule> rules, BagRule currentRule)
        {
            var sum = 0;
            for (int ii = 0; ii < currentRule.Contains.Length; ii++)
            {
                var containsBag = currentRule.Contains[ii];
                var countBag = currentRule.ContainsCount[ii];

                sum += countBag;
                var bagRule = rules.First(rule => rule.BagName == containsBag);
                sum += countBag * GetBagsInsideBag(rules, bagRule);
            }
            return sum;
        }


    }
}
