using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkipLists
{
    public class SkipList<T> : ICollection<T>
    {
        Node<T> header;

        public Comparer<T> Comparer { get; private set; }


        public SkipList(Comparer<T> comparer)
        {
            this.Comparer = comparer ?? Comparer<T>.Default;
        }

        public int Count
        {
            get
            {
                int count = 0;
                Node<T> node = header;
                while (node[0] != null)
                {
                    count++;
                    node = node[0];
                }
                return count;
            }
        }

        public bool IsReadOnly => false;

        private int PickLevel()
        {
            Random rand = new Random();
            int level = 0;
            int rnd = 1;
            while (rnd == 1)
            {
                rnd = rand.Next(2);
                level++;
                if (level > header.Level)
                {
                    rnd = 0;
                    CreateLevel();
                }
            }
            return level;
        }

        void CreateLevel()
        {
            header.IncreaseMaxLevel();
        }

        public void Add(T item)
        {
            int level = PickLevel();
            Last()[0] = new Node<T>(item, level);
            UpdateNexts();
        }

        void UpdateNexts()
        {
            Node<T>[] previouses = new Node<T>[header.Level];
            for (int i = 0; i < previouses.Length - 1; i++)
            {
                previouses[i] = header;
            }
            for (int i = 0; i < Count; i++)
            {
                Node<T> node = previouses[0];
                for (int j = 0; j < node.Level - 1; j++)
                {
                    if(node.Next[j] != null)
                    {
                        previouses[j][j] = node.Next[j];
                        previouses[j] = node.Next[j];
                    }
                }
            }
        }

        Node<T> Last()
        {
            Node<T> node = header;
            while (node[0] != null)
            {
                node = node[0];
            }
            return node;
        }

        public void Clear()
        {
            header = new Node<T>(1);
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node<T> current = header;
            while (current.Next != null)
            {
                yield return current.Key;
                current = current[0];
            }
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
