using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    public class Day11
    {
        public static string inputFilePath = @"C:\Users\Shaun\Documents\Projects\AdventOfCode\AdventOfCode2020\AdventOfCode2020\Inputs\inputDay11.txt";

        public static string[] testInput = new string[]
        {
            "L.LL.LL.LL",
            "LLLLLLL.LL",
            "L.L.L..L..",
            "LLLL.LL.LL",
            "L.LL.LL.LL",
            "L.LLLLL.LL",
            "..L.L.....",
            "LLLLLLLLLL",
            "L.LLLLLL.L",
            "L.LLLLL.LL",
        };



        public static void Part1()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;


            var seatMap = new SeatMap(inputStrings);

            Console.WriteLine($"Original:\n{seatMap}");
            var stepsTillStabilize = 0;
            while (true)
            {
                var changedSeats = seatMap.Step();
                stepsTillStabilize++;
                //Console.WriteLine($"\nChanges: {changedSeats}\n{seatMap}");
                if (changedSeats == 0)
                    break;
            }
            Console.WriteLine($"\nSteps to Stabilize = {stepsTillStabilize}");
            var occupied = seatMap.Count('#');
            Console.WriteLine($"\nSeats Occupied = {occupied}");

        }


        public static void Part2()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;


            var seatMap = new SeatMap(inputStrings);

            Console.WriteLine($"Original:\n{seatMap}");
            var stepsTillStabilize = 0;
            while (true)
            {
                var changedSeats = seatMap.Step2();
                stepsTillStabilize++;
                //Console.WriteLine($"\nChanges: {changedSeats}\n{seatMap}");
                if (changedSeats == 0)
                    break;
            }
            Console.WriteLine($"\nSteps to Stabilize = {stepsTillStabilize}");
            var occupied = seatMap.Count('#');
            Console.WriteLine($"\nSeats Occupied = {occupied}");
        }


        public class SeatMap
        {
            public char[,] map;
            public int width;
            public int height;
            public SeatMap(string[] inputStrings)
            {
                width = inputStrings[0].Length;
                height = inputStrings.Length;
                map = new char[width, height];
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        map[x, y] = inputStrings[y][x];
                    }
                }
            }

            //There's probably a better way to do this.  But it's early in the morning right now.
            private char[,] Copy(char[,] mapToCopy)
            {
                var newMap = new char[width, height];
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        newMap[x, y] = mapToCopy[x, y];
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
                        var newCell = GetStepCellAt(previousMap, x, y);
                        map[x, y] = newCell;
                        if (newCell != previousMap[x, y])
                        {
                            numChanges++;
                        }
                    }
                }
                return numChanges;
            }

            private char GetStepCellAt(char[,] map, int x, int y)
            {
                /*
                If a seat is empty (L) and there are no occupied seats adjacent to it, the seat becomes occupied.
                If a seat is occupied (#) and four or more seats adjacent to it are also occupied, the seat becomes empty.
                Otherwise, the seat's state does not change.
                 */
                var currentState = map[x, y];
                if (currentState == '.')
                {
                    return '.';
                }
                if (currentState == 'L')
                {
                    //Empty seat!
                    var adjacentCellsTo = GetAdjacentCells(map, x, y);
                    var numOccupied = adjacentCellsTo.Count( cell => cell == '#');
                    if( numOccupied == 0)
                    {
                        return '#';
                    }
                    return 'L';
                }
                if (currentState == '#')
                {
                    //Occupied!
                    var adjacentCellsTo = GetAdjacentCells(map, x, y);
                    var numOccupied = adjacentCellsTo.Count(cell => cell == '#');
                    if( numOccupied >= 4)
                    {
                        return 'L';
                    }
                }
                return currentState;
            }

            private List<char> GetAdjacentCells(char[,] map, int x, int y)
            {
                var adjacentCells = new List<char>();
                var xM1 = x - 1;
                var yM1 = y - 1;
                var xP1 = x + 1;
                var yP1 = y + 1;
                var points = new List<Tuple<int, int>>()
                {
                    new Tuple<int, int>(xM1, yM1),
                    new Tuple<int, int>(xM1, y  ),
                    new Tuple<int, int>(xM1, yP1),
                    new Tuple<int, int>(x,   yM1),
                    new Tuple<int, int>(x,   yP1),
                    new Tuple<int, int>(xP1, yM1),
                    new Tuple<int, int>(xP1, y  ),
                    new Tuple<int, int>(xP1, yP1),
                };

                foreach (var point in points)
                {
                    if (IsInMap(point, width, height))
                    {
                        adjacentCells.Add(map[point.Item1, point.Item2]);
                    }
                }
                return adjacentCells;
            }

            private bool IsInMap(Tuple<int, int> point, int width, int height)
            {
                if (
                    point.Item1 < 0 ||
                    point.Item1 >= width ||
                    point.Item2 < 0 ||
                    point.Item2 >= height
                    )
                    return false;
                return true;
            }

            public override string ToString()
            {
                var str = "";
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        str += map[x, y];
                    }
                    str += "\n";
                }
                return str;
            }

            public int Count(char v)
            {
                var count = 0;
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if(map[x,y] == v)
                        {
                            count++;
                        }
                    }
                }
                return count;
            }

            public int Step2()
            {
                var numChanges = 0;
                var previousMap = Copy(map);

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        var newCell = GetStepCellAt2(previousMap, x, y);
                        map[x, y] = newCell;
                        if (newCell != previousMap[x, y])
                        {
                            numChanges++;
                        }
                    }
                }
                return numChanges;
            }


            private char GetStepCellAt2(char[,] map, int x, int y)
            {
                /*
                If a seat is empty (L) and there are no occupied seats adjacent to it, the seat becomes occupied.
                If a seat is occupied (#) and four or more seats adjacent to it are also occupied, the seat becomes empty.
                Otherwise, the seat's state does not change.
                 */
                var currentState = map[x, y];
                if (currentState == '.')
                {
                    return '.';
                }
                if (currentState == 'L')
                {
                    //Empty seat!
                    var adjacentCellsTo = GetLineOfSightSCells(map, x, y);
                    var numOccupied = adjacentCellsTo.Count(cell => cell == '#');
                    if (numOccupied == 0)
                    {
                        return '#';
                    }
                    return 'L';
                }
                if (currentState == '#')
                {
                    //Occupied!
                    var adjacentCellsTo = GetLineOfSightSCells(map, x, y);
                    var numOccupied = adjacentCellsTo.Count(cell => cell == '#');
                    //Also, people seem to be more tolerant than you expected: 
                    //it now takes five or more visible occupied seats for an 
                    //occupied seat to become empty(rather than four or more from the previous rules)
                    if (numOccupied >= 5)
                    {
                        return 'L';
                    }
                }
                return currentState;
            }


            private List<char> GetLineOfSightSCells(char[,] map, int x, int y)
            {
                var adjacentCells = new List<char>();
                var directions = new List<Vector2>()
                {
                    new Vector2(-1, -1),
                    new Vector2(-1, 0),
                    new Vector2(-1, 1),
                    new Vector2(0, -1),
                    new Vector2(0, 1),
                    new Vector2(1, -1),
                    new Vector2(1, 0),
                    new Vector2(1, 1),
                };

                foreach (var direction in directions)
                {
                    var startingPoint = new Vector2(x, y);
                    var point = startingPoint + direction;
                    while(IsInMap(point, width, height))
                    {
                        if (map[point.x, point.y] != '.')
                        {
                            adjacentCells.Add(map[point.x, point.y]);
                            break;
                        }
                        point += direction;
                    }

                }
                return adjacentCells;
            }


            public class Vector2
            {
                public int x, y;
                public Vector2 (int x, int y)
                {
                    this.x = x;
                    this.y = y;
                }

                public static Vector2 operator +(Vector2 vectorA, Vector2 vectorB)
                {
                    return new Vector2(vectorA.x + vectorB.x, vectorA.y + vectorB.y);
                }
            }
            private bool IsInMap(Vector2 point, int width, int height)
            {
                if (
                    point.x < 0 ||
                    point.x >= width ||
                    point.y < 0 ||
                    point.y >= height
                    )
                    return false;
                return true;
            }

        }
    }
}