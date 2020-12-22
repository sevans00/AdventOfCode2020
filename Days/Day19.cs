using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    public class Day19
    {

        public static string inputFilePath = @"C:\Users\Shaun\Documents\Projects\AdventOfCode\AdventOfCode2020\AdventOfCode2020\Inputs\inputDay19.txt";

        public static string[] testInput = new string[]
        {
            "0: 4 1 5",
            "1: 2 3 | 3 2",
            "2: 4 4 | 5 5",
            "3: 4 5 | 5 4",
            "4: \"a\"",
            "5: \"b\"",
            "",
            "ababbb",
            "bababa",
            "abbbab",
            "aaabbb",
            "aaaabbb",
        };

        public static string[] testInput2 = new string[]
        {
            "0: 1 2",
            "1: 2 3 | 3 2",
            "2: \"a\"",
            "3: \"b\"",
            "",
            "aba", //Match
            "abaa", //No
            "baa", //Match
        };

        public static string[] testInput5 = new string[]
        {
            "0: 1 2 4",
            "1: 2 3 | 3 2",
            "2: \"a\"",
            "3: \"b\"",
            "4: 2 2",
            "",
            "abaaa", //Match
            "abaaaa", //No
            "baaaa", //Match
        };

        public static string[] testInput6 = new string[]
        {
            "0: 1 2 4",
            "1: 2 3 | 3 2",
            "2: \"a\"",
            "3: \"b\"",
            "4: 1 1",
            "",
            "abaaa", //Match
            "abaaaa", //No
            "baaaa", //Match
        };
        public static string[] testInput3 = new string[]
        {
            "42: 9 14 | 10 1",
            "9: 14 27 | 1 26",
            "10: 23 14 | 28 1",
            "1: \"a\"",
            "11: 42 31",
            "5: 1 14 | 15 1",
            "19: 14 1 | 14 14",
            "12: 24 14 | 19 1",
            "16: 15 1 | 14 14",
            "31: 14 17 | 1 13",
            "6: 14 14 | 1 14",
            "2: 1 24 | 14 4",
            "0: 8 11",
            "13: 14 3 | 1 12",
            "15: 1 | 14",
            "17: 14 2 | 1 7",
            "23: 25 1 | 22 14",
            "28: 16 1",
            "4: 1 1",
            "20: 14 14 | 1 15",
            "3: 5 14 | 16 1",
            "27: 1 6 | 14 18",
            "14: \"b\"",
            "21: 14 1 | 1 14",
            "25: 1 1 | 1 14",
            "22: 14 14",
            "8: 42",
            "26: 14 22 | 1 20",
            "18: 15 15",
            "7: 14 5 | 1 21",
            "24: 14 1",
            "",
            "abbbbbabbbaaaababbaabbbbabababbbabbbbbbabaaaa",
            "bbabbbbaabaabba",
            "babbbbaabbbbbabbbbbbaabaaabaaa",
            "aaabbbbbbaaaabaababaabababbabaaabbababababaaa",
            "bbbbbbbaaaabbbbaaabbabaaa",
            "bbbababbbbaaaaaaaabbababaaababaabab",
            "ababaaaaaabaaab",
            "ababaaaaabbbaba",
            "baabbaaaabbaaaababbaababb",
            "abbbbabbbbaaaababbbbbbaaaababb",
            "aaaaabbaabaaaaababaa",
            "aaaabbaaaabbaaa",
            "aaaabbaabbaaaaaaabbbabbbaaabbaabaaa",
            "babaaabbbaaabaababbaabababaaab",
            "aabbbbbaabbbaaaaaabbbbbababaaaaabbaaabba",
        };

        public static void Part1()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;
            //var inputStrings = testInput3;
            //var inputStrings = testInput3;


            var ruleStrings = new List<string>();
            var messages = new List<string>();

            bool parsingRules = true;
            foreach (var input in inputStrings)
            {
                if (input == "")
                {
                    parsingRules = false;
                    continue;
                }
                if (parsingRules)
                {
                    ruleStrings.Add(input);
                }
                else
                {
                    messages.Add(input);
                }
            }

            //Attempt1(ruleStrings, messages);
            //Attempt2Part2(ruleStrings, messages);
            //Attempt1Part2(ruleStrings, messages);
            Console.BufferHeight = Int16.MaxValue - 1;
            Attempt1Part2(ruleStrings, messages);
        }

        private static void Attempt2Part2(List<string> ruleStrings, List<string> messages)
        {
            var allRules = ruleStrings.Select(ruleString => new Rule(ruleString)).ToDictionary(rule => int.Parse(rule.RuleId));
            //allRules = allRules.OrderBy(rule => int.Parse(rule.RuleId)).ToArray();

            
            //Console.WriteLine(string.Join("\n", allRulePermutations));
            allRules[8] = new Rule("8: 42 | 42 8");
            allRules[11] = new Rule("11: 42 31 | 42 11 31");

            var maxLength = messages.Max(s => s.Length);

            var allRulePermutations = allRules[0].GenerateStrings(new List<string>() { "" }, allRules, maxLength);


            var completelyMatchCount = 0;
            foreach (var message in messages)
            {
                if (allRulePermutations.Contains(message))
                {
                    completelyMatchCount++;
                }
            }
            Console.WriteLine($"Count: {completelyMatchCount}");
        }
        private static void Attempt2(List<string> ruleStrings, List<string> messages)
        {
            var allRules = ruleStrings.Select(ruleString => new Rule(ruleString)).ToDictionary(rule => int.Parse(rule.RuleId));

            var allRulePermutations = allRules[0].GenerateStrings(new List<string>() { "" }, allRules);

            //Console.WriteLine(string.Join("\n", allRulePermutations));


            var completelyMatchCount = 0;
            foreach (var message in messages)
            {
                if(allRulePermutations.Contains(message))
                {
                    completelyMatchCount++;
                }
            }
            Console.WriteLine($"Count: {completelyMatchCount}");
        }

        private static void Attempt1(List<string> ruleStrings, List<string> messages)
        {
            var allRules = ruleStrings.Select(ruleString => new Rule(ruleString)).ToDictionary(rule => int.Parse(rule.RuleId));

            var completelyMatchCount = 0;
            foreach (var message in messages)
            {
                if (allRules[0].DoesMatch(message, allRules, out var unMatched))
                {
                    completelyMatchCount++;
                    WriteLine($"Message {message} Matched\n");
                    continue;
                }
                WriteLine($"Message {message} Did not match.  Unmatched: '{unMatched.Count}'\n");

            }

            WriteLine($"Completely match: {completelyMatchCount}");
        }
        
        private static void Attempt1Part2(List<string> ruleStrings, List<string> messages)
        {
            var allRules = ruleStrings.Select(ruleString => new Rule(ruleString)).ToDictionary(rule => int.Parse(rule.RuleId));

            allRules[8] = new Rule("8: 42 | 42 8");
            allRules[11] = new Rule("11: 42 31 | 42 11 31");

            var completelyMatchCount = 0;
            foreach (var message in messages)
            {
                Console.WriteLine("===");
                if (allRules[0].DoesMatch(message, allRules, out var unMatched))
                {
                    if (!unMatched.Any(s => string.IsNullOrEmpty(s)))
                    {
                        Console.WriteLine($"----\nNomatch! '{unMatched}' remained unmatched\n");
                        continue;
                    }

                    /*if(!string.IsNullOrEmpty(unMatched.Trim()))
                    {
                        Console.WriteLine($"----\nNomatch! '{unMatched}' remained unmatched\n");
                        continue;
                    }*/
                    completelyMatchCount++;
                    Console.WriteLine($"Message {message} Matched\n");
                    continue;
                }
                Console.WriteLine($"----\n");
                Console.WriteLine($"Message {message} Did not match.  Unmatched: '{unMatched}'\n");
            }

            Console.WriteLine($"Completely match: {completelyMatchCount}");
        }

        public static void WriteLine(string output)
        {
            //Console.WriteLine(output);
        }

        public class Rule
        {
            private readonly string ruleString;

            public string RuleId;

            public enum RuleType
            {
                Character,
                All,
                PipeRule,
            }

            public RuleType ruleType;

            //Character:
            public char matchingChar;

            //All:
            public List<int> allRuleIds;

            //Pipe:
            public List<int> allRuleIds1;
            public List<int> allRuleIds2;

            public Rule(string ruleString)
            {
                this.ruleString = ruleString;

                var splitRule = ruleString.Split(':');
                RuleId = splitRule[0];

                if(splitRule[1].Contains('|'))
                {
                    ruleType = RuleType.PipeRule;
                    var rulesSplit = splitRule[1].Split('|');
                    allRuleIds1 = rulesSplit[0]
                        .Split(' ')
                        .Where(s => !string.IsNullOrEmpty(s))
                        .Select(s => int.Parse(s))
                        .ToList();
                    allRuleIds2 = rulesSplit[1]
                        .Split(' ')
                        .Where(s => !string.IsNullOrEmpty(s))
                        .Select(s => int.Parse(s))
                        .ToList();
                    return;
                }

                if(splitRule[1].Contains('"'))
                {
                    ruleType = RuleType.Character;
                    var charSplit = splitRule[1].Trim().Split('"');
                    matchingChar = splitRule[1].Trim()[1];
                    return;
                }

                ruleType = RuleType.All;
                allRuleIds = splitRule[1]
                    .Split(' ')
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Select(s => int.Parse(s.Trim()))
                    .ToList();
            }

            public bool DoesMatch(string message, Dictionary<int, Rule> allRules, out List<string> unMatched, int depth = 0)
            {
                WriteLine($"Rule [{RuleId}:{ruleString}] trying to match '{message}'.  {this}");

                if(message.Length == 0)
                {
                    unMatched = new List<string>();
                    return false;
                }

                switch (ruleType)
                {
                    case RuleType.Character:
                        if (message.Length == 0)
                        {
                            unMatched = new List<string>() { message };
                            return false; //Rule kept going too long!
                        }
                        unMatched = new List<string>() { message.Substring(1) };
                        return message[0] == matchingChar;
                    case RuleType.All:
                        unMatched = GetUnmatchedFromSequence(message, allRules, ref depth, allRuleIds);
                        //if (string.IsNullOrEmpty(partialUnMatched))
                        //{

                        return true;
                    //}
                    //return false;
                    case RuleType.PipeRule:
                        WriteLine($"\t[{depth}] Pipe Rule LEFT trying: '{string.Join(",", allRuleIds1)}'");
                        var matchedLeft = false;
                        var matchedRight = false;
                        if (MatchPartialRules(message, allRules, out var partialUnmatched1, allRuleIds1, depth++))
                        {
                            matchedLeft = true;
                        }
                        WriteLine($"\t[{depth}] Pipe Rule RIGHT trying: '{string.Join(",", allRuleIds2)}'");
                        if (MatchPartialRules(message, allRules, out var partialUnmatched2, allRuleIds2, depth++))
                        {
                            matchedRight = true;
                        }
                        //if (string.IsNullOrEmpty(unMatched))
                        //{
                        if (matchedLeft && matchedRight)
                        {
                            partialUnmatched1.AddRange(partialUnmatched2);
                            unMatched = partialUnmatched1;
                            return true;
                        }
                        if (matchedLeft)
                        {
                            unMatched = partialUnmatched1;
                            return true;
                        }
                        if(matchedRight)
                        {
                            unMatched = partialUnmatched2;
                            return true;
                        }
                        partialUnmatched1.AddRange(partialUnmatched2);
                        unMatched = partialUnmatched1;
                        return false;
                        //}
                        //break;
                    default:
                        break;
                }
                unMatched = new List<string>() { message };
                return false;
            }

            private List<string> GetUnmatchedFromSequence(string message, Dictionary<int, Rule> allRules, ref int depth, List<int> ruleIds)
            {
                List<string> unMatched;
                var partialUnmatchedList = new List<string>() { message };
                var nextRulePartialUnmatchedList = new List<string>();
                foreach (var ruleId in ruleIds)
                {
                    var partialUnmatchedList_bkup = partialUnmatchedList.Select(s => s).ToList();
                    foreach (var partialUnmatched in partialUnmatchedList_bkup)
                    {
                        if (!allRules[ruleId].DoesMatch(partialUnmatched, allRules, out var newPartialUnmatchedList, depth++))
                        {
                            //unMatched = new List<string>() { partialUnmatched };
                            WriteLine($"\t[{depth}] Seq {ruleId} Did not match!");
                            continue; //Can't just return false
                        }
                        nextRulePartialUnmatchedList.AddRange(newPartialUnmatchedList);
                        WriteLine($"\t[{depth}] Seq {ruleId} Matched!");
                    }
                    partialUnmatchedList = nextRulePartialUnmatchedList;
                    nextRulePartialUnmatchedList = new List<string>();
                }
                unMatched = partialUnmatchedList;
                return unMatched;
            }

            private bool MatchPartialRules(string message, Dictionary<int, Rule> allRules, out List<string> unMatched, List<int> ruleIds, int depth)
            {
                unMatched = GetUnmatchedFromSequence(message, allRules, ref depth, ruleIds);
                return true;
            }

/*
            private bool MatchPartialRules(string message, Dictionary<int, Rule> allRules, out List<string> unMatched, List<int> ruleIds, int depth)
            {
                string partialUnMatched = message;
                foreach (var ruleId in ruleIds)
                {
                    WriteLine($"\t\tPartial Rule {RuleId}:");
                    if (!allRules[ruleId].DoesMatch(partialUnMatched, allRules, out partialUnMatched, depth))
                    {
                        unMatched = partialUnMatched;
                        return false;
                    }
                }
                unMatched = partialUnMatched;
                return true;
            }*/

            public override string ToString()
            {
                switch (ruleType)
                {
                    case RuleType.Character:
                        return $"{RuleId}: {ruleType}: '{matchingChar}'";
                    case RuleType.All:
                        return $"{RuleId}: {ruleType}: '{string.Join(", ", allRuleIds)}'";
                    case RuleType.PipeRule:
                        return $"{RuleId}: {ruleType}: '{string.Join(", ", allRuleIds1)}' | '{string.Join(", ", allRuleIds2)}'";
                    default:
                        break;
                }
                return "";
            }

            public List<string> GenerateStrings(List<string> currentPrefixes, Dictionary<int, Rule> allRules)
            {
                switch (ruleType)
                {
                    case RuleType.Character:
                        return currentPrefixes.Select(currentString => currentString + matchingChar).ToList();
                    case RuleType.All:
                        var newPrefixes = currentPrefixes.Select(s => s).ToList();
                        //This is a sequence, we want to append each new prefix
                        foreach (var ruleId in allRuleIds)
                        {
                            newPrefixes = allRules[ruleId].GenerateStrings(newPrefixes, allRules);
                        }
                        return newPrefixes;
                    case RuleType.PipeRule:

                        var newPrefixes1 = currentPrefixes.Select(s => s).ToList();
                        //This is a sequence, we want to append each new prefix
                        foreach (var ruleId in allRuleIds1)
                        {
                            newPrefixes1 = allRules[ruleId].GenerateStrings(newPrefixes1, allRules);
                        }

                        var newPrefixes2 = currentPrefixes.Select(s => s).ToList();
                        //This is a sequence, we want to append each new prefix
                        foreach (var ruleId in allRuleIds2)
                        {
                            newPrefixes2 = allRules[ruleId].GenerateStrings(newPrefixes2, allRules);
                        }


                        newPrefixes1.AddRange(newPrefixes2);
                        return newPrefixes1;
                    default:
                        break;
                }
                return currentPrefixes;
            }

            public Dictionary<int, List<string>> cachedRuleIdsToPrecalculated = new Dictionary<int, List<string>>();

            public List<string> GenerateStrings(List<string> currentPrefixes, Dictionary<int, Rule> allRules, int maxLength, int currentDepth = 0)
            {
                currentDepth++;
                if(currentDepth > 100000)
                {
                    return currentPrefixes;
                }

                foreach (var prefix in currentPrefixes)
                {
                    if (prefix.Length > maxLength)
                    {
                        currentPrefixes.Remove(prefix); //'s gonna be too big
                    }
                }

                switch (ruleType)
                {
                    case RuleType.Character:
                        return currentPrefixes.Select(currentString => currentString + matchingChar).ToList();
                    case RuleType.All:
                        var newPrefixes = currentPrefixes.Select(s => s).ToList();
                        //This is a sequence, we want to append each new prefix
                        foreach (var ruleId in allRuleIds)
                        {
                            if (cachedRuleIdsToPrecalculated.ContainsKey(ruleId))
                            {
                                newPrefixes = cachedRuleIdsToPrecalculated[ruleId].Select(s => s).ToList();
                            }
                            else
                            {
                                newPrefixes = allRules[ruleId].GenerateStrings(newPrefixes, allRules, maxLength, currentDepth);
                                cachedRuleIdsToPrecalculated[ruleId] = newPrefixes.Select(s => s).ToList();
                            }
                        }
                        return newPrefixes;
                    case RuleType.PipeRule:

                        var newPrefixes1 = currentPrefixes.Select(s => s).ToList();
                        //This is a sequence, we want to append each new prefix
                        foreach (var ruleId in allRuleIds1)
                        {
                            if (cachedRuleIdsToPrecalculated.ContainsKey(ruleId))
                            {
                                newPrefixes1 = cachedRuleIdsToPrecalculated[ruleId].Select(s => s).ToList();
                            }
                            else
                            {
                                newPrefixes1 = allRules[ruleId].GenerateStrings(newPrefixes1, allRules, maxLength, currentDepth);
                                cachedRuleIdsToPrecalculated[ruleId] = newPrefixes1.Select(s => s).ToList();
                            }
                        }

                        var newPrefixes2 = currentPrefixes.Select(s => s).ToList();
                        //This is a sequence, we want to append each new prefix
                        foreach (var ruleId in allRuleIds2)
                        {
                            if (cachedRuleIdsToPrecalculated.ContainsKey(ruleId))
                            {
                                newPrefixes2 = cachedRuleIdsToPrecalculated[ruleId].Select(s => s).ToList();
                            }
                            else
                            {
                                newPrefixes2 = allRules[ruleId].GenerateStrings(newPrefixes2, allRules, maxLength, currentDepth);
                                cachedRuleIdsToPrecalculated[ruleId] = newPrefixes2.Select(s => s).ToList();
                            }
                        }

                        newPrefixes1.AddRange(newPrefixes2);
                        return newPrefixes1;
                    default:
                        break;
                }
                return currentPrefixes;
            }
        }
    }
}