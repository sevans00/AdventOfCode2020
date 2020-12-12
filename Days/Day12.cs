using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    public class Day12
    {
        public static string inputFilePath = @"C:\Users\Shaun\Documents\Projects\AdventOfCode\AdventOfCode2020\AdventOfCode2020\Inputs\inputDay12.txt";

        public static string[] testInput = new string[]
        {
            "F10",
            "N3",
            "F7",
            "R90",
            "F11",
        };



        public static void Part1()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;

            var currentPosition = Vector2.Zero;
            var currentFacing = Vector2.East;
            foreach (var instruction in inputStrings)
            {
                var action = instruction[0];
                var value = int.Parse(instruction.Remove(0, 1));
                switch(action)
                {
                    case 'N': //move North by the given value.
                        currentPosition += Vector2.North * value;
                        break;
                    case 'S': //move South by the given value.
                        currentPosition += Vector2.South * value;
                        break;
                    case 'E': //move East by the given value.
                        currentPosition += Vector2.East * value;
                        break;
                    case 'W': //move West by the given value.
                        currentPosition += Vector2.West * value;
                        break;
                    case 'L': //turn Left the given number of degrees.
                        while(value > 0)
                        {
                            currentFacing = currentFacing.RotateLeft();
                            value -= 90;
                        }
                        break;
                    case 'R': //turn Right the given number of degrees.
                        while (value > 0)
                        {
                            currentFacing = currentFacing.RotateRight();
                            value -= 90;
                        }
                        break;
                    case 'F': //move forward by the given value in the direction the ship is currently facing.
                        currentPosition += currentFacing * value;
                        break;
                    default:
                        Console.WriteLine($"Unexpected direction! {action} {value}");
                        break;
                }

            }

            var manhattanDistance = Math.Abs(currentPosition.x) + Math.Abs(currentPosition.y);
            Console.WriteLine($"Current Position: {currentPosition}");
            Console.WriteLine($"Manhattan Distance: {manhattanDistance}");
        }


        public static void Part2()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;

            var shipPosition = Vector2.Zero;

            var waypointPositionRelativeToShip = new Vector2(10, 1);


            foreach (var instruction in inputStrings)
            {
                var action = instruction[0];
                var value = int.Parse(instruction.Remove(0, 1));
                switch (action)
                {
                    case 'N': //move North by the given value.
                        waypointPositionRelativeToShip += Vector2.North * value;
                        break;
                    case 'S': //move South by the given value.
                        waypointPositionRelativeToShip += Vector2.South * value;
                        break;
                    case 'E': //move East by the given value.
                        waypointPositionRelativeToShip += Vector2.East * value;
                        break;
                    case 'W': //move West by the given value.
                        waypointPositionRelativeToShip += Vector2.West * value;
                        break;
                    case 'L': //turn Left the given number of degrees.
                        while (value > 0)
                        {
                            waypointPositionRelativeToShip = waypointPositionRelativeToShip.RotateLeft();
                            value -= 90;
                        }
                        break;
                    case 'R': //turn Right the given number of degrees.
                        while (value > 0)
                        {
                            waypointPositionRelativeToShip = waypointPositionRelativeToShip.RotateRight();
                            value -= 90;
                        }
                        break;
                    case 'F': //move forward by the given value in the direction the ship is currently facing.
                        shipPosition += waypointPositionRelativeToShip * value;
                        break;
                    default:
                        Console.WriteLine($"Unexpected direction! {action} {value}");
                        break;
                }

            }

            var manhattanDistance = Math.Abs(shipPosition.x) + Math.Abs(shipPosition.y);
            Console.WriteLine($"Current Position: {shipPosition}");
            Console.WriteLine($"Manhattan Distance: {manhattanDistance}");
        }

        public class Vector2
        {
            public static Vector2 North  { get { return  new Vector2(0, 1); } }
            public static Vector2 South  { get { return  new Vector2(0, -1); } }
            public static Vector2 East  { get { return  new Vector2(1, 0); } }
            public static Vector2 West  { get { return  new Vector2(-1, 0); } }

            public static Vector2 Zero { get { return new Vector2(0, 0); } }


            public int x, y;
            public Vector2(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public static Vector2 operator +(Vector2 vectorA, Vector2 vectorB)
            {
                return new Vector2(vectorA.x + vectorB.x, vectorA.y + vectorB.y);
            }

            public static Vector2 operator *(Vector2 vectorA, Vector2 vectorB)
            {
                return new Vector2(vectorA.x * vectorB.x, vectorA.y * vectorB.y);
            }
            public static Vector2 operator *(int scalar, Vector2 vectorB)
            {
                return new Vector2(scalar * vectorB.x, scalar * vectorB.y);
            }
            public static Vector2 operator *(Vector2 vectorA, int scalar)
            {
                return new Vector2(vectorA.x * scalar, vectorA.y * scalar);
            }


            public Vector2 RotateLeft()
            {
                return new Vector2(-y, x);
            }

            public Vector2 RotateRight()
            {
                return new Vector2(y, -x);
            }

            public override string ToString()
            {
                return $"[{x},{y}]";
            }
        }
    }
}
