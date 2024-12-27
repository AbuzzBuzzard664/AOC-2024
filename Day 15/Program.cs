using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;

namespace Day_15
{
    internal class Program
    {
        public struct Node
        {
            public int x, y;
            public char c;
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Part 1: {SOL1(GetPuzzleInput())}");
            Console.WriteLine($"Part 2: {SOL2(GetPuzzleInput())}");
            Console.ReadKey();
        }

        public enum Direction
        {
            left,
            right,
            up,
            down
        }


        static long SOL1((Node[,] map, Direction[] directions) intup)
        {
            Node[,] map = intup.map;
            Direction[] directions = intup.directions;
            foreach (Direction direction in directions)
            {
                Move(map, direction);
            }
            int answer = 0;
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[x, y].c == 'O')
                    {
                        answer += x + y * 100;
                    }
                }
            }
            return answer;
        }

        static void Move(Node[,] map, Direction dir)
        {
            Node robot = new Node();
            foreach (Node node in map) //finds the robot
            {
                if (node.c == '@')
                {
                    robot = node;
                    break;
                }
            }
            Node tempnode = new Node();
            switch (dir) //Gets where the next node to the robot.
            {
                case Direction.left:
                    tempnode.x = robot.x - 1;
                    tempnode.y = robot.y;
                    break;
                case Direction.right:
                    tempnode.x = robot.x + 1;
                    tempnode.y = robot.y;
                    break;
                case Direction.up:
                    tempnode.x = robot.x;
                    tempnode.y = robot.y - 1;
                    break;
                case Direction.down:
                    tempnode.x = robot.x;
                    tempnode.y = robot.y + 1;
                    break;
            }
            tempnode.c = map[tempnode.x, tempnode.y].c;
            if (tempnode.c != '#')
            {
                if (tempnode.c == '.')
                {
                    map[tempnode.x, tempnode.y].c = '@';
                    map[robot.x, robot.y].c = '.';
                }
                else if (tempnode.c == 'O')
                {
                    int moves = CheckNext(map, tempnode, dir);
                    Node tempnode2 = tempnode;
                    if (moves > 0)
                    {
                        map[tempnode.x, tempnode.y].c = '@';
                        for (int i = 1; i <= moves; i++)
                        {
                            switch (dir)
                            {
                                case Direction.left:
                                    tempnode2.x = tempnode.x - i;
                                    tempnode2.y = tempnode.y;
                                    break;
                                case Direction.right:
                                    tempnode2.x = tempnode.x + i;
                                    tempnode2.y = tempnode.y;
                                    break;
                                case Direction.up:
                                    tempnode2.x = tempnode.x;
                                    tempnode2.y = tempnode.y - i;
                                    break;
                                case Direction.down:
                                    tempnode2.x = tempnode.x;
                                    tempnode2.y = tempnode.y + i;
                                    break;
                            }
                            map[tempnode2.x, tempnode2.y].c = 'O';
                        }
                        map[robot.x, robot.y].c = '.';
                    }
                }
            }
        }

        static int CheckNext(Node[,] map, Node firstO, Direction dir)
        {
            bool loop = true;
            int moves = 1;
            while (loop)
            {
                Node tempnode = new Node();
                //checks the direction and how many boxes are in the line.
                switch (dir)
                {
                    case Direction.left:
                        tempnode.x = firstO.x - 1;
                        tempnode.y = firstO.y;
                        break;
                    case Direction.right:
                        tempnode.x = firstO.x + 1;
                        tempnode.y = firstO.y;
                        break;
                    case Direction.up:
                        tempnode.x = firstO.x;
                        tempnode.y = firstO.y - 1;
                        break;
                    case Direction.down:
                        tempnode.x = firstO.x;
                        tempnode.y = firstO.y + 1;
                        break;
                }
                if (map[tempnode.x, tempnode.y].c == 'O')
                {
                    moves++;
                    firstO = tempnode;
                }
                else if (map[tempnode.x, tempnode.y].c == '#')
                {
                    moves = 0;
                    loop = false;
                }
                else
                {
                    loop = false;
                }
            }
            return moves;
        }

        static (Node[,], Direction[]) GetPuzzleInput()
        {
            string[] lines = File.ReadAllLines("input.txt");
            Node[,] map = new Node[0, 0];
            List<Direction> directions = new List<Direction>();
            int x = 0, y = 0;
            bool firstline = true;
            foreach (string line in lines)
            {
                if (line.Contains("#"))
                {
                    if (firstline) map = new Node[line.Length, line.Length];
                    firstline = false;
                    foreach (char c in line)
                    {
                        Node tempNode;
                        tempNode.x = x;
                        tempNode.y = y;
                        tempNode.c = c;
                        map[x, y] = tempNode;
                        x++;
                    }
                    y++;
                    x = 0;
                }
                else
                {
                    foreach (char c in line)
                    {
                        switch (c)
                        {
                            case '^':
                                directions.Add(Direction.up);
                                break;
                            case '>':
                                directions.Add(Direction.right);
                                break;
                            case 'v':
                                directions.Add(Direction.down);
                                break;
                            case '<':
                                directions.Add(Direction.left);
                                break;
                        }
                    }
                }
            }
            return (map, directions.ToArray());
        }

        static long SOL2((Node[,] map, Direction[] directions) intup)
        {
            Node[,] map = intup.map;
            Direction[] directions = intup.directions;
            Node[,] biggerMap = new Node[map.GetLength(0) * 2, map.GetLength(1) * 2];
            foreach (Node node in map)
            {
                Node tempnode1 = new Node(), tempnode2 = new Node();
                switch (node.c)
                {
                    case '#':
                        tempnode1.c = '#';
                        tempnode2.c = '#';
                        break;
                    case '@':
                        tempnode1.c = '@';
                        tempnode2.c = '.';
                        break;
                    case '.':
                        tempnode1.c = '.';
                        tempnode2.c = '.';
                        break;
                    case 'O':
                        tempnode1.c = '[';
                        tempnode2.c = ']';
                        break;
                }
                tempnode1.x = node.x * 2;
                tempnode2.x = node.x * 2 + 1;
                tempnode1.y = node.y;
                tempnode2.y = node.y;
                biggerMap[node.x * 2, node.y] = tempnode1;
                biggerMap[node.x * 2 + 1, node.y] = tempnode2;
            }
            foreach (Direction direction in directions)
            {
                Move2(biggerMap, direction);
            }
            int answer = 0;
            for (int y = 0; y < biggerMap.GetLength(0); y++)
            {
                for (int x = 0; x < biggerMap.GetLength(1); x++)
                {
                    if (biggerMap[x, y].c == '[')
                    {
                        answer += x + y * 100;
                    }
                }
            }
            return answer;
        }

        static void Move2(Node[,] map, Direction dir)
        {
            Node robot = new Node();
            foreach (Node node in map) //finds the robot
            {
                if (node.c == '@')
                {
                    robot = node;
                    break;
                }
            }
            Node tempnode = new Node();
            switch (dir) //Gets where the next node to the robot.
            {
                case Direction.left:
                    tempnode.x = robot.x - 1;
                    tempnode.y = robot.y;
                    break;
                case Direction.right:
                    tempnode.x = robot.x + 1;
                    tempnode.y = robot.y;
                    break;
                case Direction.up:
                    tempnode.x = robot.x;
                    tempnode.y = robot.y - 1;
                    break;
                case Direction.down:
                    tempnode.x = robot.x;
                    tempnode.y = robot.y + 1;
                    break;
            }
            tempnode.c = map[tempnode.x, tempnode.y].c;
            if (tempnode.c != '#')
            {
                if (tempnode.c == '.')
                {
                    map[tempnode.x, tempnode.y].c = '@';
                    map[robot.x, robot.y].c = '.';
                }
                else if (tempnode.c == '[' || tempnode.c == ']')
                {
                    Node tempnode2 = tempnode;
                    List<Node> nodesup = new List<Node>();
                    bool notFoundWall = true;
                    int i = 0;
                    while (notFoundWall)
                    {
                        switch (dir)
                        {
                            case Direction.left:
                                tempnode2.x = tempnode.x - i;
                                tempnode2.y = tempnode.y;
                                tempnode2.c = map[tempnode2.x, tempnode2.y].c;
                                if (tempnode2.c == '#')
                                {
                                    notFoundWall = false;
                                    nodesup.Clear();
                                    break;
                                }
                                else if (tempnode2.c == '.')
                                {
                                    notFoundWall = false;
                                }
                                else nodesup.Add(tempnode2);
                                i++;
                                break;
                            case Direction.right:
                                tempnode2.x = tempnode.x + i;
                                tempnode2.y = tempnode.y;
                                tempnode2.c = map[tempnode2.x, tempnode2.y].c;
                                if (tempnode2.c == '#')
                                {
                                    notFoundWall = false;
                                    nodesup.Clear();
                                    break;
                                }
                                else if (tempnode2.c == '.')
                                {
                                    notFoundWall = false;
                                }
                                else nodesup.Add(tempnode2);
                                i++;
                                break;
                            default:
                                HashSet<Node> reprints = new HashSet<Node>();
                                bool hithash = false;
                                BoxUpDown(tempnode, map, dir, reprints, ref hithash);
                                if (!hithash)
                                {
                                    foreach (Node node in reprints)
                                    {
                                        nodesup.Add(node);
                                    }
                                }
                                notFoundWall = false;
                                break;
                        }
                    }
                    if (nodesup.Count > 0)
                    {
                        if (dir == Direction.up || dir == Direction.down)
                        {
                            foreach (Node node in nodesup)
                            {
                                if (dir == Direction.up) map[node.x, node.y - 1].c = node.c;
                                else map[node.x, node.y + 1].c = node.c;
                            }
                        }
                        else
                        {
                            foreach (Node node in nodesup)
                            {
                                if (dir == Direction.left) map[node.x - 1, node.y].c = node.c;
                                else map[node.x + 1, node.y].c = node.c;
                            }
                        }
                        map[tempnode.x, tempnode.y].c = '@';
                        if (dir == Direction.up || dir == Direction.down)
                        {
                            if (map[tempnode.x - 1, tempnode.y].c == '[') map[tempnode.x - 1, tempnode.y].c = '.';
                            else if (map[tempnode.x + 1, tempnode.y].c == ']') map[tempnode.x + 1, tempnode.y].c = '.';
                        }
                        map[robot.x, robot.y].c = '.';
                    }
                    for (int y = 0; y < map.GetLength(0); y++)
                    {
                        for (int x = 0; x < map.GetLength(1); x++)
                        {
                            if (map[x, y].c == '[' && '[' == map[x - 1, y].c)
                            {
                                map[x - 1, y].c = '.';
                            }
                            else if (map[x, y].c == ']' && ']' == map[x + 1, y].c)
                            {
                                map[x + 1, y].c = '.';
                            }
                        }
                    }
                }
            }
        }

        static void BoxUpDown(Node start, Node[,] map, Direction dir, HashSet<Node> nodestomove, ref bool hithash)
        {
            Node myNode, myNode2;
            myNode.y = start.y;
            myNode.x = start.x;
            myNode.c = map[myNode.x, myNode.y].c;
            if (myNode.c == '[')
            {
                myNode2.x = myNode.x + 1;
                myNode2.y = myNode.y;
                myNode2.c = map[myNode2.x, myNode2.y].c;
                nodestomove.Add(myNode);
                nodestomove.Add(myNode2);
                Node temp = myNode;
                if (dir == Direction.up)
                {
                    temp.y = myNode.y - 1;
                }
                else
                {
                    temp.y = myNode.y + 1;
                }
                BoxUpDown(temp, map, dir, nodestomove, ref hithash);
                temp = myNode2;
                if (dir == Direction.up)
                {
                    temp.y = myNode2.y - 1;
                }
                else
                {
                    temp.y = myNode2.y + 1;
                }
                BoxUpDown(temp, map, dir, nodestomove, ref hithash);
            }
            else if (myNode.c == ']')
            {
                myNode2.x = myNode.x - 1;
                myNode2.y = myNode.y;
                myNode2.c = map[myNode2.x, myNode2.y].c;
                nodestomove.Add(myNode);
                nodestomove.Add(myNode2);
                Node temp = myNode;
                if (dir == Direction.up)
                    temp.y = myNode.y - 1;
                else
                    temp.y = myNode.y + 1;
                
                BoxUpDown(temp, map, dir, nodestomove, ref hithash);
                temp = myNode2;
                if (dir == Direction.up)
                    temp.y = myNode2.y - 1;
                else
                    temp.y = myNode2.y + 1;
                BoxUpDown(temp, map, dir, nodestomove, ref hithash);
            }
            else if (myNode.c == '#')
            {
                nodestomove.Clear();
                hithash = true;
            }
            else
            {
                //lol its a .
            }
        }
    }
}