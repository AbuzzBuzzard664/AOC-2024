using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day_04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            Console.WriteLine($"Part 1:  {SOl1(lines)} \nPart 2: {SOl2(lines)}");
            Console.ReadKey();
        }

        static int SOl1(string[] lines)
        {
            int answer = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == 'X')
                    {
                        answer += CheckAround(lines, j, i);
                    }
                }
            }
            return answer;
        }

        static int CheckAround(string[] lines, int x, int y)
        {
            List<string> maybes = new List<string>();
            if (x < lines[y].Length - 3) 
                maybes.Add(lines[y][x].ToString() + lines[y][x + 1].ToString() + lines[y][x + 2].ToString() + lines[y][x + 3].ToString());
            if (x > 2) 
                maybes.Add(lines[y][x].ToString() + lines[y][x - 1].ToString() + lines[y][x - 2].ToString() + lines[y][x - 3].ToString());
            if (y > 2) 
                maybes.Add(lines[y][x].ToString() + lines[y - 1][x].ToString() + lines[y - 2][x].ToString() + lines[y - 3][x].ToString());
            if (y < lines.Length - 3) 
                maybes.Add(lines[y][x].ToString() + lines[y + 1][x].ToString() + lines[y + 2][x].ToString() + lines[y + 3][x].ToString());
            if (x > 2 && y > 2) 
                maybes.Add(lines[y][x].ToString() + lines[y - 1][x - 1].ToString() + lines[y - 2][x - 2].ToString() + lines[y - 3][x - 3].ToString());
            if (x < lines[y].Length - 3 && y > 2) 
                maybes.Add(lines[y][x].ToString() + lines[y - 1][x + 1].ToString() + lines[y - 2][x + 2].ToString() + lines[y - 3][x + 3].ToString());
            if (x > 2 && y < lines.Length - 3) 
                maybes.Add(lines[y][x].ToString() + lines[y + 1][x - 1].ToString() + lines[y + 2][x - 2].ToString() + lines[y + 3][x - 3].ToString());
            if (x < lines[y].Length - 3 && y < lines.Length - 3) 
                maybes.Add(lines[y][x].ToString() + lines[y + 1][x + 1].ToString() + lines[y + 2][x + 2].ToString() + lines[y + 3][x + 3].ToString());
            return maybes.Where(a => a == "XMAS").Count();
             
        }

        static int SOl2(string[] lines)
        {
            int answer = 0;
            for (int i = 1; i < lines.Length - 1; i++)
            {
                for (int j = 1; j < lines[i].Length - 1; j++)
                {
                    if (lines[i][j] == 'A')
                    {
                        string lefttoright = lines[i - 1][j - 1].ToString() + lines[i][j].ToString() + lines[i + 1][j + 1].ToString();
                        string righttoleft = lines[i - 1][j + 1].ToString() + lines[i][j].ToString() + lines[i + 1][j - 1].ToString();
                        if ((Regex.IsMatch(lefttoright, "(MAS)|(SAM)"))
                           && (Regex.IsMatch(righttoleft, "(MAS)|(SAM)"))) answer++;
                    }
                }
            }
            return answer;
        }
    }
}
