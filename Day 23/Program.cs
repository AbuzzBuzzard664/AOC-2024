using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using System.Data.SqlClient;

namespace Day_23
{

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Part 1: {SOL1()}");
            Console.WriteLine($"Part 2: {SOL2()}");
            Console.ReadKey();
        }

        static (Dictionary<string, HashSet<string>>, HashSet<(string,string)>) GetPuzzleInput()
        {
            Dictionary<string, HashSet<string>> graph = new Dictionary<string, HashSet<string>>();
            HashSet<(string, string)> edges = new HashSet<(string, string)>();
            string[] connections = File.ReadAllLines("input.txt");
            foreach (string connection in connections)
            {
                string[] parts = connection.Split('-');
                string a = parts[0];
                string b = parts[1];
                edges.Add((a,b));
                if (!graph.ContainsKey(a))

                    graph[a] = new HashSet<string>();

                if (!graph.ContainsKey(b))
                    graph[b] = new HashSet<string>();

                graph[a].Add(b);
                graph[b].Add(a);
            }

            return (graph,edges);
        }

        static int SOL1()
        {
            (Dictionary<string, HashSet<string>> graph, HashSet<(string,string)> edges) = GetPuzzleInput();
            return GetConnections(graph, edges).Count;
        }

        static HashSet<string> GetConnections(Dictionary<string, HashSet<string>> graph, HashSet<(string, string)> edges)
        {
            HashSet<string> connections = new HashSet<string>();
            foreach (string node in graph.Keys)
            {
                foreach ((string u, string v) in edges)
                {
                    if (u == node || v == node)
                        continue;
                    List<string> temp = new List<string>() { u, v, node };
                    if ((edges.Contains((u, node)) || edges.Contains((node, u))) && (edges.Contains((v, node)) || edges.Contains((node, v))))
                    {
                        temp.Sort();
                        if (u[0] == 't' || v[0] == 't' || node[0] == 't')
                            connections.Add(temp[0] + "," + temp[1] + "," + temp[2]);
                    }
                }
            }
            return connections;
        }

        static string SOL2()
        {
            (Dictionary<string, HashSet<string>> graph, _)= GetPuzzleInput();
            List<string> maxClique = FindMaxClique(graph);
            return GetPassword(maxClique);
        }

        static List<string> FindMaxClique(Dictionary<string, HashSet<string>> graph)
        {
            List<string> maxClique = new List<string>();
            BronKerbosch(new List<string>(), graph.Keys.ToList(), new List<string>(), graph, ref maxClique);
            return maxClique;
        }

        static void BronKerbosch(List<string> R, List<string> P, List<string> X, Dictionary<string, HashSet<string>> graph, ref List<string> maxClique)
        {
            if (P.Count == 0 && X.Count == 0)
            {
                if (R.Count > maxClique.Count)
                    maxClique = new List<string>(R);
               
                return;
            }

            List<string> P_copy = new List<string>(P);
            foreach (string v in P_copy)
            {
                List<string> newR = new List<string>(R) { v };
                List<string> newP = P.Intersect(graph[v]).ToList();
                List<string> newX = X.Intersect(graph[v]).ToList();
                BronKerbosch(newR, newP, newX, graph, ref maxClique);
                P.Remove(v);
                X.Add(v);
            }
        }

        static string GetPassword(List<string> clique)
        {
            clique.Sort();
            return string.Join(",", clique);
        }
    }
}
