using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class MyListEnum<T> : IEnumerator<T>
    {
        T[] items;
        int current;

        public MyListEnum(T[] items)
        {
            this.items = items;
            current = -1;
        }

        public T Current
        {
            get
            {
                return items[current];
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return items[current];
            }
        }

        public void Dispose()
        {
            //We have nothing to dispose
        }

        public bool MoveNext()
        {
            if(current < items.Length - 1)
            {
                current += 1;
                return true;
            }
            return false;
        }
        
        public void Reset()
        {
            //Because we're dealing with a simple array, we have to set current to -1 in order for the first MoveNext-call to point to the first element
            current = -1;
        }
    }

    class MyList <T>  : IEnumerable<T>
    {
        T[] items = new T[0];

        

        public void Add(T newItem)
        {
            T[] temp = new T[items.Length + 1];

            for (int i = 0; i < items.Length; i++)
            {
                temp[i] = items[i];
            }

            temp[items.Length] = newItem;

            items = temp;
        }

        public void Remove(T item)
        {
            if(items.Length == 0)
            {
                return;
            }

            T[] temp = new T[items.Length - 1];

            int x = 0;

            for (int i = 0; i < items.Length; i++)
            {
                if(items[i].Equals(item))
                {
                    continue;
                }

                temp[x] = items[i];
                x++;
            }
            items = temp;
        }

        public void Clear()
        {
            items = new T[0];
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new MyListEnum<T>(items);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new MyListEnum<T>(items);
        }
    }
}
