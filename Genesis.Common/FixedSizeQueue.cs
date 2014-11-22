using System.Collections.Generic;

namespace Genesis.Common
{
    public class FixedSizedQueue<T>
    {
        readonly Queue<T> queue = new Queue<T>();

        public Queue<T> Queue
        {
            get
            {
                return queue;
            }
        }

        public int Size { get; private set; }

        public FixedSizedQueue(int size)
        {
            Size = size;
        }

        public void Enqueue(T obj)
        {
            queue.Enqueue(obj);
            lock (this)
            {
                while (queue.Count > Size)
                {
                    T outObj = queue.Dequeue();
                }
            }
        }

        public void Clear()
        {
            this.queue.Clear();
        }
    }            
}
