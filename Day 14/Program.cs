
using System;
using System.Collections.Generic;
using System.IO;

namespace Day_14
{
    internal class Program
    {
        public struct Robot
        {
            public int px, py, vx, vy;
        }

        const int maxx = 101, maxy = 103;

        static void Main(string[] args)
        {
            Console.WriteLine($"Part 1: {SOL1(GetPuzzleInput())}");
            Console.WriteLine($"Part 2: {SOL2(GetPuzzleInput())}");
            Console.ReadKey();
        }

        static Robot[] GetPuzzleInput()
        {
            string[] lines = File.ReadAllLines("input.txt");
            List<Robot> robots = new List<Robot>();
            foreach (string line in lines)
            {
                string[] split = line.Split(' ');
                Robot rtemp;
                string[] parts = split[0].Substring(2).Split(',');
                rtemp.px = int.Parse(parts[0]);
                rtemp.py = int.Parse(parts[1]);
                parts = split[1].Substring(2).Split(',');
                rtemp.vx = int.Parse(parts[0]);
                rtemp.vy = int.Parse(parts[1]);
                robots.Add(rtemp);
            }
            return robots.ToArray();
        }

        static void MoveRobots(Robot[] robots)
        {
            for (int j = 0; j < robots.Length; j++)
            {
                robots[j].px += robots[j].vx;
                robots[j].py += robots[j].vy;
                if (robots[j].px < 0) robots[j].px += maxx;
                if (robots[j].py < 0) robots[j].py += maxy;
                if (robots[j].px >= maxx) robots[j].px -= maxx;
                if (robots[j].py >= maxy) robots[j].py -= maxy;
            }
        }

        static long SOL1(Robot[] robots)
        {
            for (int i = 0; i < 100; i++)
            {
                MoveRobots(robots);
            }
            long tleft = 0, tright = 0, bleft = 0, bright = 0;
            foreach (Robot r in robots)
            {
                if (r.px < maxx / 2)
                {
                    if (r.py < maxy / 2) tleft++;
                    else if (r.py > maxy / 2) bleft++;
                }
                else if (r.px > maxx / 2)
                {
                    if (r.py < maxy / 2) tright++;
                    else if (r.py > maxy / 2) bright++;
                }
            }
            return tleft * tright * bright * bleft;
        }

        static long SOL2(Robot[] robots)
        {
            bool distinct = false;
            int time = 0;
            do
            {
                MoveRobots(robots);
                distinct = false;
                int[,] map = new int[maxx, maxy];
                foreach (Robot r in robots)
                {
                    map[r.px, r.py]++;
                    if (map[r.px, r.py] > 1) distinct = true;
                }
                time++;
            } while (distinct);
            return time;

        }
    }
}

