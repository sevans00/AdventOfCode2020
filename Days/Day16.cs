using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    public class Day16
    {

        public static string inputFilePath = @"C:\Users\Shaun\Documents\Projects\AdventOfCode\AdventOfCode2020\AdventOfCode2020\Inputs\inputDay16.txt";

        public static string[] testInput = new string[]
        {
            "class: 1-3 or 5-7",
            "row: 6-11 or 33-44",
            "seat: 13-40 or 45-50",
            "",
            "your ticket:",
            "7,1,14",
            "",
            "nearby tickets:",
            "7,3,47",
            "40,4,50",
            "55,2,20",
            "38,6,12",
        };
        public static string[] testInput2 = new string[]
        {
            "class: 0-1 or 4-19",
            "row: 0-5 or 8-19",
            "seat: 0-13 or 16-19",
            "",
            "your ticket:",
            "11,12,13",
            "",
            "nearby tickets:",
            "3,9,18",
            "15,1,5",
            "5,14,9",
        };

        public enum ParsingStep
        {
            Validation,
            MyTicket,
            NearbyTickets
        }

        public static void Part1()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput2;

            var validation = new Validation();
            Ticket myTicket = null;
            List<Ticket> nearbyTickets = new List<Ticket>();

            var parsingStep = ParsingStep.Validation;
            for (int ii = 0; ii < inputStrings.Length; ii++)
            {
                var inputString = inputStrings[ii];

                if (inputString == "")
                    continue;
                if (inputString == "your ticket:")
                {
                    parsingStep = ParsingStep.MyTicket;
                    continue;
                }
                if (inputString == "nearby tickets:")
                {
                    parsingStep = ParsingStep.NearbyTickets;
                    continue;
                }

                if (parsingStep == ParsingStep.Validation)
                    validation.Add(inputStrings[ii]);
                if (parsingStep == ParsingStep.MyTicket)
                    myTicket = new Ticket(inputString);
                if (parsingStep == ParsingStep.NearbyTickets)
                    nearbyTickets.Add(new Ticket(inputString));
            }

            Console.WriteLine($"Validation:\n{string.Join("\n", validation.validationEntries)}");
            Console.WriteLine($"My Ticket:\n{myTicket}");
            Console.WriteLine($"Nearby Tickets:\n{string.Join("\n", nearbyTickets)}");

            Console.WriteLine("Parsing complete!");

            var allValidTickets = DoPart1(validation, myTicket, nearbyTickets);
            Console.WriteLine($"Valid Tickets:\n{string.Join("\n", allValidTickets)}");
            Console.WriteLine($"--- Part 2 ---");
            DoPart2_3(validation, myTicket, nearbyTickets, allValidTickets);
        }

        private static void DoPart2_3(Validation validation, Ticket myTicket, List<Ticket> nearbyTickets, List<Ticket> allValidTickets)
        {

            var ticketColumns = GetTicketColumns(allValidTickets);
            //Prepopulate 
            var columnsToValidationEntries = new Dictionary<int, List<ValidationEntry>>();
            for (int ii = 0; ii < ticketColumns.Count; ii++)
            {
                columnsToValidationEntries[ii] = new List<ValidationEntry>();
                columnsToValidationEntries[ii].AddRange(validation.validationEntries);
            }

            //Find which condition fails to satisfy each column:
            for (int ii = 0; ii < ticketColumns.Count; ii++)
            {
                List<int> column = ticketColumns[ii];
                var validValidationEntries = new List<ValidationEntry>();
                for (int jj = 0; jj < columnsToValidationEntries[ii].Count; jj++)
                {
                    var entry = columnsToValidationEntries[ii][jj];
                    if (entry.AreValid(column))
                    {
                        validValidationEntries.Add(entry);
                    }
                }
                columnsToValidationEntries[ii] = validValidationEntries;
            }

            var orderedColumnsToValidationEntries = columnsToValidationEntries.OrderBy(kvp => kvp.Value.Count);
            var validationsInOrder = new ValidationEntry[ticketColumns.Count];
            while (orderedColumnsToValidationEntries.Count() > 0)
            {
                var lowestValidationEntry = orderedColumnsToValidationEntries.First(); //There should be more checking that there's only one item, but i'm tired.
                validationsInOrder[lowestValidationEntry.Key] = lowestValidationEntry.Value.First(entry => !validationsInOrder.Contains(entry));

                columnsToValidationEntries.Remove(lowestValidationEntry.Key);
                foreach (var item in columnsToValidationEntries)
                {
                    item.Value.Remove(validationsInOrder[lowestValidationEntry.Key]);
                }
                orderedColumnsToValidationEntries = columnsToValidationEntries.OrderBy(kvp => kvp.Value.Count);
            }


            long departureProduct = 1;
            for (int ii = 0; ii < validationsInOrder.Length; ii++)
            {
                var validationEntry = validationsInOrder[ii];
                if (validationEntry == null)
                {
                    Console.WriteLine("NULL!");
                    continue;
                }
                Console.WriteLine($"{validationEntry.Name}: {myTicket.entries[ii]}");
                if(validationEntry.Name.StartsWith("departure"))
                {
                    departureProduct *= (long)myTicket.entries[ii];
                }
            }
            Console.WriteLine($"Product: {departureProduct}");
        }
        private static void DoPart2_2(Validation validation, Ticket myTicket, List<Ticket> nearbyTickets, List<Ticket> allValidTickets)
        {

            var ticketColumns = GetTicketColumns(allValidTickets);

            //Find which condition fails to satisfy each column:
            var columnsToValidationEntries = new Dictionary<int, List<ValidationEntry>>();
            for (int ii = 0; ii < ticketColumns.Count; ii++)
            {
                List<int> column = ticketColumns[ii];
                foreach (var entry in validation.validationEntries)
                {
                    if (entry.AreValid(column))
                    {
                        if(!columnsToValidationEntries.ContainsKey(ii))
                            columnsToValidationEntries[ii] = new List<ValidationEntry>();
                        columnsToValidationEntries[ii].Add(entry);
                    }
                }
            }

            var validationsInOrder = new ValidationEntry[ticketColumns.Count];
            for (int ii = 0; ii < ticketColumns.Count; ii++)
            {
                var validationEntries = columnsToValidationEntries[ii];
                if(validationEntries.Count == 1)
                {
                    //Found something:
                    validationsInOrder[ii] = validationEntries[0];
                    //Remove every one of the same validator:
                    foreach (var keyPairs in columnsToValidationEntries)
                    {
                        keyPairs.Value.Remove(validationsInOrder[ii]);
                    }
                }

            }


            for (int ii = 0; ii < validationsInOrder.Length; ii++)
            {
                var validationEntry = validationsInOrder[ii];
                if (validationEntry == null)
                {
                    Console.WriteLine("NULL!");
                    continue;
                }
                Console.WriteLine($"{validationEntry.Name}");
            }


        }

        private static void DoPart2(Validation validation, Ticket myTicket, List<Ticket> nearbyTickets, List<Ticket> allValidTickets)
        {
            var ticketColumns = GetTicketColumns(allValidTickets);

            //Find which condition satisfies each column:
            var validationsInOrder = new ValidationEntry[ticketColumns.Count];
            for (int ii = 0; ii < ticketColumns.Count; ii++)
            {
                if(validationsInOrder[ii] != null)
                {
                    continue;
                }
                List<int> column = ticketColumns[ii];
                var validationEntry = validation.validationEntries[0];
                var numValidEntries = 0;
                foreach (var entry in validation.validationEntries)
                {
                    if (entry.AreValid(column) )
                    {
                        validationEntry = entry;
                        numValidEntries++;
                    }
                }
                if (numValidEntries == 1)
                {
                    validationsInOrder[ii] = validationEntry;
                    ii = 0;
                }
            }

            Console.WriteLine(string.Join(",", validationsInOrder.Select(entry => entry.Name)));
        }

        private static List<List<int>> GetTicketColumns(List<Ticket> tickets)
        {
            var listOfColumns = new List<List<int>>();
            for (int ii = 0; ii < tickets[0].entries.Count; ii++)
            {
                var currentColumn = new List<int>();
                foreach (var ticket in tickets)
                {
                    currentColumn.Add(ticket.entries[ii]);
                }
                listOfColumns.Add(currentColumn);
            }
            return listOfColumns;
        }

        private static List<Ticket> DoPart1(Validation validation, Ticket myTicket, List<Ticket> nearbyTickets)
        {
            var invalidEntries = new List<int>();
            foreach (var ticket in nearbyTickets)
            {
                invalidEntries.AddRange(validation.GetInvalidEntriesFrom(ticket));
            }

            Console.WriteLine($"Invalid Entries:\n{string.Join(", ", invalidEntries)}");
            Console.WriteLine($"Invalid Entries Sum: {invalidEntries.Sum(x => x)}");

            //We've got invalid entries, now remove invalid tickets:
            return nearbyTickets
                .Where(ticket => !ticket.entries.Any(entry => invalidEntries.Contains(entry)))
                .ToList();
        }

        private class Validation
        {
            public List<ValidationEntry> validationEntries = new List<ValidationEntry>();
            public void Add(string validationString)
            {
                validationEntries.Add(new ValidationEntry(validationString));
            }

            public List<int> GetInvalidEntriesFrom(Ticket ticket)
            {
                var invalidEntries = new List<List<int>>();
                foreach (var validationEntry in validationEntries)
                {
                    invalidEntries.Add(validationEntry.GetInvalidEntriesFrom(ticket));
                }
                var invalidInEveryEntry = new List<int>();
                invalidInEveryEntry.AddRange(invalidEntries[0]);
                foreach (var invalidEntry in invalidEntries)
                {
                    invalidInEveryEntry = invalidInEveryEntry.Intersect(invalidEntry).ToList();
                }

                return invalidInEveryEntry;
            }
        }

        private class ValidationEntry
        {
            public string Name;
            public List<Range> ranges;

            public ValidationEntry(string validationString)
            {
                var nameAndRanges = validationString.Split(':');
                Name = nameAndRanges[0];
                var rangeStrs = nameAndRanges[1].Trim().Split(new string[] { " or " }, StringSplitOptions.None);
                ranges = rangeStrs.Select(range => new Range(range)).ToList();
            }

            public class Range
            {
                public int min;
                public int max;

                public Range(string range)
                {
                    var splitRange = range.Split('-');
                    min = int.Parse(splitRange[0]);
                    max = int.Parse(splitRange[1]);
                }

                public bool isValid(int value)
                {
                    return min <= value && value <= max;
                }

                public override string ToString()
                {
                    return $"Range: {min}-{max}";
                }
            }

            public override string ToString()
            {
                return $"{Name}: {string.Join(", ", ranges)}";
            }

            public List<int> GetInvalidEntriesFrom(Ticket ticket)
            {
                var invalidEntries = new List<int>();
                foreach (var entry in ticket.entries)
                {
                    var validInAnyRange = false;
                    foreach (var range in ranges)
                    {
                        if(range.isValid(entry))
                        {
                            validInAnyRange = true;
                        }
                    }
                    if (!validInAnyRange)
                        invalidEntries.Add(entry);
                }
                return invalidEntries;
            }

            public bool AreValid(List<int> list)
            {
                foreach (var item in list)
                {
                    var validForAnyRange = ranges.Any(range => range.isValid(item));
                    if(!validForAnyRange)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private class Ticket
        {
            public List<int> entries = new List<int>();
            public Ticket(string ticketString)
            {
                entries = ticketString.Split(',').Select(x => int.Parse(x)).ToList();
            }
            public override string ToString()
            {
                return $"Ticket: {string.Join(", ", entries)}";
            }
        }
    }
}