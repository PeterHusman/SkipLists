using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkipLists
{
    class Program
    {
        static void Main(string[] args)
        {
            SkipList<int> skipList = new SkipList<int>(Comparer<int>.Default);
            while (true)
            {
                Console.Clear();
                Visualize(skipList);
                string s = Console.ReadLine();
                int val = s.Length > 1 ? int.Parse(s.Remove(0, 1)) : 0;
                if (s[0] == 'i')
                {
                    skipList.Add(val);
                }
                else if (s[0] == 'c')
                {
                    Console.Write(skipList.Contains(val));
                    System.Threading.Thread.Sleep(500);
                }
                else if(s[0] == 'd')
                {
                    Console.Write(skipList.Remove(val));
                    System.Threading.Thread.Sleep(500);
                }
                else if (s == "R")
                {
                    skipList.Clear();
                }
            }
        }

        static void Visualize(SkipList<int> sL)
        {
            Node<int> n = sL.header;
            int x = 0;
            while (n != null)
            {
                for (int i = 0; i < sL.header.Height; i++)
                {
                    if (n.Height > i)
                    {
                        Console.SetCursorPosition(x, i);
                        Console.Write(n.Key);
                    }
                }
                n = n[0];
                x += 3;
            }
        }
    }
}
