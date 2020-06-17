using System;
namespace PubSubExample
{
    public class BlockingQueue<T> : IBuffer<T>
    {
        private T[] _blockingQueue;
        private int _start;
        private int _end;
        private int _capacity;

        public BlockingQueue(int capacity)
        {
            this._blockingQueue = new T[capacity];
            this._start = 0;
            this._end = 0;
            this._capacity = capacity;
        }
        // Producer
        public void Write(T value)
        {
            this._blockingQueue[this._end] = value;
            this._end = (this._end + 1) % this._capacity;
            if (this._end == this._start)
            {
                this._start = (this._start + 1) % _capacity;
            }
        }
        // Consumer
        public T Read()
        {
            T result = this._blockingQueue[this._start];
            this._start = (this._start + 1) % this._capacity;
            return result;
        }
        public int Capacity => this._capacity;

        public bool IsEmpty => this._start == this._end;
        public bool IsFull => (this._end + 1) % _capacity == this._start;
    }
}