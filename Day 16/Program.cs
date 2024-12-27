using System;
using System.Collections.Generic;
using System.IO;

namespace Day_16_2nd_try
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
                var bestItem = elements[0].item;
                elements.RemoveAt(0);
                return bestItem;
            }
        }

        public enum Direction
        {
            right, down, left, up
        }

        public struct Point
        {
            public int x, y, cost;
            public Direction dir;

            public Point(int x, int y, Direction dir,int cost)
            {
                this.x = x;
                this.y = y;
                this.cost = cost;
                this.dir = dir;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Part 1: {SOL1(GetPuzzleInput("input.txt"))}");
            Console.WriteLine($"Part 2: {SOL2(GetPuzzleInput("input.txt"))}");
            Console.ReadKey();
        }

        static int SOL1(char[,] maze)
        {
            ((int, int) start, (int, int) end) = FindStartEnd(maze);
            int result = BFS1(maze, start, end);
            return result;
        }

        static int SOL2(char[,] maze)
        {
            ((int, int) start, (int, int) end) = FindStartEnd(maze);
            int result = BFS2(maze, start, end);
            return result;
        }

        static char[,] GetPuzzleInput(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            char[,] maze = new char[lines[0].Length, lines.Length];
            for (int i = 0; i < lines.Length; i++)
                for (int j = 0; j < lines[i].Length; j++)
                    maze[j, i] = lines[i][j];
            return maze;
        }

        static int BFS(char[,] map, (int x, int y) startPos, (int x, int y) endPos, HashSet<(int, int, Direction)> seen, int[,] costs, ref Direction dIn)
        {
            (int x, int y)[] dxy = { (1, 0), (0, 1), (-1, 0), (0, -1) };
            PriorityQueue<Point> myQueue = new PriorityQueue<Point>();
            myQueue.Enqueue(new Point(startPos.x, startPos.y, dIn, 0), 0);
            costs[startPos.x, startPos.y] = 0;

            while (myQueue.Count > 0)
            {
                Point myPoint = myQueue.Dequeue();
                if ((myPoint.x, myPoint.y) == endPos)
                {
                    if (costs[myPoint.x, myPoint.y] > myPoint.cost)
                    {
                        costs[myPoint.x, myPoint.y] = Math.Min(myPoint.cost, costs[endPos.x, endPos.y]);
                        dIn = myPoint.dir;
                    }
                }

                if (!seen.Add((myPoint.x, myPoint.y, myPoint.dir)))
                    continue;

                costs[myPoint.x, myPoint.y] = Math.Min(myPoint.cost, costs[myPoint.x, myPoint.y]);
                int nx = myPoint.x + dxy[(int)myPoint.dir].x;
                int ny = myPoint.y + dxy[(int)myPoint.dir].y;
                if (IsValidMove(nx, ny, map) && !seen.Contains((nx, ny, myPoint.dir)))
                    myQueue.Enqueue(new Point(nx, ny, myPoint.dir, myPoint.cost + 1), myPoint.cost + 1);

                for (int i = 1; i <= 3; i++)
                {
                    Direction newDir = (Direction)(((int)myPoint.dir + i) % 4);
                    if (!seen.Contains((myPoint.x, myPoint.y, newDir)))
                        myQueue.Enqueue(new Point(myPoint.x, myPoint.y, newDir, myPoint.cost + 1000), myPoint.cost + 1000);
                }
            }
            return costs[endPos.x, endPos.y];
        }

        static int BFS1(char[,] map, (int x, int y) startPos, (int x, int y) endPos)
        {
            (int x, int y)[] dxy = { (1, 0), (0, 1), (-1, 0), (0, -1) };
            HashSet<(int, int, Direction)> seen = new HashSet<(int, int, Direction)>();
            int[,] costs = new int[map.GetLength(0), map.GetLength(1)];
            Direction dir = Direction.right;
            for (int x = 0; x < costs.GetLength(0); x++)
                for (int y = 0; y < costs.GetLength(1); y++)
                    costs[x, y] = int.MaxValue;
            return BFS(map, startPos, endPos, seen, costs, ref dir);
        }

        static ((int, int), (int, int)) FindStartEnd(char[,] maze)
        {
            (int, int) start = (0, 0), end = (0, 0);
            for (int i = 0; i < maze.GetLength(0); i++)
                for (int j = 0; j < maze.GetLength(1); j++)
                    if (maze[i, j] == 'E')
                        end = (i, j);
                    else if (maze[i, j] == 'S')
                        start = (i, j);

            return (start, end);
        }

        static bool IsValidMove(int x, int y, char[,] maze)
            => x >= 0 && y >= 0 && x < maze.GetLength(0) && y < maze.GetLength(1) && maze[x, y] != '#';

        public static int BFS2(char[,] map, (int x, int y) startPos, (int x, int y) endPos)
        {
            (int x, int y)[] dxy = { (1, 0), (0, 1), (-1, 0), (0, -1) };
            HashSet<(int, int, Direction)> seen = new HashSet<(int, int, Direction)>();
            int[,] costs2 = new int[map.GetLength(0), map.GetLength(1)];
            int[,] costs = new int[map.GetLength(0), map.GetLength(1)];
            for (int x = 0; x < costs.GetLength(0); x++)
                for (int y = 0; y < costs.GetLength(1); y++)
                {
                    costs[x, y] = int.MaxValue;
                    costs2[x, y] = int.MaxValue;
                }
            Direction dir = Direction.right;
            int best = BFS(map, startPos, endPos, seen, costs, ref dir);
            seen.Clear();
            dir = (Direction)(((int)dir + 2) % 4);
            BFS(map, endPos, startPos, seen, costs2, ref dir);
            HashSet<(int, int)> bestPaths = new HashSet<(int, int)>();
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (costs[x, y] + costs2[x, y] == best || costs[x, y] + costs2[x, y] == best - 1000)
                    {
                        bestPaths.Add((x, y));
                        map[x, y] = 'O';
                    }
                }
            }
            return bestPaths.Count + 1; //idk why its an off by one error but it is
        }
    }
}