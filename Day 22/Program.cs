using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_22
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine($"Part 1: {SOL1()}");
            Console.WriteLine($"Part 2: {SOL2()}");
            Console.ReadKey();
        }

        static List<long> GetPuzzleInput()
        {
            string[] lines = File.ReadAllLines("input.txt");
            List<long> values = new List<long>();
            foreach (string line in lines)
                values.Add(long.Parse(line));
            return values;
        }

        static long SOL1()
        {
            List<long> prices = GetPuzzleInput();
            long sum = 0;
            foreach (long price in prices)
                sum += Solve(price);
            return sum;
        }

        static long Solve(long secret)
        {
            for (int i = 0; i < 2000; i++)
                secret = Next(secret);
            return secret;
        }

        static long Next(long secret)
        {
            secret = MixAndPrune(secret, secret * 64);
            secret = MixAndPrune(secret, secret / 32);
            return MixAndPrune(secret, secret * 2048);
        }

        static long MixAndPrune(long secret, long num)
            => (secret ^ num) % 16777216;

        static long SOL2()
        {
            List<long> prices = GetPuzzleInput();
            Dictionary<(int, int, int, int), int> sequences = new Dictionary<(int, int, int, int), int>();
            foreach (long price in prices)
                GetSequences(price, sequences);
            return sequences.Values.Max();
        }

        static void GetSequences(long secret, Dictionary<(int, int, int, int), int> sequences)
        {
            HashSet<(int, int, int, int)> seen = new HashSet<(int, int, int, int)>();
            List<int> changes = new List<int>();
            int prevPrice = (int)(secret % 10);
            for (int i = 0; i < 2000; i++)
            {
                long newSecret = Next(secret);
                int newPrice = (int)(newSecret % 10);
                changes.Add(newPrice - prevPrice);
                prevPrice = newPrice;
                secret = newSecret;
                if (changes.Count >= 4)
                {
                    (int, int, int, int) seq = (changes[changes.Count - 4], changes[changes.Count - 3], changes[changes.Count - 2], changes[changes.Count - 1]);
                    if (seen.Add(seq))
                    {
                        if (sequences.ContainsKey(seq))
                            sequences[seq] += newPrice;
                        else
                            sequences.Add(seq, newPrice);
                    }
                }
            }
        }
    }
}
