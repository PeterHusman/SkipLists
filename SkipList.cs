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
        public Node<T> header;
        Random rand = new Random();

        public Comparer<T> Comparer { get; private set; }


        public SkipList(Comparer<T> comparer)
        {
            this.Comparer = comparer ?? Comparer<T>.Default;
            header = new Node<T>(1);
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

        private int PickLevel(int maxValue)
        {
            int level = 1;
            while (level < maxValue && rand.Next(2) == 0)
            {
                level++;
            }
            return level;
        }

        public void Add(T item)
        {
            Node<T> temp = new Node<T>(item, PickLevel(header.Height + 1));
            if (temp.Height > header.Height)
            {
                header.IncreaseMaxLevel();
            }
            int level = header.Height - 1;
            var curr = header;

            while (level >= 0)
            {
                if (curr[level] == null || Comparer.Compare(item, curr[level].Key) < 0)
                {
                    if (level < temp.Height)
                    {
                        temp[level] = curr[level];
                        curr[level] = temp;
                    }

                    level--;
                }
                else if (Comparer.Compare(item,curr[level].Key) > 0)
                {
                    curr = curr[level];

                }
                else if(Comparer.Compare(item,curr[level].Key) == 0)
                {
                    throw new ArgumentException("Attempted to add a duplicate key.");
                }
            }
        }

        //public void Add(T item)
        //{
        //    int level = header.Height - 1;
        //    Node<T> previous = null;
        //    Node<T> n = header;
        //    if (n[0] != null && Comparer.Compare(item, n[0].Key) < 0)
        //    {
        //        Node<T> node = new Node<T>(item, PickLevel(header.Height + 1));
        //        if(node.Height > header.Height)
        //        {
        //            header.IncreaseMaxLevel();
        //        }
        //        node[0] = n[0];
        //        n[0] = node;
        //        UpdateNexts();
        //        return;
        //    }
        //    while (true)
        //    {
        //        if (level < 0)
        //        {
        //            n[0] = new Node<T>(item, PickLevel(header.Height + 1));
        //            if (n[0].Height > header.Height)
        //            {
        //                header.IncreaseMaxLevel();
        //            }
        //            UpdateNexts();
        //            return;
        //        }
        //        if (n[level] == null)
        //        {
        //            level--;
        //            continue;
        //        }
        //        int comparison = Comparer.Compare(item, n[level].Key);
        //        if (comparison == 0)
        //        {
        //            throw new ArgumentException("Attempted to insert duplicate value.");
        //        }
        //        if (comparison >= 0)
        //        {
        //            previous = n;
        //            n = n[level];
        //            continue;
        //        }
        //        while (previous[0] != n)
        //        {
        //            previous = previous[0];
        //        }
        //        Node<T> node = new Node<T>(item, PickLevel(header.Height + 1));
        //        if (node.Height > header.Height)
        //        {
        //            header.IncreaseMaxLevel();
        //        }
        //        node.Next[0] = previous[0][0];
        //        previous[0][0] = node;
        //        UpdateNexts();
        //        return;
        //    }
        //    //int level = PickLevel();
        //    //Last()[0] = new Node<T>(item, level);
        //    //UpdateNexts();
        //}

        void UpdateNexts()
        {
            Node<T>[] previouses = new Node<T>[header.Height];
            for (int i = 0; i < previouses.Length - 1; i++)
            {
                previouses[i] = header;
            }
            for (int i = 0; i < Count; i++)
            {
                if (previouses[0] == null)
                {
                    continue;
                }
                Node<T> node = previouses[0];
                for (int j = 0; j < node.Height - 1; j++)
                {
                    if (node.Next[j] != null)
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

        Node<T> Find(T item)
        {
            int level = header.Height - 1;
            var curr = header;

            while (level >= 0)
            {
                if (curr[level] == null || Comparer.Compare(item, curr[level].Key) < 0)
                {
                    level--;
                }
                else if (Comparer.Compare(item, curr[level].Key) > 0)
                {
                    curr = curr[level];
                }
                else if (Comparer.Compare(item, curr[level].Key) == 0)
                {
                    return curr[level];
                }
            }
            return null;
        }

        public bool Contains(T item)
        {
            return Find(item) != null;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Node<T> n = header[0];
            while (n != null && arrayIndex < array.Length)
            {
                array[arrayIndex] = n.Key;
                n = n[0];
                arrayIndex++;
            }
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
            bool retVal = false;
            int level = header.Height - 1;
            var curr = header;

            while (level >= 0)
            {
                if (curr[level] == null || Comparer.Compare(item, curr[level].Key) < 0)
                {
                    level--;
                }
                else if (Comparer.Compare(item, curr[level].Key) > 0)
                {
                    curr = curr[level];
                }
                else if(Comparer.Compare(item, curr[level].Key) == 0)
                {
                    retVal = true;
                    curr[level] = curr[level][level];
                    level--;
                }
            }
            return retVal;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
