using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    public class Day14
    {

        public static string inputFilePath = @"C:\Users\Shaun\Documents\Projects\AdventOfCode\AdventOfCode2020\AdventOfCode2020\Inputs\inputDay14.txt";

        public static string[] testInput = new string[]
        {
            "mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X",
            "mem[8] = 11",
            "mem[7] = 101",
            "mem[8] = 0",
        };
        public static string[] testInput2 = new string[]
        {
            "mask = 000000000000000000000000000000X1001X",
            "mem[42] = 100",
            "mask = 00000000000000000000000000000000X0XX",
            "mem[26] = 1",
        };


        public static void Part2()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput2;

            var addressSpace = new Dictionary<ulong, ulong>();

            Mask currentMask = null;
            foreach (var line in inputStrings)
            {
                if (line.StartsWith("mask = "))
                {
                    currentMask = new Mask(line);
                    continue;
                }
                var command = new Command(line);
                ApplyCommandToAddresses3(currentMask, command, ref addressSpace);
            }

            Console.WriteLine($"Result:\n{string.Join("\n", addressSpace.Select(kv => $"{kv.Key} = {kv.Value}"))}");
            ulong sum = 0;
            foreach (var addressValue in addressSpace.Values)
            {
                sum += addressValue;
            }

            Console.WriteLine($"Sum of address space = {sum}");
        }


        private static void ApplyCommandToAddresses3(Mask mask, Command command, ref Dictionary<ulong, ulong> addressSpace)
        {
            var allMasks = GenerateAllMasks(mask.maskString);
            foreach (var newMask in allMasks)
            {
                var iiAddress = mask.xMaskZeroes & command.address;
                iiAddress = newMask.onesMask  | iiAddress;
                addressSpace[iiAddress] = command.value;
            }
        }

        private static List<Mask> GenerateAllMasks(string maskString)
        {
            var xIndex = maskString.IndexOf('X');
            if (xIndex == -1)
                return new List<Mask>() { new Mask(maskString) };
            var oneMask = maskString.Remove(xIndex, 1).Insert(xIndex, "1");
            var zeroMask = maskString.Remove(xIndex, 1).Insert(xIndex, "0");
            var masks = new List<Mask>() { 
                //new Mask(oneMask), new Mask(zeroMask) 
            };
            masks.AddRange(GenerateAllMasks(oneMask));
            masks.AddRange(GenerateAllMasks(zeroMask));
            return masks;
        }

        private static void ApplyCommandToAddresses2(Mask mask, Command command, ref Dictionary<ulong, ulong> addressSpace)
        {
            var maskedAddress = mask.onesMask | command.address;
            //maskedAddress = ~mask.zerosMask & maskedAddress;
            maskedAddress = ~mask.xMask & maskedAddress;

            //Each iteration can be thought of as a distinct number, that varies on the number of X's.
            //ulong numXs = (ulong)mask.maskString.Count(maskChar => maskChar == 'X');
            //var maxX = (ulong)Math.Pow(2, numXs);
            var xCount = mask.maskString.Count(ch => ch == 'X');
            ulong currentX = 0;
            for (int x = 0; x < xCount; x++)
            {
                for (var jj = mask.maskString.Length - 1; jj >= 0; jj--)
                {
                    var ii = mask.maskString.Length - 1 - jj;
                    if (mask.maskString[jj] == 'X')
                    {

                    }
                    var iiAddress = (currentX & mask.xMask) | maskedAddress;
                    //ulong iiAddress = GetAddressForII(mask, ii, maskedAddress);
                    addressSpace[iiAddress] = command.value;
                }
            }
        }

        private static void ApplyCommandToAddresses(Mask mask, Command command, ref Dictionary<ulong, ulong> addressSpace)
        {
            var maskedAddress = mask.onesMask | command.address;
            //maskedAddress = ~mask.zerosMask & maskedAddress;
            maskedAddress = ~mask.xMask & maskedAddress;

            //Each iteration can be thought of as a distinct number, that varies on the number of X's.
            //ulong numXs = (ulong)mask.maskString.Count(maskChar => maskChar == 'X');
            //var maxX = (ulong)Math.Pow(2, numXs);
            var firstXIndex = mask.maskString.IndexOf('X');
            var xPositionFromRight = mask.maskString.Length - firstXIndex;
            var maxX = (ulong)Math.Pow(2, xPositionFromRight) - 1;
            for (ulong ii = 0; ii <= maxX; ii++)
            {
                var iiAddress = (ii & mask.xMask) | maskedAddress;
                //ulong iiAddress = GetAddressForII(mask, ii, maskedAddress);
                addressSpace[iiAddress] = command.value;
            }
        }

        private static ulong GetAddressForII(Mask mask, long ii, ulong maskedAddress)
        {
            var iiString = Convert.ToString(ii, 2);
            var iiAddressString = string.Copy(mask.maskString);
            var iiAddressJJ = iiString.Length - 1;
            for (var jj = iiAddressString.Length - 1; jj >= 0; jj--)
            {
                if (iiAddressJJ < 0)
                {
                    if (iiAddressString[jj] == 'X')
                    {
                        iiAddressString = iiAddressString.Remove(jj, 1).Insert(jj, "0");
                    }
                    continue;
                }
                if (iiAddressString[jj] == 'X')
                {
                    iiAddressString = iiAddressString.Remove(jj, 1).Insert(jj, iiString[iiAddressJJ].ToString());
                }
                iiAddressJJ--;
            }

            var iiAddress = Convert.ToUInt64(iiAddressString, 2);
            return iiAddress | maskedAddress;
        }








        public static void Part1()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;

            var addressSpace = new Dictionary<ulong, ulong>();

            Mask currentMask = null;
            foreach (var line in inputStrings)
            {
                if(line.StartsWith("mask = "))
                {
                    currentMask = new Mask(line);
                    continue;
                }
                var command = new Command(line);
                addressSpace[command.address] = currentMask.ApplyMask(command.value);
            }


            Console.WriteLine($"Result:\n{string.Join("\n", addressSpace.Select(kv => $"{kv.Key} = {kv.Value}"))}");
            ulong sum = 0;
            foreach (var addressValue in addressSpace.Values)
            {
                sum += addressValue;
            }

            Console.WriteLine($"Sum of address space = {sum}");
        }

        public class Mask
        {
            public string maskString;
            public ulong onesMask;
            public ulong zerosMask;
            public ulong xMask;
            public ulong xMaskZeroes;
            public Mask(string line)
            {
                maskString = line.Split('=').Last().Trim();
                var lengthAdjustedMask = maskString;
                while (lengthAdjustedMask.Length < 64)
                {
                    lengthAdjustedMask = lengthAdjustedMask.Insert(0, "X");
                }
                onesMask = Convert.ToUInt64(string.Join("", maskString.Select(maskChar => charToBinary(maskChar, '0'))), 2); //use with OR
                zerosMask = Convert.ToUInt64(string.Join("", maskString.Select(maskChar => charToBinary(maskChar, '1'))), 2); //use with AND
                xMask = Convert.ToUInt64(string.Join("", maskString.Select(maskChar => charToXes(maskChar))), 2); //use with addresses
                xMaskZeroes = Convert.ToUInt64(string.Join("", maskString.Select(maskChar => charToXesZeros(maskChar))), 2); //use with addresses
            }

            public ulong ApplyMask(ulong value)
            {
                return (value | onesMask) & zerosMask;
            }

            public override string ToString()
            {
                return $"MaskString: {maskString}, onesMask:{Convert.ToString((long)onesMask, 2)} ({onesMask})";
            }

        }



        private static char charToXesZeros(char maskChar)
        {
            switch (maskChar)
            {
                case 'X':
                    return '0';
            }
            return '1';
        }
        private static char charToXes(char maskChar)
        {
            switch (maskChar)
            {
                case 'X':
                    return '1';
            }
            return '0';
        }
        public static char charToBinary(char maskChar, char xChar)
        {
            switch (maskChar)
            {
                case 'X':
                    return xChar;
                case '1':
                    return '1';
                case '0':
                    return '0';
                default:
                    break;
            }
            return '0';
        }
        

        public class Command
        {
            public ulong address;
            public ulong value;

            public Command(string command)
            {
                var equals = command.Split('=');
                value = ulong.Parse(equals.Last());
                //This is ugly:
                var mem = equals[0].Split(']')[0].Split('[').Last();
                address = ulong.Parse(mem);
            }

            public ulong ApplyCommand()
            {
                return 0;
            }
        }
    }
}
