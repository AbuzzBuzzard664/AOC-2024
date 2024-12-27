using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_19
{
    internal class Program
    {

        static string[] patterns;

        static void Main(string[] args)
        {
            Console.WriteLine($"Part 1: {SOL1()}");
            Console.WriteLine($"Part 2: {SOL2()}");
            Console.ReadKey();
        }

        static string[] GetPuzzleInput()
        {
            string[] lines = File.ReadAllLines("input.txt");
            patterns = lines[0].Trim().Split(',');
            for (int i = 0; i < patterns.Length; i++)
                patterns[i] = patterns[i].Trim();
            Array.Sort(patterns, (x, y) => y.Length.CompareTo(x.Length));
            List<string> towel_toGet = new List<string>();
            foreach (string line in lines.Skip(2))
                towel_toGet.Add(line);

            return towel_toGet.ToArray();
        }

        static long SOL1()
        {
            string[] towel_toGet = GetPuzzleInput();
            int answer = 0;
            Dictionary<string, long> solved = new Dictionary<string, long>();
            foreach (string towel in towel_toGet)
                answer += CanGetTowel(towel, solved) > 0 ? 1 : 0;
            return answer;
        }

        static long SOL2()
        {
            string[] towels = GetPuzzleInput();
            long answer = 0;
            Dictionary<string, long> solved = new Dictionary<string, long>();
            foreach (string towel in towels)
                answer += CanGetTowel(towel, solved);
            return answer;
        }

        static long CanGetTowel(string towel, Dictionary<string, long> solved)
        {
            if (towel.Length == 0)
                return 1;
            else if (solved.ContainsKey(towel))
                return solved[towel];
            long combinations = 0;
            int i = 0;
            foreach (string pattern in patterns.SkipWhile(pat => pat.Length > towel.Length - i))
            {
                if (towel.StartsWith(pattern))
                {
                    combinations += CanGetTowel(towel.Substring(pattern.Length), solved);
                    i++;
                }
            }
            solved.Add(towel, combinations);
            return combinations;
        }
    }
}