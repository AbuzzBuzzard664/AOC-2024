using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Day_07
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Part 1: " + SOL1(GetPuzzleInput()));
            Console.WriteLine("Part 2: " + SOL2(GetPuzzleInput()));
            Console.ReadKey();
        }

        static List<(long, List<long>)> GetPuzzleInput()
        {
            string[] lines = File.ReadAllLines("input.txt");
            List<(long, List<long>)> tests = new List<(long, List<long>)>();
            foreach (string line in lines)
            {
                string[] temp = line.Split(':');
                string[] temp2 = temp[1].Split(' ').Skip(1).ToArray();
                List<long> temp3 = new List<long>();
                for (int i = 0; i < temp2.Length; i++)
                    temp3.Add(int.Parse(temp2[i]));
                tests.Add((long.Parse(temp[0].Trim()), temp3));
            }
            return tests;
        }

        static long SOL1(List<(long, List<long>)> tests)
        {
            long answer = 0;
            foreach ((long, List<long>) a in tests)
                if (GetOperation(a.Item2.First(), a.Item1, a.Item2.Skip(1).ToList(), false)) answer += a.Item1;

            return answer;
        }

        static bool GetOperation(long curVal, long target, List<long> remainingVals, bool part2 = false)
        {
            if (remainingVals.Count() == 0) 
                return curVal == target;
            long tempM = curVal * remainingVals.First();
            long tempA = curVal + remainingVals.First();
            long tempC = long.Parse($"{curVal}{remainingVals.First()}");
            if (GetOperation(tempM, target, remainingVals.Skip(1).ToList(), part2)) 
                return true;
            if (GetOperation(tempA, target, remainingVals.Skip(1).ToList(), part2)) 
                return true;
            if (part2)
                if (GetOperation(tempC, target, remainingVals.Skip(1).ToList(), true)) 
                    return true;
            return false;
        }

        static long SOL2(List<(long, List<long>)> tests)
        {
            long answer = 0;
            foreach ((long, List<long>) a in tests)
                if (GetOperation(a.Item2.First(), a.Item1, a.Item2.Skip(1).ToList(), true)) answer += a.Item1;

            return answer;
        }
    }
}
