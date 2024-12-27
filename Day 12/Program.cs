﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_12
{
    internal class Program
    {
        public struct point
        {
            public int x, y;
            public point(int inx, int iny)
            {
                x = inx;
                y = iny;
            }
        }

        public struct region
        {
            public long perimeter, area, side;
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Part 1: {SOL1(Get2DArray())}");
            Console.WriteLine($"Part 2: {SOL2(Get2DArray())}");
            Console.ReadKey();
        }

        static char[,] Get2DArray()
        {
            string[] lines = File.ReadAllLines("input.txt");
            char[,] garden = new char[lines.Length, lines[0].Length];
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    garden[x, y] = lines[y][x];
                }
            }
            return garden;
        }

        static long SOL1(char[,] garden)
        {
            long answer = 0;
            bool[,] visited = new bool[garden.GetLength(0), garden.GetLength(1)];
            List<List<point>> plots = new List<List<point>>();
            for (int y = 0; y < garden.GetLength(0); y++)
            {
                for (int x = 0; x < garden.GetLength(1); x++)
                {
                    if (!visited[x, y])
                    {
                        List<point> temp = FloodFill(new point(x, y), garden[x, y], garden, visited);
                        plots.Add(temp);
                    }
                }
            }
            List<region> regions = new List<region>();
            foreach (List<point> plot in plots)
            {
                regions.Add(GetPerimeter(plot, garden));
            }
            foreach (region region in regions)
            {
                answer += region.perimeter * region.area;
            }
            return answer;
        }

        static region GetPerimeter(List<point> plot, char[,] garden)
        {
            region rtemp = new region();
            foreach (point p in plot)
            {
                char plantType = garden[p.x, p.y];
                if (p.x > 0)
                {
                    if (garden[p.x - 1, p.y] != plantType) rtemp.perimeter++;
                }
                else if (p.x == 0) rtemp.perimeter++;
                if (p.y > 0)
                {
                    if (garden[p.x, p.y - 1] != plantType) rtemp.perimeter++;
                }
                else if (p.y == 0) rtemp.perimeter++;
                if (p.x < garden.GetLength(0) - 1)
                {
                    if (garden[p.x + 1, p.y] != plantType) rtemp.perimeter++;
                }
                else if (p.x == garden.GetLength(0) - 1) rtemp.perimeter++;
                if (p.y < garden.GetLength(1) - 1)
                {
                    if (garden[p.x, p.y + 1] != plantType) rtemp.perimeter++;
                }
                else if (p.y == garden.GetLength(1) - 1) rtemp.perimeter++;
            }
            rtemp.area = plot.Count;
            return rtemp;
        }

        static List<point> FloodFill(point start, char plantType, char[,] garden, bool[,] visited)
        {
            bool allvisited = false;
            HashSet<point> pointsToSearch = new HashSet<point>() { start };
            while (!allvisited)
            {
                HashSet<point> temp = new HashSet<point>();
                foreach (point point in pointsToSearch)
                {
                    if (point.x > 0)
                    {
                        if (garden[point.x - 1, point.y] == plantType && !visited[point.x - 1, point.y])
                        {
                            temp.Add(new point(point.x - 1, point.y));
                            visited[point.x - 1, point.y] = true;
                        }
                    }
                    if (point.y > 0)
                    {
                        if (garden[point.x, point.y - 1] == plantType && !visited[point.x, point.y - 1])
                        {
                            temp.Add(new point(point.x, point.y - 1));
                            visited[point.x, point.y - 1] = true;
                        }
                    }
                    if (point.x < garden.GetLength(0) - 1)
                    {
                        if (garden[point.x + 1, point.y] == plantType && !visited[point.x + 1, point.y])
                        {
                            temp.Add(new point(point.x + 1, point.y));
                            visited[point.x + 1, point.y] = true;
                        }
                    }
                    if (point.y < garden.GetLength(1) - 1)
                    {
                        if (garden[point.x, point.y + 1] == plantType && !visited[point.x, point.y + 1])
                        {
                            temp.Add(new point(point.x, point.y + 1));
                            visited[point.x, point.y + 1] = true;
                        }
                    }
                }
                if (temp.Count == 0) allvisited = true;
                else
                {
                    foreach (point point in temp)
                    {
                        pointsToSearch.Add(point);
                    }
                }
            }
            return pointsToSearch.ToList();
        }

        static long SOL2(char[,] garden)
        {
            long answer = 0;
            bool[,] visited = new bool[garden.GetLength(0), garden.GetLength(1)];
            List<List<point>> plots = new List<List<point>>();
            for (int y = 0; y < garden.GetLength(0); y++)
            {
                for (int x = 0; x < garden.GetLength(1); x++)
                {
                    if (!visited[x, y])
                    {
                        List<point> temp = FloodFill(new point(x, y), garden[x, y], garden, visited);
                        plots.Add(temp);
                    }
                }
            }
            List<region> fences = new List<region>();
            foreach (List<point> plot in plots)
            {
                fences.Add(GetSides(plot, garden));
            }
            foreach (region region in fences)
            {
                answer += region.side * region.area;
            }
            return answer;
        }


        static region GetSides(List<point> plot, char[,] garden)
        {
            region rtemp = new region();
            rtemp.area = plot.Count;
            HashSet<(point, int)> edges = new HashSet<(point, int)>();
            foreach (point point in plot)
            {
                char plantType = garden[point.x, point.y];
                if (point.x > 0)
                {
                    if (garden[point.x - 1, point.y] != plantType) edges.Add((point, 0));
                }
                else edges.Add((point, 0));
                if (point.y > 0)
                {
                    if (garden[point.x, point.y - 1] != plantType) edges.Add((point, 1));
                }
                else edges.Add((point, 1));
                if (point.x < garden.GetLength(0) - 1)
                {
                    if (garden[point.x + 1, point.y] != plantType) edges.Add((point, 2));
                }
                else edges.Add((point, 2));
                if (point.y < garden.GetLength(1) - 1)
                {
                    if (garden[point.x, point.y + 1] != plantType) edges.Add((point, 3));
                }
                else edges.Add((point, 3));
            }
            while (edges.Count > 0)
            {
                rtemp.side++;
                var cur = edges.First();
                edges.Remove(cur);
                (point p, int d) temp = cur;
                while (edges.Contains((new point(temp.p.x + 1, temp.p.y), temp.d)))
                {
                    edges.Remove((new point(temp.p.x + 1, temp.p.y), temp.d));
                    temp.p.x++;
                }
                temp = cur;
                while (edges.Contains((new point(temp.p.x - 1, temp.p.y), temp.d)))
                {
                    edges.Remove((new point(temp.p.x - 1, temp.p.y), temp.d));
                    temp.p.x--;
                }
                temp = cur;
                while (edges.Contains((new point(temp.p.x, temp.p.y + 1), temp.d)))
                {
                    edges.Remove((new point(temp.p.x, temp.p.y + 1), temp.d));
                    temp.p.y++;
                }
                temp = cur;
                while (edges.Contains((new point(temp.p.x, temp.p.y - 1), temp.d)))
                {
                    edges.Remove((new point(temp.p.x, temp.p.y - 1), temp.d));
                    temp.p.y--;
                }
            }
            return rtemp;
        }
    }
}
