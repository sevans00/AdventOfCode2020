using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    public class Day20
    {

        public static string inputFilePath = @"C:\Users\Shaun\Documents\Projects\AdventOfCode\AdventOfCode2020\AdventOfCode2020\Inputs\inputDay20.txt";


        public static string[] testInput0 = new string[]
        {
            "Tile 2311:", //210, 89, 924, 318
            "..##.#..#.",
            "##..#.....",
            "#...##..#.",
            "####.#...#",
            "##.##.###.",
            "##...#.###",
            ".#.#.#..##",
            "..#....#..",
            "###...#.#.",
            "..###..###",
            "",
            "Tile 1951:", //710, 498, 177, 587
            "#.##...##.",
            "#.####...#",
            ".....#..##",
            "#...######",
            ".##.#....#",
            ".###.#####",
            "###.##.##.",
            ".###....#.",
            "..#.#..#.#",
            "#...##.#..",
        };



        public static string[] testInput = new string[]
        {
            "Tile 2311:",
            "..##.#..#.",
            "##..#.....",
            "#...##..#.",
            "####.#...#",
            "##.##.###.",
            "##...#.###",
            ".#.#.#..##",
            "..#....#..",
            "###...#.#.",
            "..###..###",
            "",
            "Tile 1951:",
            "#.##...##.",
            "#.####...#",
            ".....#..##",
            "#...######",
            ".##.#....#",
            ".###.#####",
            "###.##.##.",
            ".###....#.",
            "..#.#..#.#",
            "#...##.#..",
            "",
            "Tile 1171:",
            "####...##.",
            "#..##.#..#",
            "##.#..#.#.",
            ".###.####.",
            "..###.####",
            ".##....##.",
            ".#...####.",
            "#.##.####.",
            "####..#...",
            ".....##...",
            "",
            "Tile 1427:",
            "###.##.#..",
            ".#..#.##..",
            ".#.##.#..#",
            "#.#.#.##.#",
            "....#...##",
            "...##..##.",
            "...#.#####",
            ".#.####.#.",
            "..#..###.#",
            "..##.#..#.",
            "",
            "Tile 1489:",
            "##.#.#....",
            "..##...#..",
            ".##..##...",
            "..#...#...",
            "#####...#.",
            "#..#.#.#.#",
            "...#.#.#..",
            "##.#...##.",
            "..##.##.##",
            "###.##.#..",
            "",
            "Tile 2473:",
            "#....####.",
            "#..#.##...",
            "#.##..#...",
            "######.#.#",
            ".#...#.#.#",
            ".#########",
            ".###.#..#.",
            "########.#",
            "##...##.#.",
            "..###.#.#.",
            "",
            "Tile 2971:",
            "..#.#....#",
            "#...###...",
            "#.#.###...",
            "##.##..#..",
            ".#####..##",
            ".#..####.#",
            "#..#.#..#.",
            "..####.###",
            "..#.#.###.",
            "...#.#.#.#",
            "",
            "Tile 2729:",
            "...#.#.#.#",
            "####.#....",
            "..#.#.....",
            "....#..#.#",
            ".##..##.#.",
            ".#.####...",
            "####.#.#..",
            "##.####...",
            "##..#.##..",
            "#.##...##.",
            "",
            "Tile 3079:",
            "#.#.#####.",
            ".#..######",
            "..#.......",
            "######....",
            "####.#..#.",
            ".#...#.##.",
            "#.#####.##",
            "..#.###...",
            "..#.......",
            "..#.###...",
        };

        public static void Part1()
        {
            //var inputStrings = File.ReadAllLines(inputFilePath);
            var inputStrings = testInput;


            var tiles = new List<Tile>();
            var currentTileStrings = new List<string>();
            foreach (var inputString in inputStrings)
            {
                if(string.IsNullOrEmpty(inputString))
                {
                    tiles.Add(new Tile(currentTileStrings));
                    currentTileStrings = new List<string>();
                    continue;
                }
                currentTileStrings.Add(inputString);
            }
            tiles.Add(new Tile(currentTileStrings));

            Console.WriteLine(string.Join("\n\n", tiles));

            Console.WriteLine($"{tiles.Count}");

            
            var arrangedTiles = ArrangeTiles(tiles);
            Console.WriteLine($"Max Used Count: {maxUsedCount}");
            if(arrangedTiles == null)
            {
                Console.WriteLine("Did not find...");
                return;
            }

            for (int ii = 0; ii < arrangedTiles.GetLength(0); ii++)
            {
                for (int jj = 0; jj < arrangedTiles.GetLength(1); jj++)
                {
                    Console.Write($"[{arrangedTiles[ii, jj]?.Id}] ");
                }
                Console.WriteLine();
            }

        }

        private static Tile[,] ArrangeTiles(List<Tile> tiles)
        {
            var sideLength = (int)Math.Sqrt(tiles.Count);

            var arrangedTiles = new Tile[sideLength, sideLength];

            var allTilePossibilities = tiles.SelectMany(tile => tile.GetTilePermutations()).ToList();
            return ArrangeTiles(tiles, allTilePossibilities, arrangedTiles, new List<int>());
        }

        public static void WriteLine(string str)
        {
            //Console.WriteLine(str);
        }

        private static int maxUsedCount = 0;
        private static Tile[,] ArrangeTiles(List<Tile> tiles, List<Tile> allTilePossibilities, Tile[,] currentMap, List<int> usedIds)
        {
            Console.WriteLine($"ArrangeTiles: UsedIds: [{usedIds.Count}] {string.Join(",",usedIds)}");
            if(usedIds.Count > maxUsedCount)
            {
                maxUsedCount = usedIds.Count;
            }
            if(usedIds.Count == tiles.Count)
            {
                //return currentMap;
            }
            var map = CopyMap(currentMap);

            ConsoleWriteMap(map);

            FindFirstOpenSpot(map, out var x, out var y);
            if (x < 0 && y < 0)
                return currentMap;

            if(map[0,0]?.Id == 1951 && map[1,0]?.Id == 2311)
            {
                Console.WriteLine("1951");
            }

            foreach (var tile in allTilePossibilities)
            {
                if(usedIds.Contains(tile.Id))
                {
                    continue;
                }
                WriteLine($"Testing {tile}");

                if (map[0, 0]?.Id == 1951 && map[1, 0]?.Id == 2311 && tile.Id == 3079)
                {
                    Console.WriteLine("1951");
                }

                if (DoesTileFit(map, tile, x, y))
                {
                    WriteLine($"Tile Fits! {tile}");
                    //This fits...
                    map[x, y] = tile;
                    var newUsedIds = usedIds.Select(id => id).ToList();
                    newUsedIds.Add(tile.Id);
                    var fullMap = ArrangeTiles(tiles, allTilePossibilities, map, newUsedIds);
                    if (fullMap != null)
                        return fullMap;
                }
            }
            Console.WriteLine($"--- Nothing Fit ---");
            return null;
        }

        private static void ConsoleWriteMap(Tile[,] map)
        {
            for (int ii = 0; ii < map.GetLength(0); ii++)
            {
                for (int jj = 0; jj < map.GetLength(1); jj++)
                {
                    Console.Write($"[{map[ii, jj]?.Id + map[ii, jj]?.label}] ");
                }
                Console.WriteLine();
            }
        }

        private static Tile[,] CopyMap(Tile[,] currentMap)
        {
            var map = new Tile[currentMap.GetLength(0),currentMap.GetLength(1)];
            Array.Copy(currentMap, map, currentMap.Length);
            return map;
        }



        /*
         * Clear tiles:
                for (int ii = 0; ii<arrangedTiles.GetLength(0); ii++)
                    {
                        for (int jj = 0; jj<arrangedTiles.GetLength(1); jj++)
                        {
                            arrangedTiles[ii, jj] = null;
                        }
        }*/

        private static bool DoesTileFit(Tile[,] arrangedTiles, Tile tile, int x, int y)
        {
            Tile testTile = null;
            //Test Tile Is LEFT of tile:
            if (x > 0)
            {
                testTile = arrangedTiles[x - 1, y];
            }
            if(testTile != null)
            {
                if(testTile.RightNumber != tile.LeftNumber)
                {
                    return false;
                }
            }

            //Test Tile Is RIGHT of tile:
            testTile = null;
            if (x < arrangedTiles.GetLength(0) - 1)
            {
                testTile = arrangedTiles[x + 1, y];
            }
            if (testTile != null)
            {
                if (testTile.LeftNumber != tile.RightNumber)
                {
                    return false;
                }
            }

            //Test Tile Is TOP of tile:
            testTile = null;
            if (y > 0)
            {
                testTile = arrangedTiles[x, y - 1];
            }
            if (testTile != null)
            {
                if (testTile.BottomNumber != tile.TopNumber)
                {
                    return false;
                }
            }

            //Test Tile Is BOTTOM of tile:
            testTile = null;
            if (y < arrangedTiles.GetLength(1) - 1)
            {
                testTile = arrangedTiles[x, y + 1];
            }
            if (testTile != null)
            {
                if (testTile.TopNumber != tile.BottomNumber)
                {
                    return false;
                }
            }

            //All tests passed!
            return true;
        }

        private static void FindFirstOpenSpot(Tile[,] arrangedTiles, out int x, out int y)
        {
            for (int yy = 0; yy < arrangedTiles.GetLength(1); yy++)
            {
                for (int xx = 0; xx < arrangedTiles.GetLength(0); xx++)
                {
                    var testTile = arrangedTiles[xx, yy];
                    if (testTile == null)
                    {
                        x = xx;
                        y = yy;
                        return;
                    }
                }
            }
            x = -1;
            y = -1;
        }

        public class Tile
        {
            public int Id;

            public List<string> imageData;

            public int TopNumber;
            public int RightNumber;
            public int BottomNumber;
            public int LeftNumber;

            public string label;

            public Tile(List<string> inputStrings)
            {
                //"Tile 3079:",
                Id = int.Parse(inputStrings[0].Split(' ')[1].TrimEnd(':'));

                imageData = inputStrings.Skip(1).ToList();

                TopNumber = GetEdgeNumber(imageData[0]);
                RightNumber = GetEdgeNumber(string.Join("", imageData.Select(str => str[str.Length - 1])));
                BottomNumber = GetEdgeNumber(imageData[imageData.Count - 1]);
                LeftNumber = GetEdgeNumber(string.Join("", imageData.Select(str => str[0])) );
                label = " ";
            }
            public Tile(int id, int topNumber, int rightNumber, int bottomNumber, int leftNumber, string label, List<string> imageData)
            {
                Id = id;

                TopNumber = topNumber;
                RightNumber = rightNumber;
                BottomNumber = bottomNumber;
                LeftNumber = leftNumber;

                this.label = label;
                this.imageData = imageData;
            }

            public override string ToString()
            {
                return $"{Id}{label}\t\t{TopNumber},\t{RightNumber},\t{BottomNumber},\t{LeftNumber}";
                //return $"{Id}{label}";
                //return $"Tile {Id}:\n{string.Join("\n", imageData)}";
            }

            public static int GetEdgeNumber(string edgeString)
            {
                return Convert.ToInt32(edgeString.Replace('.', '0').Replace('#', '1'), 2);
            }


            public Tile RotateClockwise()
            {
                return new Tile(Id, FlipBinaryNumber(LeftNumber), TopNumber, FlipBinaryNumber(RightNumber), BottomNumber, label+"R", imageData);
            }
            private Tile RotateClockwiseTwice()
            {
                return new Tile(Id, FlipBinaryNumber(BottomNumber), FlipBinaryNumber(LeftNumber), FlipBinaryNumber(TopNumber), FlipBinaryNumber(RightNumber), label + "RR", imageData);
            }

            public Tile FlipX()
            {
                return new Tile(Id, FlipBinaryNumber(TopNumber), LeftNumber, FlipBinaryNumber(BottomNumber), RightNumber, label + "FlX", imageData);
            }

            public Tile FlipY()
            {
                return new Tile(Id, BottomNumber, FlipBinaryNumber(RightNumber), TopNumber, FlipBinaryNumber(LeftNumber), label + "FlY", imageData);
            }

            private int FlipBinaryNumber(int number)
            {
                var binaryLength = imageData.Count();
                var numberString = Convert.ToString(number, 2).PadLeft(binaryLength, '0');
                return Convert.ToInt32(numberString, 2);
            }


            public List<Tile> GetTilePermutations()
            {
                return new List<Tile>()
                {
                    this,
                    FlipX(),
                    FlipY(),
                    FlipX().FlipY(), //This is probably the same as something else... ah, right, double rotation
                    RotateClockwise(),
                    RotateClockwiseTwice(), 
                    RotateClockwise().RotateClockwise().RotateClockwise(),
                    FlipX().RotateClockwise(),
                    FlipX().RotateClockwiseTwice(),
                    FlipX().RotateClockwiseTwice().RotateClockwise(),
                    FlipY().RotateClockwise(),
                    FlipY().RotateClockwiseTwice(),
                    FlipY().RotateClockwiseTwice().RotateClockwise(),
                };
            }

        }

    }
}