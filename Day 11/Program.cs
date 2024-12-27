using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Part 1: {SOL1(GetPuzzleInput())}");
            Console.WriteLine($"Part 2: {SOL2(GetPuzzleInput())}");
            Console.ReadKey();
        }

        static string GetPuzzleInput()
        {
            string line = File.ReadAllText("input.txt");
            return line;
        }

        static int SOL1(string input)
        {
            string[] strlist = input.Split(' ');
            List<long> ints = new List<long>();
            foreach (string str in strlist)
            {
                ints.Add(long.Parse(str));
            }
            for (int i = 0; i < 25; i++)
            {
                Blink(ref ints);
            }
            return ints.Count;
        }

        static void Blink(ref List<long> ints)
        {
            List<long> temp = new List<long>();
            for (int j = 0; j < ints.Count; j++)
            {
                int length = (int)Math.Floor(Math.Log10(ints[j]) + 1);
                if (ints[j] == 0) temp.Add(1);
                else if (length % 2 == 0)
                {
                    long left = (long)(ints[j] / Math.Pow(10, length / 2));
                    long right = (long)(ints[j] % Math.Pow(10, length / 2));
                    temp.Add(left);
                    temp.Add(right);
                }
                else
                {
                    temp.Add(2024 * ints[j]);
                }
            }
            ints = temp;
        }

        static void Blinkpt2(ref Dictionary<long, long> counts)
        {
            Dictionary<long, long> tempcounts = new Dictionary<long, long>();
            foreach (long stone in counts.Keys)
            {
                int length = (int)Math.Floor(Math.Log10(stone) + 1);
                if (stone == 0)
                {
                    if (!tempcounts.ContainsKey(1)) tempcounts.Add(1, 0);
                    tempcounts[1] += counts[stone];
                }
                else if (length % 2 == 0)
                {
                    long left = (long)(stone / Math.Pow(10, length / 2));
                    long right = (long)(stone % Math.Pow(10, length / 2));
                    if (!tempcounts.ContainsKey(left)) tempcounts.Add(left, 0);
                    tempcounts[left] += counts[stone];
                    if (!tempcounts.ContainsKey(right)) tempcounts.Add(right, 0);
                    tempcounts[right] += counts[stone];
                }
                else
                {
                    long val = stone * 2024;
                    if (!tempcounts.ContainsKey(val)) tempcounts.Add(val, 0);
                    tempcounts[val] += counts[stone];
                }
            }
            counts = tempcounts;
        }

        static long SOL2(string input)
        {
            string[] strlist = input.Split(' ');
            Dictionary<long, long> counts = new Dictionary<long, long>();
            foreach (string str in strlist)
            {
                counts.Add(long.Parse(str), 1);
            }
            for (int i = 0; i < 75; i++)
            {
                Blinkpt2(ref counts);
            }
            return counts.Values.Sum();
        }
    }
}
