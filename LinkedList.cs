namespace DSA
{
    public class LinkedListNode<T>
    {
        public T Data { get; set; }
        public LinkedListNode<T>? Next { get; set; }

        public LinkedListNode(T data) => Data = data;
    }

    public class LinkedList<T>
    {
        public LinkedListNode<T>? Head { get; private set; }
        public int Count { get; private set; }

        public void AddLast(T data)
        {
            var newNode = new LinkedListNode<T>(data);
            if (Head == null) Head = newNode;
            else
            {
                var current = Head;
                while (current.Next != null)
                    current = current.Next;
                current.Next = newNode;
            }
            Count++;
        }

        public T? GetAt(int index)
        {
            if (index < 0 || index >= Count) return default;
            var current = Head;
            for (int i = 0; i < index; i++)
                current = current!.Next;
            return current!.Data;
        }
    }
}