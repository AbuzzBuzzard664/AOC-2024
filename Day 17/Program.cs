using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_17
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Part 1: {SOL1()}");
            Console.WriteLine($"Part 2: {SOL2()}");
            Console.ReadKey();
        }

        static (long, List<int>) GetPuzzleInput()
        {
            string[] lines = File.ReadAllLines("input.txt");
            long regA = long.Parse(lines[0].Trim().Split(':')[1]);
            string[] opcodes = lines[4].Trim().Split(':')[1].Trim().Split(',').ToArray();
            List<int> operands = new List<int>();
            foreach (string line in opcodes)
            {
                operands.Add(int.Parse(line));
            }
            return (regA, operands);
        }

        static string SOL1()
        {
            (long regA, List<int> operands) = GetPuzzleInput();
            List<int> output = RunProg(operands, regA);
            string outstr = string.Join(",", output);
            return outstr;
        }

        static List<int> RunProg(List<int> operands, long regA)
        {
            long regB = 0;
            long regC = 0;
            List<int> output = new List<int>();
            int pc = 0;
            while (pc < operands.Count)
            {
                long combo;
                switch (operands[pc + 1])
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        combo = operands[pc + 1];
                        break;
                    case 4:
                        combo = regA;
                        break;
                    case 5:
                        combo = regB;
                        break;
                    case 6:
                        combo = regC;
                        break;
                    default:
                        combo = long.MinValue;
                        break;
                }
                long literal = operands[pc + 1];
                long res = 0;
                bool jumped = false;
                switch (operands[pc])
                {
                    case 0:
                        res = (long)(regA / Math.Pow(2, combo));
                        regA = res;
                        break;
                    case 1:
                        res = regB ^ literal;
                        regB = res;
                        break;
                    case 2:
                        res = combo % 8;
                        regB = res;
                        break;
                    case 3:
                        if (regA != 0)
                        {
                            pc = (int)literal;
                            jumped = true;
                        }
                        break;
                    case 4:
                        res = regB ^ regC;
                        regB = res;
                        break;
                    case 5:
                        output.Add((int)(combo % 8));
                        break;
                    case 6:
                        res = (long)(regA / Math.Pow(2, combo));
                        regB = res;
                        break;
                    case 7:
                        res = (long)(regA / Math.Pow(2, combo));
                        regC = res;
                        break;
                    default: break;
                }
                if (!jumped) pc += 2;
                if (output.Count > operands.Count) break;
            }

            return output;
        }

        static long SOL2()
        {
            (_, List<int> operands) = GetPuzzleInput();
            long aValue = GetA(operands);
            return aValue;
        }

        static long GetA(List<int> operands)
        {
            long a = 1;
            bool loop = true;
            while (loop)
            {
                List<int> aValues = RunProg(operands, a);
                if (ListsEqual(aValues, operands))
                    return a;
                if (operands.Count > aValues.Count)
                    a *= 2;
                if (operands.Count == aValues.Count)
                {
                    for (int i = operands.Count - 1; i > -1; i--)
                    {
                        if (operands[i] != aValues[i])
                        {
                            a += (long)Math.Pow(8, i); // every nth digit increments on every nth step
                            break;
                        }
                    }
                }
                if (operands.Count < aValues.Count)
                    a /= 2;

            }
            return a;
        }

        static bool ListsEqual(List<int> listToCheck, List<int> baseList)
        {
            if (listToCheck.Count == baseList.Count)
            {
                for (int i = 0; i < listToCheck.Count; i++)
                {
                    if (listToCheck[i] != baseList[i])
                        return false;
                }
                return true;
            }
            return false;
        }
    }
}