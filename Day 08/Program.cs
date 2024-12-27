using System;
using System.Collections.Generic;
using System.IO;

namespace Day_08
{
    internal class Program
    {
        static void Main(string[] args)
        {
            char[,] map = GetPuzzleInput();
            int answer = SOL1(map);
            Console.WriteLine($"Part 1: {answer}");
            answer = SOL2(map);
            Console.WriteLine($"Part 2: {answer}");
            Console.ReadKey();
        }

        static char[,] GetPuzzleInput()
        {
            string[] lines = File.ReadAllLines("input.txt");
            char[,] map = new char[lines.Length, lines[0].Length];
            int i = 0;
            foreach (string line in lines)
            {
                foreach (char c in line)
                {
                    map[i / map.GetLength(0), i % map.GetLength(1)] = c;
                    i++;
                }
            }
            return map;
        }

        static Dictionary<char, List<(int x, int y)>> PopulateDict(char[,] map)
        {
            Dictionary<char, List<(int x, int y)>> nodes = new Dictionary<char, List<(int, int)>>();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] != '.')
                    {
                        if (nodes.ContainsKey(map[i, j]))
                            nodes[map[i, j]].Add((i, j));

                        else
                        {
                            nodes.Add(map[i, j], new List<(int, int)>());
                            nodes[map[i, j]].Add((i, j));
                        }
                    }
                }
            }
            return nodes;
        }

        static int SOL1(char[,] map)
        {
            Dictionary<char, List<(int x, int y)>> nodes = PopulateDict(map);
            HashSet<(int, int)> antinode = new HashSet<(int, int)>();
            (int x, int y) = (0, 0);
            foreach (char c in nodes.Keys)
            {
                for (int i = 0; i < nodes[c].Count; i++)
                {
                    (int x, int y) temp = nodes[c][i];
                    for (int j = 0; j < nodes[c].Count; j++)
                    {
                        if (temp != nodes[c][j])
                        {
                            y = temp.y - nodes[c][j].y;
                            x = temp.x - nodes[c][j].x;

                            if (temp.y + y < map.GetLength(0) && temp.y + y > -1 && temp.x + x < map.GetLength(1) && temp.x + x > -1)
                                antinode.Add((temp.y + y, temp.x + x));
                            
                            if (nodes[c][j].y - y < map.GetLength(0) && nodes[c][j].y - y > -1 && nodes[c][j].x - x < map.GetLength(1) && nodes[c][j].x - x > -1)
                                antinode.Add((nodes[c][j].y - y, nodes[c][j].x - x));  
                        }
                    }
                }
            }
            return antinode.Count;
        }

        static int SOL2(char[,] map)
        {
            Dictionary<char, List<(int x, int y)>> nodes = PopulateDict(map);
            HashSet<(int, int)> antinode = new HashSet<(int, int)>();
            foreach (char c in nodes.Keys)
            {
                for (int i = 0; i < nodes[c].Count; i++)
                {
                    (int x, int y) temp = nodes[c][i];
                    for (int j = 0; j < nodes[c].Count; j++)
                    {
                        if (temp != nodes[c][j])
                        {
                            int k = 0;
                            int y = temp.y - nodes[c][j].y;
                            int x = temp.x - nodes[c][j].x;
                            while (temp.y + y * k < map.GetLength(0) && temp.y + y * k > -1 && temp.x + x * k < map.GetLength(1) && temp.x + x * k > -1)
                            {
                                antinode.Add((temp.y + y * k, temp.x + x * k));
                                k++;
                            }
                            k = 0;
                            while (nodes[c][j].y - y * k < map.GetLength(0) && nodes[c][j].y - y * k > -1 && nodes[c][j].x - x * k < map.GetLength(1) && nodes[c][j].x - x * k > -1)
                            {
                                antinode.Add((nodes[c][j].y - y * k, nodes[c][j].x - x * k));
                                k++;

                            }
                        }
                    }
                }
            }
            return antinode.Count;
        }
    }
}
