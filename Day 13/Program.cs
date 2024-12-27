using System;
using System.Collections.Generic;
using System.IO;

namespace Day_13
{
    internal class Program
    {

        public struct clawMachine
        {
            public Matrix m;
            public long xans, yans;
        }
        public class Matrix
        {
            public long a, b, c, d;

            public Matrix((int x, int y) ina, (int x, int y) inb)
            {
                a = ina.x;
                c = ina.y;
                b = inb.x;
                d = inb.y;
            }

            public long GetDeterminent() => a * d - b * c;

            public (long, long) Solve((long e, long f) answer)
            {
                (long x, long y) variables = (0, 0);
                variables.x = (d * answer.e - b * answer.f) / GetDeterminent();
                variables.y = (-c * answer.e + a * answer.f) / GetDeterminent();
                return variables;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Part 1: {SOL1(GetPuzzleInput())}");
            Console.WriteLine($"Part 2: {SOL2(GetPuzzleInput())}");
            Console.ReadKey();
        }

        static List<clawMachine> GetPuzzleInput()
        {
            string[] lines = File.ReadAllLines("input.txt");
            List<clawMachine> claw = new List<clawMachine>();
            (int x, int y) left = (0, 0), right = (0, 0);

            int i = 0;
            foreach (string line in lines)
            {
                if (i == 0)
                {
                    string temp = line.Substring(10);
                    string[] splits = temp.Trim().Split(',');
                    left.x = int.Parse(splits[0].Trim().Substring(1));
                    left.y = int.Parse(splits[1].Trim().Substring(1));
                    i++;
                }
                else if (i == 1)
                {
                    string temp = line.Substring(10);
                    string[] splits = temp.Trim().Split(',');
                    right.x = int.Parse((splits[0].Trim().Substring(1)));
                    right.y = int.Parse(splits[1].Trim().Substring(1));
                    i++;
                }
                else if (i == 2)
                {
                    string temp = line.Substring(8);
                    string[] splits = temp.Trim().Split(',');
                    clawMachine myClaw;
                    myClaw.xans = int.Parse(splits[0].Trim().Substring(1));
                    myClaw.yans = int.Parse(splits[1].Trim().Substring(2));
                    myClaw.m = new Matrix(left, right);
                    claw.Add(myClaw);
                    i++;
                }
                else
                    i = 0;
                
            }
            return claw;
        }

        static long SOL1(List<clawMachine> clawMachines)
        {
            long coins = 0;
            foreach (clawMachine clawMachine in clawMachines)
            {
                (long a, long b) temp = GetToXY(clawMachine);
                coins += temp.a * 3 + temp.b * 1;
            }
            return coins;
        }

        static (long, long) GetToXY(clawMachine machine)
        {
            long acount, bcount;
            if (machine.m.GetDeterminent() != 0)
            {
                acount = machine.m.Solve((machine.xans, machine.yans)).Item1;
                bcount = machine.m.Solve((machine.xans, machine.yans)).Item2;
                if ((acount * machine.m.a + bcount * machine.m.b, acount * machine.m.c + bcount * machine.m.d) == (machine.xans, machine.yans))
                {
                    return (acount, bcount);
                }
            }
            return (0, 0);
        }

        static long SOL2(List<clawMachine> machines)
        {
            long offset = 10000000000000;
            long coins = 0;
            foreach (clawMachine machine in machines)
            {
                clawMachine myClaw;
                myClaw.m = machine.m;
                myClaw.xans = machine.xans + offset;
                myClaw.yans = machine.yans + offset;
                (long a, long b) temp = GetToXY(myClaw);
                coins += temp.a * 3 + temp.b * 1;
            }
            return coins;
        }
    }
}
