using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day_03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string lines = File.ReadAllText("input.txt");
            Console.WriteLine($"Part 1: {SOl1(lines)}\nPart 2: {SOl2(lines)}");
            Console.ReadKey();
        }

        static int SOl1(string line)
        {
            int answer = 0;
            foreach (Match s in Regex.Matches(line, "mul\\([1-9]\\d?\\d?\\,[1-9]\\d?\\d?\\)")) answer += FindMul(s.Value);
            return answer;
        }

        static int FindMul(string temp)
        {
            
            temp = temp.Replace("(", "").Replace("mul", "").Replace(")", ""); ;
            string[] split = temp.Trim().Split(',');
            int num1 = int.Parse(split[0]), num2 = int.Parse(split[1]);
            return num1 * num2;
        }

        static int SOl2(string lines)
        {
            int answer = 0;
            bool dont = false;
            foreach (Match s in Regex.Matches(lines, "mul\\([1-9]\\d?\\d?\\,[1-9]\\d?\\d?\\)|do(n't)?\\(\\)"))
            {
                if (s.Value == "don't()") dont = true;
                else if (s.Value == "do()") dont = false;
                if (s.Value.StartsWith("mul(") && !dont) answer += FindMul(s.Value);
            }
            return answer;
        }
    }
}
