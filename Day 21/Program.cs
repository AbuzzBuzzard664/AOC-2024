
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Day_21
{
    internal class Program
    {

        static string[] keypadNumeric = { "789", "456", "123", "X0A" };
        static string[] keypadDirectional = { "X^A", "<v>" };
        static Dictionary<string, long> cache = new Dictionary<string, long>();

        static void Main(string[] args)
        {
            Console.WriteLine($"part 1: {SOL1()}");
            Console.WriteLine($"part 2: {SOL2()}");
            Console.ReadKey();
        }

        static long SOL1()
        {
            string[] data = File.ReadAllLines("input.txt");
            long ans = 0;
            foreach (string code in data)
                ans += Solve(code, 2) * int.Parse(code.Substring(0, 3));

            return ans;
        }

        static long SOL2()
        {
            string[] data = File.ReadAllLines("input.txt");
            long ans = 0;
            foreach (string code in data)
                ans += Solve(code, 25) * int.Parse(code.Substring(0, 3));

            return ans;
        }

        static (int, int) FindPosition(char key, string[] keypad)
        {
            for (int y = 0; y < keypad.Length; y++)
                for (int x = 0; x < keypad[y].Length; x++)
                    if (keypad[y][x] == key)
                        return (y, x);

            return (-1, -1);
        }

        static List<string> GetPathsBetweenKeys(char key1, char key2, string[] keypad)
        {
            (int y1, int x1) = FindPosition(key1, keypad);
            (int y2, int x2) = FindPosition(key2, keypad);
            (int, int) gap = FindPosition('X', keypad);
            int dy = y2 - y1;
            int dx = x2 - x1;

            string yMoves = (dy >= 0) ? new string('v', Math.Abs(dy)) : new string('^', Math.Abs(dy));
            string xMoves = (dx >= 0) ? new string('>', Math.Abs(dx)) : new string('<', Math.Abs(dx));

            if (dy == 0 && dx == 0)
                return new List<string>() { "" };

            else if (dy == 0)
                return new List<string>() { xMoves };

            else if (dx == 0)
                return new List<string>() { yMoves };

            else if ((y1, x2) == gap)
                return new List<string>() { yMoves + xMoves };

            else if ((y2, x1) == gap)
                return new List<string>() { xMoves + yMoves };

            return new List<string>() { yMoves + xMoves, xMoves + yMoves };

        }

        static List<List<string>> GetSequenceOfPaths(string sequence, string[] keypad)
        {
            List<List<string>> shortestPaths = new List<List<string>>();
            string extendedSequence = "A" + sequence;
            for (int i = 0; i < sequence.Length; i++)
            {
                char key1 = extendedSequence[i];
                char key2 = sequence[i];
                List<string> paths = GetPathsBetweenKeys(key1, key2, keypad);
                for (int j = 0; j < paths.Count; j++)
                    paths[j] += 'A';

                shortestPaths.Add(paths);
            }
            return shortestPaths;
        }

        static long Solve(string sequence, int depth)
        {
            if (depth < 0)
                return sequence.Length;

            string key = sequence + depth;
            if (cache.ContainsKey(key))
                return cache[key];


            string[] keypad;
            if (Regex.IsMatch(sequence, @"\d"))
                keypad = keypadNumeric;

            else
                keypad = keypadDirectional;

            long result = 0;
            List<List<string>> shortestPaths = GetSequenceOfPaths(sequence, keypad);
            foreach (List<string> paths in shortestPaths)
            {
                long minPath = long.MaxValue;
                foreach (string path in paths)
                {
                    long tempPath = Solve(path, depth - 1);
                    if (tempPath < minPath)
                        minPath = tempPath;

                }
                result += minPath;
            }
            cache[key] = result;
            return result;
        }
    }
}
