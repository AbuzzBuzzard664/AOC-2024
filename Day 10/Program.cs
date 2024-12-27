using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_10
{
    internal class Program
    {
        public struct Tile
        {
            public int x, y;
            public bool up, down, left, right;
            public int height;
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Part 1: {SOL1(GetMap())}");
            Console.WriteLine($"Part 2: {SOL2(GetMap())}");
            Console.ReadKey();
        }

        static Tile[,] GetMap()
        {
            string[] lines = File.ReadAllLines("input.txt");
            Tile[,] map = new Tile[lines.Length, lines[0].Length];
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[0].Length; j++)
                {
                    Tile temptile;
                    temptile.x = j;
                    temptile.y = i;
                    temptile.left = false;
                    if (j != 0) temptile.left = true;
                    temptile.right = false;
                    if (j != map.GetLength(0) - 1) temptile.right = true;
                    temptile.down = false;
                    if (i != 0) temptile.down = true;
                    temptile.up = false;
                    if (i != map.GetLength(1) - 1) temptile.up = true;
                    temptile.height = int.Parse(lines[i][j].ToString());
                    map[temptile.x, temptile.y] = temptile;
                }
            }
            return map;
        }

        static int SOL1(Tile[,] map)
        {
            int answer = 0;
            HashSet<Tile> nines = new HashSet<Tile>();
            foreach (Tile myTile in map)
            {
                if (myTile.height == 0)
                {
                    CheckAround(map, myTile, nines);
                    answer += nines.Count;
                    nines.Clear();
                }
            }
            return answer;
        }

        static void CheckAround(Tile[,] map, Tile startTile, ICollection<Tile> nines)
        {
            if (startTile.height == 9)
            {
                nines.Add(startTile);
            }
            else
            {
                if (startTile.down)
                {
                    if (startTile.height + 1 == map[startTile.x, startTile.y - 1].height)
                    {
                        Tile temptile = map[startTile.x, startTile.y - 1];
                        CheckAround(map, temptile, nines);
                    }
                }
                if (startTile.up)
                {
                    if (startTile.height + 1 == map[startTile.x, startTile.y + 1].height)
                    {
                        Tile temptile = map[startTile.x, startTile.y + 1];
                        CheckAround(map, temptile, nines);
                    }
                }
                if (startTile.right)
                {
                    if (startTile.height + 1 == map[startTile.x + 1, startTile.y].height)
                    {
                        Tile temptile = map[startTile.x + 1, startTile.y];
                        CheckAround(map, temptile, nines);
                    }
                }
                if (startTile.left)
                {
                    if (startTile.height + 1 == map[startTile.x - 1, startTile.y].height)
                    {
                        Tile temptile = map[startTile.x - 1, startTile.y];
                        CheckAround(map, temptile, nines);
                    }
                }
            }
        }

        static int SOL2(Tile[,] map)
        {
            List<Tile> nines = new List<Tile>();
            foreach (Tile myTile in map)
            {
                if (myTile.height == 0)
                {
                    CheckAround(map, myTile, nines);
                }
            }
            return nines.Count;
        }
    }
}
