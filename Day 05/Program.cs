using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Dynamic;

namespace Day_05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            (List<string> updates, List<string> rules) = GetPuzzleInput();
            Console.WriteLine($"Part 1: {SOL1(updates, rules)}\nPart 2: {SOL2(updates, rules)}");
            Console.ReadKey();
        }

        static (List<string>, List<string>) GetPuzzleInput()
        {
            string[] lines = File.ReadAllLines("input.txt");
            List<string> updates = new List<string>();
            List<string> rules = new List<string>();
            foreach (string line in lines)
            {
                if (line.Contains("|"))
                    rules.Add(line);
                else if (line.Contains(","))
                    updates.Add(line);
            }
            return (updates, rules);
        }

        static int SOL1(List<string> updates, List<string> rules)
        {
            int answer = 0;
            foreach (string update in updates)
            {
                if (CheckUpdate(update, rules.ToArray()))
                {
                    string[] temp = update.Split(',');
                    answer += int.Parse(temp[temp.Length / 2]);
                }
            }
            return answer;
        }

        static bool CheckUpdate(string update, string[] rules)
        {
            foreach (string rule in rules)
            {
                (string a, string b) = (rule.Substring(0, 2), rule.Substring(3, 2));
                string[] temp = update.Split(',');
                if (temp.Contains(a) && temp.Contains(b))
                {
                    if (temp.ToList().IndexOf(a) > temp.ToList().IndexOf(b)) return false;
                }
            }
            return true;
        }

        static int SOL2(List<string> updates, List<string> rules)
        {
            int answer = 0;
            foreach (string update in updates)
            {
                if (!CheckUpdate(update, rules.ToArray()))
                {
                    string[] temp = update.Split(',');
                    List<int> updater = new List<int>();
                    foreach (string s in temp)
                    {
                        updater.Add(int.Parse(s));
                    }
                    updater.Sort((a, b) =>
                    {
                        foreach (string rule in rules)
                        {
                            (int a, int b) order = (int.Parse(rule.Substring(0, 2)), int.Parse(rule.Substring(3, 2)));
                            if (order == (a, b) || order == (b, a))
                            {
                                return a == order.a ? -1 : 1;
                            }
                        }
                        return 0;
                    });
                    answer += updater[updater.Count / 2];
                }
            }
            return answer;
        }
    }
}
