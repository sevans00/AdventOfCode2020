using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    public class Day13
    {

        public static string inputFilePath = @"C:\Users\Shaun\Documents\Projects\AdventOfCode\AdventOfCode2020\AdventOfCode2020\Inputs\inputDay13.txt";

        public static string[] testInput = new string[]
        {
            "939",
            "7,13,x,x,59,x,31,19"
        };


        public static void Part1()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;

            var earliestDepartureTimestamp = int.Parse(inputStrings[0]);
            var busNotes = inputStrings[1];

            busNotes = busNotes.Replace("x", ""); //Remove busses out of service
            var busIds = busNotes
                .Split(',')
                .Where(id => !string.IsNullOrEmpty(id))
                .Select(id => int.Parse(id));

            var earliestDeparture = GetEarliestDeparture(earliestDepartureTimestamp, busIds);
            var busId = earliestDeparture.Item1;
            var timestamp = earliestDeparture.Item2;
            var timeToWait = timestamp - earliestDepartureTimestamp;
            Console.WriteLine($"Found Bus! {busId} at: {timestamp}.");
            Console.WriteLine($"Wait {timeToWait}.  Product: {busId*timeToWait}.");
        }

        private static Tuple<int, int> GetEarliestDeparture(int earliestDepartureTimestamp, IEnumerable<int> busIds)
        {
            var maximumWait = busIds.Max();
            for (int timestamp = earliestDepartureTimestamp; timestamp < earliestDepartureTimestamp + maximumWait; timestamp++)
            {
                foreach (var busId in busIds)
                {
                    if (timestamp % busId == 0)
                    {
                        Console.WriteLine($"Found Bus! {busId} at: {timestamp}");
                        return new Tuple<int, int>(busId, timestamp);
                    }
                }
            }
            return new Tuple<int, int>(-1, -1);
        }



        public static void Part2()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;

            var busNotesInput = inputStrings[1];

            var busNotes2 = "17,x,13,19"; //3417
            //17*(201) + 13(263)-2 + 19(180) -3 = 3417 * 3

            //17*(x) = 13(y)-2 
            //17*(x) = 19(z) -3 
            //13(y) -2 = 19(z) -3 
            /*
            x = 1/102
            y = 1/6
            z = 3/19 + (17/102)/19

            
            "7,13,x,x,59,x,31,19"

            t % 17 = 0
            t % 13 = 11
            t % 19 = 16

            t = 11 %-1 13
            t % 17 = 0
             */



            // 17x + 13(x+2) + 19(x+3)
            // 17x + 13(x+2)-2 + 19(x+3)-3 = 17x * 3
            // 17x + 13(x+2) + 19(x+3) - 5 = 17x * 3
            // 17x + 13x + 26 + 19x + 57 - 5 = 17x * 3
            // 17x + 13x + 19x + 57 + 26 - 5 = 17x * 3
            // 49x + 78 = 51x
            // 49x + 78 = 51x

            var busNotes3 = "67,7,59,61"; //754018
            var busNotes4 = "67,x,7,59,61"; //779210
            var busNotes5 = "67,7,x,59,61"; //1261476
            var busNotes6 = "1789,37,47,1889"; //1202161486

            var busNotes = busNotesInput; //Change this over for testing others
            busNotes = "17,x,13,19".Replace("x", "1"); //Remove busses out of service
            busNotes = "102,x,x,19".Replace("x", "1"); //Remove busses out of service
            var busIds = busNotes
                .Split(',')
                .Select(id => ulong.Parse(id))
                .ToList();
            Console.WriteLine($"Bus Ids: {string.Join(",", busIds)}");

            var firstBusId = (uint)busIds.First();
            if (busIds.First() == 1)
                Console.WriteLine("Uh oh...");

            ulong productOfBusIds = 1;
            foreach (var busId in busIds)
            {
                productOfBusIds *= busId;
            }
            Console.WriteLine($"Product: {productOfBusIds}");
            ulong maxBusId = busIds.Max();
            ulong maxBusIdIndex = (ulong)busIds.IndexOf(maxBusId);

            ulong timestamp = productOfBusIds;
            ulong maxTimestamp = 1;
            //At least 11436189414 24412218628 44,542,080,817 74109568324
            // And lower than 1157664013323402
            // Higher than         74109568324
            //It actually was: 702970661767766
            //So that's only... a lot to find.
            while (timestamp > 1)
            {
                timestamp -= firstBusId; //'s gotta be at least that
                maxTimestamp = timestamp + (uint)busIds.Count();
                if (IsValidTimestamp(timestamp, busIds, maxTimestamp))
                {
                    break;
                }
            }


            /*
            while (true)
            {
                timestamp += firstBusId; //'s gotta be at least that
                maxTimestamp = timestamp + (uint)busIds.Count();
                if(IsValidTimestamp(timestamp, busIds, maxTimestamp))
                {
                    break;
                }
            }*/

            Console.WriteLine($"Timestamp: {timestamp}");
        }



        public static void Part3()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;
            

            var busNotesInput = inputStrings[1];
            var busNotes1 = "7,13,x,x,59,x,31,19";
            var busNotes2 = "17,x,13,19"; //3417
            var busNotes3 = "67,7,59,61"; //754018
            var busNotes4 = "67,x,7,59,61"; //779210
            var busNotes5 = "67,7,x,59,61"; //1261476
            var busNotes6 = "1789,37,47,1889"; //1202161486

            var busNotes = busNotesInput.Replace("x", "1"); //Remove busses out of service
            var busIds = busNotes
                .Split(',')
                .Select(id => ulong.Parse(id))
                .ToList();
            Console.WriteLine($"Bus Ids: {string.Join(",", busIds)}");
            var busMinutesLaterThan = new List<int>();
            for (int ii = 0; ii < busIds.Count; ii++)
            {
                busMinutesLaterThan.Add(ii);
            }

            var compoundBusId = busIds[0];
            ulong timestep = compoundBusId; //Just for the first one!
            ulong busProduct = busIds[0];
            for (int ii = 1; ii < busIds.Count; ii++)
            {
                var busId = busIds[ii];
                var busMinutes = busMinutesLaterThan[ii];
                if (busId == 1)
                    continue;
                var lastCompoundId = compoundBusId;
                compoundBusId = GetSequentialDeparture(timestep, compoundBusId, busId, busMinutes);
                timestep = (timestep * busId); //102 + 221 = 323
                Console.WriteLine($"\tCompoundId of {lastCompoundId} & {busId} is {compoundBusId}");
            }


            Console.WriteLine($"Final Timestamp: {compoundBusId}");
            Console.WriteLine($"Checking: {compoundBusId}");
            for (int ii = 0; ii < busIds.Count; ii++)
            {
                var busId = busIds[ii];
                var busMinutes = busMinutesLaterThan[ii];
                if (busId == 1)
                    continue;
                Console.WriteLine($"\t{compoundBusId} % {busId} = {compoundBusId%busId}  \t Correct={(compoundBusId % busId)+(ulong)busMinutes}, {(compoundBusId+(ulong)busMinutes)% busId == 0}");
            }



            /*

            //Now, get the first one:
            var firstBusId = busNotes.First(busNote => busNote != "x");
            var firstBusIndex = busNotes.IndexOf(firstBusId);
            var firstBusMinutesAfter = busMinutesLaterThan[firstBusIndex];
            busNotes.RemoveAt(firstBusIndex);
            busMinutesLaterThan.RemoveAt(firstBusIndex);
            //Now, get the second one:
            var secondBusId = busNotes.First(busNote => busNote != "x");
            var secondBusIndex = busNotes.IndexOf(secondBusId);
            var secondBusMinutesAfter = busMinutesLaterThan[secondBusIndex];
            busNotes.RemoveAt(secondBusIndex);
            busMinutesLaterThan.RemoveAt(secondBusIndex);

            var timestep = ulong.Parse(firstBusId);
            var firstSequentialTimestamp = GetSequentialDeparture(0, ulong.Parse(firstBusId), ulong.Parse(secondBusId), secondBusMinutesAfter);
            var sequentialTimestamp = firstSequentialTimestamp;
            timestep = ulong.Parse(firstBusId) * ulong.Parse(secondBusId);
            while (busNotes.Any(busNote => busNote != "x"))
            {
                var nextBusId = busNotes.First(busNote => busNote != "x");
                var nextBusIndex = busNotes.IndexOf(nextBusId);
                var nextBusMinutesAfter = busMinutesLaterThan[nextBusIndex];
                busNotes.RemoveAt(nextBusIndex);
                busMinutesLaterThan.RemoveAt(nextBusIndex);

                var nextBusIds = new List<ulong>() {
                    sequentialTimestamp,
                    ulong.Parse(secondBusId)
                };
                

                sequentialTimestamp = GetSequentialDeparture(timestep, sequentialTimestamp, ulong.Parse(nextBusId), nextBusMinutesAfter);
                timestep = sequentialTimestamp * ulong.Parse(nextBusId);
                Console.WriteLine($"Sequential Timestamp: {sequentialTimestamp}.  Remaining ids: [{String.Join(",",busNotes)}]");
            }
            
            Console.WriteLine($"Final Timestamp: {sequentialTimestamp}");
            */
        }

        private static ulong GetSequentialDeparture(ulong timestep, ulong currentTime, ulong busId2, int nextBusAfterMinutes)
        {
            ulong timestamp = currentTime;
            while (true)
            {
                timestamp += timestep;
                if (IsValidTimestamp2(timestamp, currentTime, busId2, nextBusAfterMinutes))
                {
                    return timestamp;
                }
            }
        }

        private static bool IsValidTimestamp2(ulong timestamp, ulong busId1, ulong busId2, int nextBusAfterMinutes)
        {
            /*
            if(timestamp % busId1 != 0) //This should always be false
            {
                return false;
            }*/
            if ((timestamp + (ulong)nextBusAfterMinutes) % busId2 != 0)
            {
                return false;
            }
            return true;
        }


        private static ulong GetSequentialDepartures(List<ulong> busIds)
        {
            ulong timestamp = 0;
            ulong maxTimestamp = 1;
            ulong maxBusId = busIds.Max();
            while (true)
            {
                timestamp += maxBusId; //'s gotta be at least that
                maxTimestamp = timestamp + (uint)busIds.Count();
                if (IsValidTimestamp(timestamp, busIds, maxTimestamp))
                {
                    return timestamp;
                }
            }
        }

        public static bool IsValidTimestamp(ulong timestamp, List<ulong> busIds, ulong maxTimestamp)
        {
            //Test every minute:
            var currentMinute = 0;
            for (var ii = timestamp; ii < maxTimestamp; ii++)
            {
                var busId = busIds[currentMinute];
                currentMinute++;
                if (busId == 1)
                    continue;
                if (ii % (uint)busId != 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
