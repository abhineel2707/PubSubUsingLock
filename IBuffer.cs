namespace PubSubExample
{
    public interface IBuffer<T>
    {
        void Write(T value);
        T Read();
        bool IsEmpty { get; }
        bool IsFull { get; }
    }
}