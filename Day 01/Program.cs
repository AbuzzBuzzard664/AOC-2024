using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Day_01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            (List<int> line1, List<int> line2) = GetPuzzleInput();
            Console.WriteLine($"Part 1: {SOl1(line1, line2)}\nPart 2: {SOl2(line1, line2)}");
            Console.ReadKey();
        }

        static (List<int>, List<int>) GetPuzzleInput()
        {
            string[] lines = File.ReadAllLines("input.txt");
            List<int> line1 = new List<int>();
            List<int> line2 = new List<int>();
            foreach (string line in lines)
            {
                string[] nums = line.Split(' ');
                line1.Add(int.Parse(nums[0].Trim()));
                line2.Add(int.Parse(nums[3].Trim()));
            }
            line1.Sort();
            line2.Sort();
            return (line1, line2);
        }

        static int SOl1(List<int> line1, List<int> line2)
        {
            int answer = 0;
            for (int i = 0; i < line1.Count; i++) answer += Math.Abs(line2[i] - line1[i]);
            return answer;
        }

        static int SOl2(List<int> line1, List<int> line2) =>line1.Select(x => x * line2.Where(y => x==y).Count()).Sum();
        
    }
}