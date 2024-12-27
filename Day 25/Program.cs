using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Day_25
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Part 1: {SOL1()}");
            Console.ReadKey();
        }

        static (List<int[]>, List<int[]>) GetPuzzleInput()
        {
            string[] lines = File.ReadAllLines("input.txt");
            int i = 0;
            var locks = new List<int[]>();
            var keys = new List<int[]>();
            while (i < lines.Length)
            {
                int[] thing = new int[5];
                string s = lines[i];
                bool isKey = (s == ".....");
                while (i < lines.Length && lines[i] != "")
                {
                    for (int j = 0; j < lines[i].Length; j++)
                    {
                        if (lines[i][j] == '#') thing[j]++;
                    }
                    i++;
                }
                if (isKey) { keys.Add(thing); }
                else { locks.Add(thing); }
                i++;
            }
            return (locks, keys);
        }
        static long SOL1()
        {
            (List<int[]> locks, List<int[]> keys) = GetPuzzleInput();
            long answer = 0;
            foreach (int[] Lock in locks)
            {
                foreach (int[] Key in keys)
                {
                    bool fit = true;
                    for (int i = 0; i < Key.Length; i++)
                    {
                        if (Key[i] + Lock[i] > 7)
                        {
                            fit = false;
                            break;
                        }
                    }
                    answer += fit ? 1 : 0;
                }
            }
            return answer;
        }

        static long SOL2()
        {
            throw new NotImplementedException(":)");
        }
    }
}
