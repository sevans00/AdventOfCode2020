using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    public class Day8
    {
        public static string inputFilePath = @"C:\Users\Shaun\Documents\Projects\AdventOfCode\AdventOfCode2020\AdventOfCode2020\Inputs\inputDay8.txt";
        public static string[] testInput = new string[]
        {
            "nop +0",
            "acc +1",
            "jmp +4",
            "acc +3",
            "jmp -3",
            "acc -99",
            "acc +1",
            "jmp -4",
            "acc +6",
        };




        public static void Part1()
        {
            //var inputStrings = File.ReadAllLines(inputFilePath);
            var inputStrings = testInput;


            var instructions = inputStrings;
            var currentInstruction = 0;
            var instructionAlreadyExecuted = new Dictionary<int, bool>();
            var accumulator = 0;
            while (true)
            {
                if (instructionAlreadyExecuted.ContainsKey(currentInstruction))
                {
                    break;
                }
                instructionAlreadyExecuted[currentInstruction] = true;
                var instruction = instructions[currentInstruction];
                var operation = instruction.Split(' ')[0];
                var value = instruction.Split(' ')[1];
                switch (operation)
                {
                    case "nop":
                        break;
                    case "acc":
                        accumulator += int.Parse(value);
                        break;
                    case "jmp":
                        currentInstruction += int.Parse(value);
                        continue;
                    default:
                        Console.WriteLine($"Unknown instruction! {operation} Value:{value}");
                        break;
                }
                currentInstruction++;
            }
            Console.WriteLine($"Accumulator = {accumulator}");
        }


        public static void Part2()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;

            var instructions = inputStrings;

            var currentModifiedLine = 0;
            while(currentModifiedLine < instructions.Length)
            {
                var originalLine = instructions[currentModifiedLine];
                var newLine = SwapJmpAndNot(instructions[currentModifiedLine]);
                instructions[currentModifiedLine] = newLine;
                Console.WriteLine($"Swapping {currentModifiedLine}");
                var loopedInstructionNumber = GetLoopedInstructionNumber(instructions);
                if (loopedInstructionNumber == instructions.Length)
                {
                    Console.WriteLine($"No loop detected!");
                    break;
                }
                instructions[currentModifiedLine] = originalLine;
                currentModifiedLine++;
            }

            Console.WriteLine($"Accumulator Value: {GetAccumulator(instructions)}");



            /*
                        var loopedInstructionNumber = GetLoopedInstructionNumber(instructions);
                        Console.WriteLine(loopedInstructionNumber);
                        Console.WriteLine($"Looped Instruction Number: {GetLoopedInstructionNumber(instructions)}");
                        if(loopedInstructionNumber == instructions.Length)
                        {
                            Console.WriteLine($"No loop detected!");
                        }
                        Console.WriteLine($"Accumulator Value: {GetAccumulator(instructions)}");*/
        }

        private static string SwapJmpAndNot(string instruction)
        {
            if(instruction.StartsWith("jmp"))
            {
                return instruction.Replace("jmp", "nop");
            }
            if(instruction.StartsWith("nop"))
            {
                return instruction.Replace("nop", "jmp");
            }
            return instruction;
        }



        private static int GetLoopedInstructionNumber(string[] instructions)
        {
            var currentInstruction = 0;
            var instructionAlreadyExecuted = new Dictionary<int, bool>();
            var accumulator = 0;
            while (currentInstruction < instructions.Length)
            {
                if (instructionAlreadyExecuted.ContainsKey(currentInstruction))
                {
                    break;
                }
                instructionAlreadyExecuted[currentInstruction] = true;
                var instruction = instructions[currentInstruction];
                var operation = instruction.Split(' ')[0];
                var value = instruction.Split(' ')[1];
                switch (operation)
                {
                    case "nop":
                        break;
                    case "acc":
                        accumulator += int.Parse(value);
                        break;
                    case "jmp":
                        currentInstruction += int.Parse(value);
                        continue;
                    default:
                        Console.WriteLine($"Unknown instruction! {operation} Value:{value}");
                        break;
                }
                currentInstruction++;
            }
            return currentInstruction;
        }

        private static int GetAccumulator(string[] instructions)
        {
            var currentInstruction = 0;
            var instructionAlreadyExecuted = new Dictionary<int, bool>();
            var accumulator = 0;
            while (currentInstruction < instructions.Length)
            {
                if (instructionAlreadyExecuted.ContainsKey(currentInstruction))
                {
                    break;
                }
                instructionAlreadyExecuted[currentInstruction] = true;
                var instruction = instructions[currentInstruction];
                var operation = instruction.Split(' ')[0];
                var value = instruction.Split(' ')[1];
                switch (operation)
                {
                    case "nop":
                        break;
                    case "acc":
                        accumulator += int.Parse(value);
                        break;
                    case "jmp":
                        currentInstruction += int.Parse(value);
                        continue;
                    default:
                        Console.WriteLine($"Unknown instruction! {operation} Value:{value}");
                        break;
                }
                currentInstruction++;
            }
            return accumulator;
        }

    }
}
