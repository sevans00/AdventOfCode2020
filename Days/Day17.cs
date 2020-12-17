using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    public class Day17
    {
        public static string inputFilePath = @"C:\Users\Shaun\Documents\Projects\AdventOfCode\AdventOfCode2020\AdventOfCode2020\Inputs\inputDay17.txt";

        public static string[] testInput = new string[]
        {
            ".#.",
            "..#",
            "###",
        };

        public static void Part1()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;

            var batMap = new BatMap(inputStrings);

            //Console.WriteLine(batMap);
            Console.WriteLine($"Initial Count = {batMap.Count()}");

            for (int ii = 1; ii <= 6; ii++)
            {
                var stepChanges = batMap.Step();
                Console.WriteLine($"Step {ii}, changes={stepChanges}, Count = {batMap.Count()}");
            }

            var count = batMap.Count();
            Console.WriteLine($"Count = {count}");
        }

        public static void Part2()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;

            var batMap = new BatMap2(inputStrings);

            //Console.WriteLine(batMap);
            Console.WriteLine($"Initial Count = {batMap.Count()}");

            for (int ii = 1; ii <= 6; ii++)
            {
                var stepChanges = batMap.Step();
                Console.WriteLine($"Step {ii}, changes={stepChanges}, Count = {batMap.Count()}");
            }

            var count = batMap.Count();
            Console.WriteLine($"Count = {count}");
        }

        public class BatMap
        {
            public char[,,] map;
            public static int dimension = 100;
            public int width = dimension;
            public int depth = dimension;
            public int height = dimension;
            public BatMap(string[] inputStrings)
            {
                map = new char[width, depth, height];
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int z = 0; z < depth; z++)
                        {
                            map[x, y, z] = '.';
                        }
                    }
                }
                int halfW = dimension / 2 - inputStrings[0].Length / 2;
                int halfH = dimension / 2 - inputStrings[0].Length / 2;
                int halfD = dimension / 2 - inputStrings[0].Length / 2;
                for (int y = 0; y < inputStrings.Length; y++)
                {
                    for (int x = 0; x < inputStrings[0].Length; x++)
                    {
                        map[x + halfW, y + halfH, halfD] = inputStrings[x][y];
                    }
                }
            }
            
            private char[,,] Copy(char[,,] mapToCopy)
            {
                var newMap = new char[width, height, height];
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int z = 0; z < depth; z++)
                        {
                            newMap[x, y, z] = mapToCopy[x, y, z];
                        }
                    }
                }
                return newMap;
            }

            public int Step()
            {
                var numChanges = 0;
                var previousMap = Copy(map);

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int z = 0; z < depth; z++)
                        {
                            var newCell = GetStepCellAt(previousMap, x, y, z);
                            map[x, y, z] = newCell;
                            if (newCell != previousMap[x, y, z])
                            {
                                numChanges++;
                            }
                        }
                    }
                }
                return numChanges;
            }

            private char GetStepCellAt(char[,,] map, int x, int y, int z)
            {
                /*
                If a cube is active and exactly 2 or 3 of its neighbors are also active, the cube remains active. Otherwise, the cube becomes inactive.
                If a cube is inactive but exactly 3 of its neighbors are active, the cube becomes active. Otherwise, the cube remains inactive.
                 */
                var currentState = map[x, y, z];
                if (currentState == '#')
                {
                    //Occupied!
                    var numActive = GetAdjacentCellActiveCount(map, x, y, z);
                    if (numActive == 2 || numActive == 3)
                    {
                        return '#';
                    }
                    return '.';
                }
                if (currentState == '.')
                {
                    var numActive = GetAdjacentCellActiveCount(map, x, y, z);
                    if (numActive == 3)
                    {
                        return '#';
                    }
                    return '.';
                }
                return currentState;
            }

            private int GetAdjacentCellActiveCount(char[,,] map, int centerX, int centerY, int centerZ)
            {
                int sum = 0;
                for (int x = centerX-1; x <= centerX+1; x++)
                {
                    for (int y = centerY-1; y <= centerY+1; y++)
                    {
                        for (int z = centerZ-1; z <= centerZ+1; z++)
                        {
                            if (
                                x == centerX
                                && y == centerY
                                && z == centerZ
                                )
                                continue;
                            if (IsInMap(x,y,z))
                            {
                                if (map[x, y, z] == '#')
                                {
                                    sum++;
                                }
                            }
                        }
                    }
                }
                return sum;
            }

            private bool IsInMap(int x, int y, int z)
            {
                return x >= 0
                    && x < width
                    && y >= 0
                    && y < height
                    && z >= 0
                    && z < depth
                    ;
            }

            public override string ToString()
            {
                var str = "";
                for (int z = 0; z < depth; z++)
                {
                    str += $"Z={z}\n";
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++) 
                        {
                            str += map[x, y, z];
                        }
                        str += "\n";
                    }
                    str += "\n\n";
                }
                return str;
            }

            public int Count()
            {
                var sum = 0;
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int z = 0; z < depth; z++)
                        {
                            if (map[x, y, z] == '#')
                                sum++;
                        }
                    }
                }
                return sum;
            }
        }


        public class BatMap2
        {
            public char[,,,] map;
            public static int dimension = 50;
            public int width = dimension;
            public int depth = dimension;
            public int height = dimension;
            public int thlabber = dimension;
            public BatMap2(string[] inputStrings)
            {
                map = new char[width, depth, height, thlabber];
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int z = 0; z < depth; z++)
                        {
                            for (int w = 0; w < thlabber; w++)
                            {
                                map[x, y, z, w] = '.';
                            }
                        }
                    }
                }
                int halfW = dimension / 2 - inputStrings[0].Length / 2;
                int halfH = dimension / 2 - inputStrings[0].Length / 2;
                int halfD = dimension / 2 - inputStrings[0].Length / 2;
                int halfTh = dimension / 2 - inputStrings[0].Length / 2;
                for (int y = 0; y < inputStrings.Length; y++)
                {
                    for (int x = 0; x < inputStrings[0].Length; x++)
                    {
                        map[x + halfW, y + halfH, halfD, halfTh] = inputStrings[x][y];
                    }
                }
            }

            private char[,,,] Copy(char[,,,] mapToCopy)
            {
                var newMap = new char[width, height, height, thlabber];
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int z = 0; z < depth; z++)
                        {
                            for (int w = 0; w < thlabber; w++)
                            {
                                newMap[x, y, z, w] = mapToCopy[x, y, z, w];
                            }
                        }
                    }
                }
                return newMap;
            }

            public int Step()
            {
                var numChanges = 0;
                var previousMap = Copy(map);

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int z = 0; z < depth; z++)
                        {
                            for (int w = 0; w < thlabber; w++)
                            {
                                var newCell = GetStepCellAt(previousMap, x, y, z, w);
                                map[x, y, z, w] = newCell;
                                if (newCell != previousMap[x, y, z, w])
                                {
                                    numChanges++;
                                }
                            }
                        }
                    }
                }
                return numChanges;
            }

            private char GetStepCellAt(char[,,,] map, int x, int y, int z, int w)
            {
                /*
                If a cube is active and exactly 2 or 3 of its neighbors are also active, the cube remains active. Otherwise, the cube becomes inactive.
                If a cube is inactive but exactly 3 of its neighbors are active, the cube becomes active. Otherwise, the cube remains inactive.
                 */
                var currentState = map[x, y, z, w];
                if (currentState == '#')
                {
                    //Occupied!
                    var numActive = GetAdjacentCellActiveCount(map, x, y, z, w);
                    if (numActive == 2 || numActive == 3)
                    {
                        return '#';
                    }
                    return '.';
                }
                if (currentState == '.')
                {
                    var numActive = GetAdjacentCellActiveCount(map, x, y, z, w);
                    if (numActive == 3)
                    {
                        return '#';
                    }
                    return '.';
                }
                return currentState;
            }

            private int GetAdjacentCellActiveCount(char[,,,] map, int centerX, int centerY, int centerZ, int centerT)
            {
                int sum = 0;
                for (int x = centerX - 1; x <= centerX + 1; x++)
                {
                    for (int y = centerY - 1; y <= centerY + 1; y++)
                    {
                        for (int z = centerZ - 1; z <= centerZ + 1; z++)
                        {
                            for (int w = centerT - 1; w <= centerT + 1; w++)
                            {
                                if (
                                    x == centerX
                                    && y == centerY
                                    && z == centerZ
                                    && w == centerT
                                    )
                                    continue;
                                if (IsInMap(x, y, z, w))
                                {
                                    if (map[x, y, z, w] == '#')
                                    {
                                        sum++;
                                    }
                                }
                            }
                        }
                    }
                }
                return sum;
            }

            private bool IsInMap(int x, int y, int z, int w)
            {
                return x >= 0
                    && x < width
                    && y >= 0
                    && y < height
                    && z >= 0
                    && z < depth
                    && w >= 0
                    && w < thlabber
                    ;
            }

            public override string ToString()
            {
                var str = "";
                for (int z = 0; z < depth; z++)
                {
                    str += $"Z={z}\n";
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            for (int w = 0; w < thlabber; w++)
                            {
                                str += map[x, y, z, w];
                            }
                            str += "\n";
                        }
                        str += "\n";
                    }
                    str += "\n\n";
                }
                return str;
            }

            public int Count()
            {
                var sum = 0;
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int z = 0; z < depth; z++)
                        {
                            for (int w = 0; w < thlabber; w++)
                            {
                                if (map[x, y, z, w] == '#')
                                    sum++;
                            }
                        }
                    }
                }
                return sum;
            }
        }

    }
}
