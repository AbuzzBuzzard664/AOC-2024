using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_09
{
    internal class Program
    {

        public struct File
        {
            public int Id;
            public int Length;
        }

        static void Main(string[] args)
        {
            string input = System.IO.File.ReadAllText("input.txt");
            Console.WriteLine("Part 1: " + SOL1(input));
            Console.WriteLine("Part 2: " + SOL2(input));
            Console.ReadKey();
        }

        static List<File> ReadDisk(string disks)
        {
            List<File> outlist = new List<File>();
            int k = 0;
            for (int i = 0; i < disks.Length; i++)
            {
                if (i % 2 == 0)
                {
                    File tempfile;
                    tempfile.Id = k;
                    tempfile.Length = int.Parse(disks[i].ToString());
                    for (int j = 0; j < tempfile.Length; j++) outlist.Add(tempfile);
                    k++;
                }
                else
                {
                    File nullfile;
                    nullfile.Id = -1;
                    nullfile.Length = int.Parse(disks[i].ToString());
                    for (int j = 0; j < nullfile.Length; j++) outlist.Add(nullfile);
                }
            }
            return outlist;
        }

        static List<File> Collapse(List<File> disk)
        {
            for (int i = 0; i < disk.Count; i++)
            {
                if (disk[i].Id == -1)
                {
                    for (int j = disk.Count - 1; j > i; j--)
                    {
                        if (disk[j].Id != -1)
                        {
                            File tempFile = disk[i];
                            disk[i] = disk[j];
                            disk[j] = tempFile;
                            break;
                        }
                    }
                }
            }
            return disk;
        }

        static long SOL1(string disks)
        {
            List<File> sortedDisk = Collapse(ReadDisk(disks));
            List<long> answers = new List<long>();
            for (int i = 0; i < sortedDisk.Count; i++)
            {
                if (sortedDisk[i].Id != -1)
                {
                    answers.Add(sortedDisk[i].Id * i);
                }
            }
            return answers.Sum();
        }

        static long SOL2(string disks)
        {
            List<File> sorted = Defragment(ReadDisk(disks));
            List<long> answers = new List<long>();
            for (int i = 0; i < sorted.Count; i++)
            {
                if (sorted[i].Id != -1) answers.Add(sorted[i].Id * i);
            }
            return answers.Sum();
        }

        static List<File> Defragment(List<File> Disk)
        {
            for (int i = 0; i < Disk.Count; i++)
            {
                if (Disk[i].Id == -1) //Free Space
                {
                    for (int j = Disk.Count - 1; j > i; j--) //For every element after the free space
                    {
                        if (Disk[j].Id != -1) //Is a file
                        {
                            if (Disk[i].Length >= Disk[j].Length)
                            {
                                int FileStartIndex = (j - Disk[j].Length) + 1;

                                //Move It!
                                int length = Disk[i].Length;
                                for (int k = 0; k < length; k++)
                                {
                                    File myFile = new File();
                                    myFile.Id = -1;
                                    myFile.Length = length - Disk[j].Length;

                                    Disk[i + k] = myFile;
                                }

                                for (int k = 0; k < Disk[j].Length; k++)
                                {
                                    Disk[i + k] = Disk[FileStartIndex + k];

                                    File myFile = new File();
                                    myFile.Id = -1;
                                    Disk[FileStartIndex + k] = myFile;
                                }
                                break;
                            }
                        }
                    }
                }
            }
            return Disk;
        }
    }
}