using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_24
{
    internal class Program
    {
        public struct Wire
        {
            public string name;
            public bool value;

            public Wire(string n, bool v)
            {
                name = n;
                value = v;
            }

        }

        public struct Gate
        {
            public Wire a, b;
            public Operation op;

            public Gate(Wire a, Wire b, string op)
            {
                this.a = a;
                this.b = b;
                switch (op)
                {
                    case "AND":
                        this.op = Operation.AND;
                        break;
                    case "OR":
                        this.op = Operation.OR;
                        break;
                    case "XOR":
                        this.op = Operation.XOR;
                        break;
                    default:
                        this.op = Operation.AND;
                        break;
                }
            }
        }

        public enum Operation
        {
            AND,
            OR,
            XOR
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Part 1: {SOL1()}");
            Console.WriteLine($"Part 2: {SOL2()}");
            Console.ReadKey();
        }

        static (HashSet<Wire>, Dictionary<Wire, Gate>) GetPuzzleInput()
        {
            string[] lines = File.ReadAllLines("input.txt");
            HashSet<Wire> wires = new HashSet<Wire>();
            Dictionary<Wire, Gate> gates = new Dictionary<Wire, Gate>();
            foreach (string line in lines)
            {
                if (line.Contains(':'))
                {
                    wires.Add(new Wire(line.Substring(0, 3), line.Last() == '1'));
                }
                else if (line.Contains("->"))
                {
                    string[] temp = line.Replace("->", "").Split(' ');
                    Wire q = new Wire(temp[4], false);
                    Gate gate = new Gate(new Wire(temp[0], false), new Wire(temp[2], false), temp[1]);
                    foreach (Wire wire in wires)
                    {
                        if (wire.name == temp[0])
                            gate.a = wire;
                        if (wire.name == temp[2])
                            gate.b = wire;
                    }
                    gates.Add(q, gate);
                }
            }
            return (wires, gates);
        }

        static bool Evaluate(Wire label, Dictionary<Wire, Gate> gates, HashSet<Wire> wires)
        {
            if (wires.Contains(label)) return label.value;
            switch (gates[label].op)
            {
                case Operation.AND:
                    return Evaluate(gates[label].a, gates, wires) & Evaluate(gates[label].b, gates, wires);
                case Operation.OR:
                    return Evaluate(gates[label].a, gates, wires) | Evaluate(gates[label].b, gates, wires);
                case Operation.XOR:
                    return Evaluate(gates[label].a, gates, wires) ^ Evaluate(gates[label].b, gates, wires);
                default:
                    return label.value;

            }
        }

        static long SOL1()
        {
            (HashSet<Wire> wires, Dictionary<Wire, Gate> gates) = GetPuzzleInput();
            List<Wire> outputs = gates.Keys.Where(x => x.name.StartsWith("z")).OrderByDescending(a => a.name).ToList();
            long res = 0;
            foreach (Wire wire in gates.Keys.Where(x => x.name.StartsWith("z")).OrderByDescending(a => a.name))
                res = res * 2 + (Evaluate(wire, gates, wires) ? 1 : 0);

            return res;
        }

        public struct Triple
        {
            public string Original;
            public string Lhs;
            public string Rhs;
            public Operation Op;
            public string Target;

            public Triple(string or, string l, string r, string op, string targ)
            {
                this.Original = or;
                this.Lhs = l;
                this.Rhs = r;
                this.Target = targ;
                switch (op)
                {
                    case "AND":
                        Op = Operation.AND;
                        break;
                    case "OR":
                        Op = Operation.OR;
                        break;
                    case "XOR":
                        Op = Operation.XOR;
                        break;
                    default:
                        Op = Operation.OR;
                        break;
                }
            }
        }

        public static string SOL2()
        {
            string[] lines = File.ReadAllLines("input.txt");
            List<Triple> triples = new List<Triple>();
            foreach (string line in lines)
            {
                if (line.Contains("->"))
                {
                    string[] parts = line.Split(new[] { " -> " }, StringSplitOptions.None);
                    string[] expression = parts[0].Split(' ');

                    string lhs = expression[0];
                    string rhs = expression[2];
                    string op = expression[1];
                    string target = parts[1];

                    if (String.Compare(lhs, rhs, StringComparison.Ordinal) > 0)
                    {
                        (rhs, lhs) = (lhs, rhs);
                    }

                    triples.Add(new Triple(line, lhs, rhs, op, target));
                }
            }

            HashSet<string> answer = new HashSet<string>();

            // Process each Triple to identify errors
            foreach (Triple triple in triples)
            {
                // Rule 1: Only XORs can target wires starting with 'z', and except 'z45'
                if (triple.Target.StartsWith("z") && triple.Op != Operation.XOR && triple.Target != "z45")
                {
                    answer.Add(triple.Target);
                }

                // Rule 2: XORs must target wires starting with 'z' and not have Lhs starting with 'x'
                if (!triple.Target.StartsWith("z") && triple.Op == Operation.XOR && !triple.Lhs.StartsWith("x"))
                {
                    answer.Add(triple.Target);
                }

                // Rule 3: AND must feed into ORs
                if (triple.Op == Operation.AND && triple.Lhs != "x00")
                {
                    List<Triple> feeds = triples.Where(t => t.Lhs == triple.Target || t.Rhs == triple.Target).ToList();
                    foreach (Triple fed in feeds)
                    {
                        if (fed.Op != Operation.OR)
                        {
                            answer.Add(triple.Target);
                            break;
                        }
                    }
                }

                // Rule 4: ORs must be fed by ANDs
                if (triple.Op == Operation.OR)
                {
                    Triple LHSFeeds = new Triple();
                    foreach (Triple t in triples)
                    {
                        if (t.Target == triple.Lhs)
                        {
                            LHSFeeds = t;
                            break;
                        }
                    }
                    if (LHSFeeds.Op != Operation.AND)
                        answer.Add(LHSFeeds.Target);

                    Triple RHSFeeds = new Triple();
                    foreach (Triple t in triples)
                    {
                        if (t.Target == triple.Rhs)
                        {
                            RHSFeeds = t;
                            break;
                        }
                    }

                    if (RHSFeeds.Op != Operation.AND)
                        answer.Add(RHSFeeds.Target);
                }
            }
            List<string> sortedAnswer = answer.ToList();
            sortedAnswer.Sort();
            return string.Join(",", sortedAnswer);
        }
    }
}
