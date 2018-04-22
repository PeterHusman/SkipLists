using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkipLists
{
    public class Node<T>
    {
        public Node<T>[] Next;
        public int Height => Next.Length;

        public Node<T> this[int index]
        {
            get
            {
                return Next[index];
            }
            set
            {
                Next[index] = value;
            }

        }

        public Node(T key, int level)
        {
            Key = key;
            Next = new Node<T>[level];
        }

        public Node(int level)
        {
            Next = new Node<T>[level];
            Key = default(T);
        }

        public void IncreaseMaxLevel()
        {
            Node<T>[] temp = new Node<T>[Height + 1];
            Next.CopyTo(temp, 0);
            temp[Height] = null;
            Next = temp;
        }

        public T Key;
    }
}
