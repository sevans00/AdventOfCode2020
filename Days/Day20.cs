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

        public static string[] seaMonster = new string[]
        {
            "                  # ",
            "#    ##    ##    ###",
            " #  #  #  #  #  #   ",
        };

        public static void Part1()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;


            var tiles = new List<Tile>();
            var currentTileStrings = new List<string>();
            foreach (var inputString in inputStrings)
            {
                if (string.IsNullOrEmpty(inputString))
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


            var tileIdToAllTilePermutations = new Dictionary<int, List<string>>();
            foreach (var tile in tiles)
            {
                tileIdToAllTilePermutations[tile.Id] = tile.GetAllSidesPermutations();
            }
            var tilesToMatchCount = TilesToMatchCount2(tileIdToAllTilePermutations);

            var ordered = tilesToMatchCount.OrderBy(kvp => kvp.Value).Select(kvp => $"{kvp.Key} Matched: {kvp.Value} sides");
            Console.WriteLine(string.Join("\n", ordered));

            var fourMatchers = tilesToMatchCount.Where(kvp => kvp.Value == 4).Select(kvp => kvp.Key);
            ulong fourMatchersProduct = 1;
            foreach (var fourMatcher in fourMatchers)
            {
                fourMatchersProduct *= (ulong)fourMatcher;
            }
            Console.WriteLine($"Product: {fourMatchersProduct}");

            //Got our four corner matchers now



            var arrangedTiles = ArrangeTiles(tiles, tilesToMatchCount);
            Console.WriteLine($"Max Used Count: {maxUsedCount}");
            if (arrangedTiles == null)
            {
                Console.WriteLine("Did not find...");
                return;
            }

            for (int ii = 0; ii < arrangedTiles.GetLength(0); ii++)
            {
                for (int jj = 0; jj < arrangedTiles.GetLength(1); jj++)
                {
                    Console.Write($"[{arrangedTiles[ii, jj]?.Id}{arrangedTiles[ii, jj].label}] ");
                }
                Console.WriteLine();
            }

            var topLeft = arrangedTiles[0, 0];
            var topRight = arrangedTiles[arrangedTiles.GetLength(0) - 1, 0];
            var bottomLeft = arrangedTiles[0, arrangedTiles.GetLength(1) - 1];
            var bottomRight = arrangedTiles[arrangedTiles.GetLength(0) - 1, arrangedTiles.GetLength(1) - 1];

            Console.WriteLine($"{topLeft.Id}, {topRight.Id}, {bottomLeft.Id}, {bottomRight.Id}");
            Console.WriteLine($"Product: {(long)topLeft.Id * (long)topRight.Id * (long)bottomLeft.Id * (long)bottomRight.Id}");

            //We have the tiles now!

            //Testing:
            /*
            var testTile = tiles[0];
            Console.WriteLine($"{testTile.Id}:{testTile.label}\n{string.Join("\n", testTile.imageData)}");
            Console.WriteLine($"====");
            var rotatedTile = testTile.RotateClockwise();
            Console.WriteLine($"{rotatedTile.Id}:{rotatedTile.label}\n{string.Join("\n", rotatedTile.imageData)}");

            Console.WriteLine($"====");
            var flipX = testTile.FlipX();
            Console.WriteLine($"{flipX.Id}:{flipX.label}\n{string.Join("\n", flipX.imageData)}");
            Console.WriteLine($"====");
            var flipY = testTile.FlipY();
            Console.WriteLine($"{flipY.Id}:{flipY.label}\n{string.Join("\n", flipY.imageData)}");
            */


            var fullTileMapString = "";
            var tileSize = tiles[0].imageData.Count();
            for (int xx = 0; xx < arrangedTiles.GetLength(0) * tileSize; xx++)
            {
                for (int yy = 0; yy < arrangedTiles.GetLength(1) * tileSize; yy++)
                {
                    //Totally not confusing at all
                    var x = yy / tileSize;
                    var y = xx / tileSize;
                    var tile = arrangedTiles[x, y];

                    var yMod = xx % tileSize;
                    var xMod = yy % tileSize;
                    if (yMod == 0 || yMod == tileSize - 1)
                        fullTileMapString += " ";
                    else if (xMod == 0 || xMod == tileSize - 1)
                        fullTileMapString += " ";
                    else
                        fullTileMapString += tile.imageData[yMod][xMod];

                }
                fullTileMapString += "\n";
            }
            Console.WriteLine($"{fullTileMapString}");

            var fullTileMapArray = fullTileMapString.Split('\n');
            var compressedTileMapString = "";
            for (int xx = 0; xx < arrangedTiles.GetLength(0) * tileSize; xx++)
            {
                for (int yy = 0; yy < arrangedTiles.GetLength(1) * tileSize; yy++)
                {
                    if (fullTileMapArray[xx][yy] == ' ')
                        continue;
                    Console.Write($"{fullTileMapArray[xx][yy]}");
                    compressedTileMapString += fullTileMapArray[xx][yy];
                }
                if (string.IsNullOrEmpty(fullTileMapArray[xx].Trim()))
                    continue;
                Console.WriteLine($"");
                compressedTileMapString += "\n";
            }
            Console.WriteLine("---");
            Console.WriteLine(compressedTileMapString);
            Console.WriteLine("---");

            string[] seaMonster = new string[]
            {
                "                  # ",
                "#    ##    ##    ###",
                " #  #  #  #  #  #   ",
            };

            var compressedTileMapArray = compressedTileMapString.Trim().Split('\n').ToArray();
            int numSeaMonsters = FindNumSeaMonsters(seaMonster, ref compressedTileMapArray);
            Console.WriteLine("Sea Monsters Found: " + numSeaMonsters);
            Console.WriteLine("Sea Monsters Found: \n" + string.Join("\n", compressedTileMapArray));

            var newMapTile = new Tile(0, "", "", "", "", "", compressedTileMapArray.ToList());
            var tilePermutations = newMapTile.GetTilePermutations();
            foreach (var tile in tilePermutations)
            {
                var newArray = tile.imageData.ToArray();
                numSeaMonsters = FindNumSeaMonsters(seaMonster, ref newArray);
                Console.WriteLine($"Sea Monsters Found {tile.label}: " + numSeaMonsters);
                if(numSeaMonsters > 0)
                {
                    //Console.WriteLine("Sea Monsters: \n" + string.Join("\n", newArray));
                    
                }
                
            }


        }

        private static int FindNumSeaMonsters(string[] seaMonster, ref string[] compressedTileMapArray)
        {
            var newTileMapArray = new string[compressedTileMapArray.Length];
            for (int ii = 0; ii < newTileMapArray.Length; ii++)
            {
                newTileMapArray[ii] = compressedTileMapArray[ii];
            }
            var numSeaMonsters = 0;
            for (int xx = 0; xx < compressedTileMapArray[0].Length; xx++)
            {
                for (int yy = 0; yy < compressedTileMapArray.Length; yy++)
                {
                    var isSeaMonsterFound = true;
                    if (xx + seaMonster[0].Length >= compressedTileMapArray[0].Length)
                        continue;
                    if (yy + seaMonster.Length >= compressedTileMapArray.Length)
                        continue;
                    for (int y = 0; y < seaMonster.Length; y++)
                    {
                        if (!isSeaMonsterFound)
                            continue;
                        for (int x = 0; x < seaMonster[0].Length; x++)
                        {
                            var seaChar = seaMonster[y][x];
                            var mapTile = compressedTileMapArray[yy + y][xx + x];
                            if (seaChar == ' ')
                                continue; //Doesn't mattress
                            if (mapTile != '#')
                            {
                                isSeaMonsterFound = false;
                                continue;
                            }
                        }
                    }
                    if (isSeaMonsterFound)
                    {
                        numSeaMonsters++;

                        for (int y = 0; y < seaMonster.Length; y++)
                        {
                            for (int x = 0; x < seaMonster[0].Length; x++)
                            {
                                var seaChar = seaMonster[y][x];
                                var mapTile = compressedTileMapArray[yy + y][xx + x];
                                if (seaChar == ' ')
                                    continue; //Doesn't mattress
                                if (mapTile == '#')
                                {
                                    newTileMapArray[yy + y] = newTileMapArray[yy + y].Substring(0, xx + x) + "O" + newTileMapArray[yy + y].Substring(xx + x + 1);
                                }
                            }
                        }
                    }
                }
            }

            var numNonSeaMonsterHash = 0;
            for (int xx = 0; xx < compressedTileMapArray[0].Length; xx++)
            {
                for (int yy = 0; yy < compressedTileMapArray.Length; yy++)
                {
                    if (newTileMapArray[yy][xx] == '#')
                        numNonSeaMonsterHash++;
                }
            }
            Console.WriteLine($"Non Sea Monsters: {numNonSeaMonsterHash}");
            return numSeaMonsters;
        }

        private static Dictionary<int, int> TilesToMatchCount2(Dictionary<int, List<string>> tileIdToAllTilePermutations)
        {
            var tileIdToEdgeMatchCount = new Dictionary<int, int>();
            foreach (var kvp in tileIdToAllTilePermutations)
            {
                tileIdToEdgeMatchCount[kvp.Key] = 0;
                foreach (var testKvp in tileIdToAllTilePermutations)
                {
                    if(kvp.Key == testKvp.Key)
                    {
                        continue;
                    }
                    var tileEdges = kvp.Value;
                    var testTileEdges = testKvp.Value;

                    var intersect = tileEdges.Intersect(testTileEdges);
                    tileIdToEdgeMatchCount[kvp.Key] += intersect.Count();
                }

            }
            return tileIdToEdgeMatchCount;
        }





        private static Tile[,] ArrangeTiles(List<Tile> tiles, Dictionary<int, int> tilesToMatchCount)
        {
            var sideLength = (int)Math.Sqrt(tiles.Count);

            var arrangedTiles = new Tile[sideLength, sideLength];

            var allTilePossibilities = tiles.SelectMany(tile => tile.GetTilePermutations()).ToList();

            arrangedTiles = PlaceEdgeTiles(tiles, tilesToMatchCount, allTilePossibilities, arrangedTiles, new List<int>());

            var usedIds = new List<int>();
            foreach (var tile in arrangedTiles)
            {
                if (tile != null)
                    usedIds.Add(tile.Id);
            }

            return ArrangeTiles(tiles, allTilePossibilities, arrangedTiles, usedIds);
        }

        private static Tile[,] PlaceEdgeTiles(List<Tile> tiles, Dictionary<int, int> tilesToMatchCount, List<Tile> allTilePossibilities, Tile[,] arrangedTiles, List<int> usedIds)
        {
            var cornerTileIds = tilesToMatchCount.Where(kvp => kvp.Value == 4).Select(kvp => kvp.Key).Distinct();
            var sideTileIds = tilesToMatchCount.Where(kvp => kvp.Value == 6).Select(kvp => kvp.Key).Distinct();

            var allOtherTileIds = tilesToMatchCount.Where(kvp => kvp.Value > 6).Select(kvp => kvp.Key).Distinct();

            var edgeTestTileIds = cornerTileIds.Concat(sideTileIds);

            var edgeTileMap = PlaceEdgeTiles(
                tiles.Where(
                    tile => edgeTestTileIds.Contains(tile.Id)
                    ).ToList(),
                allTilePossibilities.Where(
                    tile => edgeTestTileIds.Contains(tile.Id)
                    ).ToList(),
                arrangedTiles, 
                usedIds);

            return edgeTileMap;
        }
        private static Tile[,] PlaceEdgeTiles(List<Tile> tiles, List<Tile> allTilePossibilities, Tile[,] currentMap, List<int> usedIds)
        {
            WriteLine($"PlaceEdgeTiles: UsedIds: [{usedIds.Count}] {string.Join(",", usedIds)}");
            if (usedIds.Count > maxUsedCount)
            {
                Console.WriteLine($"maxUsedCount went up to {maxUsedCount}.  Needs to get to {tiles.Count}");
                maxUsedCount = usedIds.Count;
            }
            if (usedIds.Count == tiles.Count)
            {
                return currentMap;
            }
            var map = CopyMap(currentMap);

            //ConsoleWriteMap(map);

            FindFirstOpenEdge(map, out var x, out var y);
            if (x < 0 && y < 0)
                return currentMap;

            foreach (var tile in allTilePossibilities)
            {
                if (usedIds.Contains(tile.Id))
                {
                    continue;
                }
                WriteLine($"Testing {tile}");

                if (DoesTileFit(map, tile, x, y))
                {
                    WriteLine($"Tile Fits! {tile}");
                    //This fits...
                    map[x, y] = tile;
                    var newUsedIds = usedIds.Select(id => id).ToList();
                    newUsedIds.Add(tile.Id);
                    var fullMap = PlaceEdgeTiles(tiles, allTilePossibilities, map, newUsedIds);
                    if (fullMap != null)
                        return fullMap;
                }
            }
            WriteLine($"--- Nothing Fit ---");
            return null;

        }
        private static void FindFirstOpenEdge(Tile[,] arrangedTiles, out int x, out int y)
        {
            for (int yy = 0; yy < arrangedTiles.GetLength(1); yy++)
            {
                for (int xx = 0; xx < arrangedTiles.GetLength(0); xx++)
                {
                    //Only edges!
                    if(xx > 0 && xx < arrangedTiles.GetLength(0) - 1)
                    {
                        //Not an edge!  Unless...
                        if (yy != 0 && yy != arrangedTiles.GetLength(1) - 1)
                        {
                            continue;
                        }
                    }
                    //Only edges!
                    if (yy > 0 && yy < arrangedTiles.GetLength(1) - 1)
                    {
                        //Not an edge!  Unless...
                        if (xx != 0 && xx != arrangedTiles.GetLength(0) - 1)
                        {
                            continue;
                        }
                    }

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

        private static Dictionary<Tile, int> TilesToMatchCount(List<Tile> tiles, List<Tile> allTilePossibilities, Tile[,] currentMap, List<int> usedIds)
        {
            var tilesToEdgeMatchCount = new Dictionary<Tile, int>();
            foreach (var tile in allTilePossibilities)
            {
                tilesToEdgeMatchCount[tile] = 0;
                foreach (var testTile in allTilePossibilities)
                {
                    if (tile.Id == testTile.Id) //No self-match
                        continue;
                    //Test Tile Is LEFT of tile:
                    if (testTile.RightNumber == Tile.FlipString(tile.LeftNumber))
                    {
                        tilesToEdgeMatchCount[tile]++;
                    }
                    //Test Tile Is RIGHT of tile:
                    if (testTile.LeftNumber == Tile.FlipString(tile.RightNumber))
                    {
                        tilesToEdgeMatchCount[tile]++;
                    }
                    //Test Tile Is TOP of tile:
                    if (testTile.BottomNumber == Tile.FlipString(tile.TopNumber))
                    {
                        tilesToEdgeMatchCount[tile]++;
                    }
                    //Test Tile Is BOTTOM of tile:
                    if (testTile.TopNumber == Tile.FlipString(tile.BottomNumber))
                    {
                        tilesToEdgeMatchCount[tile]++;
                    }
                }
            }
            return tilesToEdgeMatchCount;
        }



        public static void WriteLine(string str)
        {
            //Console.WriteLine(str);
        }

        private static int maxUsedCount = 0;
        private static Tile[,] ArrangeTiles(List<Tile> tiles, List<Tile> allTilePossibilities, Tile[,] currentMap, List<int> usedIds)
        {
            WriteLine($"ArrangeTiles: UsedIds: [{usedIds.Count}] {string.Join(",",usedIds)}");
            if(usedIds.Count > maxUsedCount)
            {
                Console.WriteLine($"maxUsedCount went up to {maxUsedCount}.  Needs to get to {tiles.Count}");
                maxUsedCount = usedIds.Count;
            }
            if(usedIds.Count == tiles.Count)
            {
                return currentMap;
            }
            var map = CopyMap(currentMap);

            //ConsoleWriteMap(map);

            FindFirstOpenSpot(map, out var x, out var y);
            if (x < 0 && y < 0)
                return currentMap;


            foreach (var tile in allTilePossibilities)
            {
                if(usedIds.Contains(tile.Id))
                {
                    continue;
                }
                WriteLine($"Testing {tile}");
/*
                if (map[0, 0]?.Id == 1951 && map[1, 0]?.Id == 2311 && tile.Id == 3079)
                {
                    Console.WriteLine("1951");
                }

                if (map[0, 0]?.Id == 1951 && map[1, 0]?.Id == 2311 && tile.Id == 3079)
                {
                    Console.WriteLine($"{map[1, 0].RightNumber}\n{tile.LeftNumber}");
                    Console.WriteLine($"{map[1, 0].RightNumber == tile.LeftNumber}");

                }
*/

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
            WriteLine($"--- Nothing Fit ---");
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
                if(testTile.RightNumber != Tile.FlipString( tile.LeftNumber ))
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
                if (testTile.LeftNumber != Tile.FlipString(tile.RightNumber))
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
                if (testTile.BottomNumber != Tile.FlipString(tile.TopNumber))
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
                if (testTile.TopNumber != Tile.FlipString(tile.BottomNumber))
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

            public string TopNumber;
            public string RightNumber;
            public string BottomNumber;
            public string LeftNumber;

            public string label;

            public Tile(List<string> inputStrings)
            {
                //"Tile 3079:",
                Id = int.Parse(inputStrings[0].Split(' ')[1].TrimEnd(':'));

                imageData = inputStrings.Skip(1).ToList();

                //Make sure we're reading in a Clockwise way:
                TopNumber = (imageData[0]);
                RightNumber = (string.Join("", imageData.Select(str => str[str.Length - 1])));
                BottomNumber = (string.Join("", imageData[imageData.Count - 1].Reverse()));
                LeftNumber = (string.Join("", imageData.Select(str => str[0]).Reverse()) );
                label = " ";
            }
            public Tile(int id, string topNumber, string rightNumber, string bottomNumber, string leftNumber, string label, List<string> imageData)
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
                var image2dArray = new char[imageData.Count, imageData.Count];
                for (int xx = 0; xx < imageData.Count; xx++)
                {
                    for (int yy = 0; yy < imageData.Count; yy++)
                    {
                        image2dArray[xx,yy] = imageData[yy][xx];
                    }
                }
                var newImageArray = new char[imageData.Count, imageData.Count];
                var width = image2dArray.GetLength(0);
                var height = image2dArray.GetLength(1);

                for (int row = 0; row < image2dArray.GetLength(0); row++)
                {
                    for (int col = 0; col < image2dArray.GetLength(1); col++)
                    {
                        int newRow = col;
                        int newCol = height - (row + 1);

                        newImageArray[newCol, newRow] = image2dArray[col, row];
                    }
                }

                var rotatedImageData = new List<string>();
                for (int yy = 0; yy < imageData.Count; yy++) 
                {
                    var xRowString = "";
                    for (int xx = 0; xx < imageData.Count; xx++)
                    {
                        xRowString += newImageArray[xx, yy];
                    }
                    rotatedImageData.Add(xRowString);
                }

                return new Tile(Id, LeftNumber, TopNumber, RightNumber, BottomNumber, label+"R", rotatedImageData);
            }

            public Tile FlipX()
            {
                var flippedImageData = imageData.Select(str => string.Join("", str.Reverse())).ToList();
                return new Tile(Id, FlipString(TopNumber), FlipString(LeftNumber), FlipString(BottomNumber), FlipString(RightNumber), label + "FlX", flippedImageData);
            }

            public Tile FlipY()
            {
                var flippedImageData = imageData.Select(str => str).ToList();
                flippedImageData.Reverse();
                return new Tile(Id, FlipString(BottomNumber), FlipString(RightNumber), FlipString(TopNumber), FlipString(LeftNumber), label + "FlY", flippedImageData);
            }

            public static string FlipString(string number)
            {
                return string.Join("", number.Reverse());
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
                    //FlipX().FlipY(), //This is probably the same as something else... ah, right, double rotation, eh, leave it in
                    RotateClockwise(),
                    RotateClockwise().RotateClockwise(), 
                    RotateClockwise().RotateClockwise().RotateClockwise(),
                    FlipX().RotateClockwise(),
                    FlipX().RotateClockwise().RotateClockwise(),
                    FlipX().RotateClockwise().RotateClockwise().RotateClockwise(),
                    FlipY().RotateClockwise(),
                    FlipY().RotateClockwise().RotateClockwise(),
                    FlipY().RotateClockwise().RotateClockwise().RotateClockwise(),
                };
            }

            public List<string> GetAllSidesPermutations()
            {
                return new List<string>()
                {
                    this.TopNumber,
                    this.RightNumber,
                    this.BottomNumber,
                    this.LeftNumber,

                    FlipString(this.TopNumber      ),
                    FlipString(this.RightNumber    ),
                    FlipString(this.BottomNumber   ),
                    FlipString(this.LeftNumber     ),
                };
            }
        }

    }
}