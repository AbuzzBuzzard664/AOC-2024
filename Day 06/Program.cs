using System;
using System.IO;

namespace Day_06
{
    internal class Program
    {
        static void Main(string[] args)
        {
            char[,] map = Get2DArray();
            int answer = SOL1(map);
            Console.WriteLine($"Part 1: {answer}");
            answer = SOL2(map);
            Console.WriteLine($"Part 2: {answer}");
            Console.ReadKey();
        }

        static char[,] Get2DArray()
        {
            string[] lines = File.ReadAllLines("input.txt");
            char[,] map = new char[lines[0].Length, lines.Length];
            for (int i = 0; i < lines.Length; i++)
                for (int j = 0; j < lines[0].Length; j++)
                    map[j, i] = lines[i][j];
                
            return map;
        }

        static int SOL1(char[,] map)
        {
            (int x, int y) currentpos = (0, 0);
            int i = 0;
            foreach (char c in map)
            {
                if (c.ToString() == "^")
                {
                    currentpos = (i / map.GetLength(1), i % map.GetLength(0));
                }
                i++;
            }
            Move(currentpos, map);
            int answer = 0;
            foreach (char c in map)
            {
                if (c == 'X') answer++;
            }
            return answer;
        }

        static bool Move((int x, int y) currentpos, char[,] map)
        {
            int right = 0, up = 1;
            (int x, int y) newpos;
            bool finished = false;
            int[,] visited = new int[map.GetLength(0), map.GetLength(1)];
            visited[currentpos.x, currentpos.y]++;
            map[currentpos.x, currentpos.y] = 'X';
            while (!finished)
            {
                newpos = (currentpos.x + right, currentpos.y - up);
                if (HitEdge(newpos, map))
                    finished = true;
                else if (map[newpos.x, newpos.y] == '#')
                    ChangeDirection(ref right, ref up);
                else
                {
                    currentpos = newpos;
                    visited[currentpos.x, currentpos.y]++;
                    map[currentpos.x, currentpos.y] = 'X';
                }
                foreach (int i in visited)
                    if (i > 4) return true;
                
            }
            return false;
        }

        static bool HitEdge((int x, int y) inpos, char[,] map) 
            => (inpos.x == -1 || inpos.y == -1 || inpos.x == map.GetLength(0) || inpos.y == map.GetLength(1));



        static void ChangeDirection(ref int right, ref int up)
        {
            if (up > 0)
            {
                right++;
                up--;
            }
            else if (right > 0)
            {
                up--;
                right--;
            }
            else if (up < 0)
            {
                up++;
                right--;
            }
            else if (right < 0)
            {
                right++;
                up++;
            }
        }

        static int SOL2(char[,] map)
        {
            map = Get2DArray();
            (int x, int y) startpos = (0, 0);
            int i = 0;
            int answer = 0;
            foreach (char c in map)
            {
                if (c.ToString() == "^")
                {
                    startpos = (i / map.GetLength(0), i % map.GetLength(1));
                    break;
                }
                i++;
            }
            Move(startpos, map);
            char[,] temp;
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {

                    if (map[x, y] == 'X' && startpos != (x, y))
                    {
                        temp = ResetMap(map);
                        temp[x, y] = '#';
                        if (Move(startpos, temp)) answer++;
                    }
                }
            }
            return answer;
        }

        static char[,] ResetMap(char[,] map)
        {
            char[,] chars = new char[map.GetLength(0), map.GetLength(1)];
            int i = 0;
            foreach (char c in map)
            {
                if (c != '#')
                    chars[i / chars.GetLength(0), i % chars.GetLength(1)] = '.';
                else
                    chars[i / chars.GetLength(0), i % chars.GetLength(1)] = '#';
                i++;
            }
            return chars;
        }
    }
}