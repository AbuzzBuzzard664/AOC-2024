using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Day_18
{
    internal class Program
    {
        public struct Point 
        {
            public int x, y, cost;

            public Point(int x, int y, int cost)
            {
                this.x = x;
                this.y = y;
                this.cost = cost;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Part 1: {SOL1(GetPuzzleInput("input.txt", 1024))}");
            Console.WriteLine($"Part 2: {SOL2(GetPuzzleInput("input.txt", 3450))}");
            Console.ReadKey();
        }

        static (int, int)[] GetPuzzleInput(string filename, int times)
        {
            string[] lines = File.ReadAllLines(filename);
            (int, int)[] points = new (int, int)[lines.Length];
            for (int i = 0; i < times; i++)
            {
                string[] split = lines[i].Split(',');
                (int, int) temp = (int.Parse(split[0]), int.Parse(split[1]));
                points[i] = temp;
            }
            return points;
        }

        static int SOL1((int, int)[] points)
        {
            char[,] map = new char[71, 71];
            PopulateMap(map, points);
            int result = BFS(map);
            return result;
        }

        static string SOL2((int, int)[] points)
        {
            List<(int, int)> pointsToUse = new List<(int, int)>();
            for (int i = 0; i < 1024; i++)
            {
                pointsToUse.Add(points[i]);
            }
            for (int i = 1024; i < points.Length; i++)
            {
                char[,] map = new char[71, 71];
                pointsToUse.Add(points[i]);
                PopulateMap(map, pointsToUse.ToArray());
                int result = BFS(map);
                if (result == int.MaxValue) return points[i].Item1 + "," + points[i].Item2;
            }
            return null;
        }

        static void PopulateMap(char[,] map, (int, int)[] points)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (points.Contains((x, y)))
                        map[x, y] = '#';
                    
                    else
                        map[x, y] = '.';
                    
                }
            }
        }

        static int BFS(char[,] map)
        {
            (int x, int y)[] dxy = { (-1, 0), (1, 0), (0, 1), (0, -1) };
            Point startPos = new Point(0, 0, 0);
            Point endPos = new Point(70, 70, int.MaxValue);
            bool[,] visited = new bool[71, 71];
            Queue<Point> myQueue = new Queue<Point>();
            myQueue.Enqueue(startPos);
            while (myQueue.Count > 0)
            {
                Point p = myQueue.Dequeue();
                if ((p.x, p.y) == (endPos.x,endPos.y)) return p.cost;
                if (!visited[p.x, p.y])
                {
                    visited[p.x, p.y] = true;
                    for (int i = 0; i < 4; i++)
                    {
                        int nx = p.x + dxy[i].x;
                        int ny = p.y + dxy[i].y;
                        if (IsValid(map, nx, ny))
                            myQueue.Enqueue(new Point(nx, ny, p.cost + 1));
                    }
                }
            }
            return int.MaxValue;
        }

        static bool IsValid(char[,] map, int x, int y)
            => x >= 0 && y >= 0 && x < map.GetLength(0) && y < map.GetLength(1) && map[x, y] != '#';
        
    }
}