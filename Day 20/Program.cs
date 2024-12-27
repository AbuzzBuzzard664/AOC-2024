using System;
using System.Collections.Generic;
using System.IO;

namespace Day_20
{
    internal class Program
    {
        public class PriorityQueue<T>
        {
            private List<(T item, int priority)> elements = new List<(T item, int priority)>();

            public int Count => elements.Count;

            public void Enqueue(T item, int priority)
            {
                elements.Add((item, priority));
                elements.Sort((x, y) => x.priority.CompareTo(y.priority));
            }

            public T Dequeue()
            {
                T bestItem = elements[0].item;
                elements.RemoveAt(0);
                return bestItem;
            }
        }

        public struct Point
        {
            public int x, y;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public static bool operator ==(Point a, Point b)
            => a.x == b.x && a.y == b.y;

            public static bool operator !=(Point a, Point b)
                => !(a == b);

            public override bool Equals(object obj)
                => base.Equals(obj);


            public override int GetHashCode()
               => base.GetHashCode();

        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Part 1: {SOL1()}");
            Console.WriteLine($"Part 2: {SOL2()}");
            Console.ReadKey();
        }

        static char[,] GetPuzzleInput()
        {
            string[] lines = File.ReadAllLines("input.txt");
            char[,] map = new char[lines[0].Length, lines.Length];
            for (int y = 0; y < lines.Length; y++)
                for (int x = 0; x < lines.Length; x++)
                    map[x, y] = lines[y][x];
            return map;
        }

        static int SOL1()
        {
            char[,] map = GetPuzzleInput();
            (Point startPos, Point endPos) = FindStartEnd(map);
            int[,] costs = BFS(map, startPos, endPos);
            return Cheats(costs, map, 3);
        }

        static int SOL2()
        {
            char[,] map = GetPuzzleInput();
            (Point startPos, Point endPos) = FindStartEnd(map);
            int[,] costs = BFS(map, startPos, endPos);
            return Cheats(costs, map, 21);
        }

        static (Point, Point) FindStartEnd(char[,] maze)
        {
            Point start = new Point(0, 0), end = new Point(0, 0);
            for (int i = 0; i < maze.GetLength(0); i++)
                for (int j = 0; j < maze.GetLength(1); j++)
                    if (maze[i, j] == 'E')
                        end = new Point(i, j);
                    else if (maze[i, j] == 'S')
                        start = new Point(i, j);

            return (start, end);
        }

        static int[,] BFS(char[,] map, Point startPos, Point endPos)
        {
            (int x, int y)[] dxy = { (1, 0), (0, 1), (-1, 0), (0, -1) };
            int[,] costs = new int[map.GetLength(0), map.GetLength(1)];
            for (int x = 0; x < map.GetLength(0); x++)
                for (int y = 0; y < map.GetLength(1); y++)
                    costs[x, y] = int.MaxValue;
            HashSet<Point> visited = new HashSet<Point>();
            PriorityQueue<(Point, int)> myQueue = new PriorityQueue<(Point, int)>();
            costs = new int[map.GetLength(0), map.GetLength(1)];
            for (int x = 0; x < map.GetLength(0); x++)
                for (int y = 0; y < map.GetLength(1); y++)
                    costs[x, y] = int.MaxValue;
            myQueue.Enqueue((startPos, 0), 0);
            while (myQueue.Count > 0)
            {
                (Point p, int score) = myQueue.Dequeue();
                costs[p.x, p.y] = Math.Min(score, costs[p.x, p.y]);
                if (!visited.Add((p)))
                    continue;
                for (int i = 0; i < dxy.Length; i++)
                {
                    Point nextP = new Point(p.x + dxy[i].x, p.y + dxy[i].y);
                    if (IsValid(map, nextP) && !visited.Contains((nextP)))
                        myQueue.Enqueue((nextP, score + 1), score + 1);
                }
            }
            return costs;
        }

        static int Cheats(int[,] costs, char[,] map, int longestDist)
        {
            int answer = 0;
            for (int i = 1; i < map.GetLength(0) - 1; i++)
                for (int j = 1; j < map.GetLength(1) - 1; j++)
                    for (int di = 1; di < map.GetLength(0) - 1; di++)
                        for (int dj = 1; dj < map.GetLength(1) - 1; dj++)
                            if (IsValid(map, new Point(i, j)) && IsValid(map, new Point(di, dj)))
                            {
                                int manHat = Math.Abs(dj - j) + Math.Abs(di - i);
                                if (manHat < longestDist)
                                    if (costs[di, dj] - costs[i, j] - manHat >= 100)
                                        answer++;
                            }
            return answer;
        }

        static bool IsValid(char[,] map, Point p)
           => p.x >= 0 && p.y >= 0 && p.x < map.GetLength(0) && p.y < map.GetLength(1) && map[p.x, p.y] != '#';
    }
}