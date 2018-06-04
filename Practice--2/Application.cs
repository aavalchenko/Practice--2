using System;
using System.Collections.Generic;
using System.IO;

namespace Practice_2
{
    public static class State
    {
        public static char[,] Shift(this char[,] pred, int di, int dj)
        {
            char[,] next = (char[,])pred.Clone();
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (next[i, j] == '#')
                    {
                        int ni = i + di;
                        int nj = j + dj;
                        if (0 <= ni && ni < 2 && 0 <= nj && nj < 4)
                        {
                            Swap(ref next[i, j], ref next[ni, nj]);
                        }
                        return next;
                    }
                }
            }
            throw new ArgumentException();
        }
        public static void Swap(ref char left, ref char right)
        {
            char temp = left;
            left = right;
            right = temp;
        }
        public static void ReadStart(this char[,] a, string input)
        {
            Int16 count = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    a[i,j] = input[count];
                    count++;
                }
            }
        }
        public static void ReadFinish(this char[,] a, string input)
        {
            Int16 count = 8;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    a[i, j] = input[count];
                    count++;
                }
            }
        }
    }

    public static class Application
    {
        private static char[,] start = new char[2,4];
        private static char[,] finish = new char[2,4];

        public static void Main(string[] args)
        {
            Dictionary<string, int> statesList = new Dictionary<string, int>();
            Queue<char[,]> queue = new Queue<char[,]>();
            char[,] cur;
            Initialize(ref statesList, ref queue);
            while (queue.Count != 0)
            {
                cur = queue.Dequeue();
                if (ToString(cur) == ToString(finish))
                {
                    File.AppendAllText("output.txt", statesList[ToString(cur)].ToString());
                    return;
                }

                for (int di = -1; di <= 1; di++)
                {
                    for (int dj = -1; dj <= 1; dj++)
                    {
                        if (di * di + dj * dj == 1)
                        {
                            char[,] next = (cur.Shift(di, dj));
                            if (!statesList.ContainsKey(ToString(next)))
                            {
                                statesList.Add(ToString(next), statesList[ToString(cur)] + 1);
                                queue.Enqueue(next);
                            }
                        }
                    }
                }
            }
            File.WriteAllText("output.txt", (-1).ToString());
        }
        
        private static void Initialize(ref Dictionary<string, int> statesList, ref Queue<char[,]> queue)
        {
            string input = File.ReadAllText("input.txt").Replace("\r", "").Replace("\n", "");
            start.ReadStart(input);
            finish.ReadFinish(input);
            statesList.Add(ToString(start), 0);
            queue.Enqueue(start);
        }

        private static string ToString(char[,] arr)
        {
            string output = "";
            foreach (char symbol in arr)
            {
                output += symbol;
            }

            return output;
        }
    }
}