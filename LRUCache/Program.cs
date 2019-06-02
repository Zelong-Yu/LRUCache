using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        LRUCache<int,int> cache = new LRUCache<int,int>(2 /* capacity */ );

        cache.Add(1, 1);
        cache.Add(2, 2);
        Console.WriteLine(   cache.Get(1)    );   // returns 1
        cache.Add(3, 3);    // evicts key 2
        Console.WriteLine(cache.TryGet(2, out int val));       // returns false (not found)
        cache[4]= 4;    // evicts key 1
        Console.WriteLine(cache.TryGet(1, out  val));       // returns false (not found)
        Console.WriteLine(cache[3]);       // returns 3
        Console.WriteLine(cache.Get(4));       // returns 4
        Console.WriteLine( $"Capacity is {cache.Capacity}" );
        cache.Capacity = 10;
        Console.WriteLine($"New Capacity is {cache.Capacity}");
    }
}

public class LRUCache<TKey,TValue>
{
    private int capacity;
    private Dictionary<TKey, LinkedListNode<(TKey, TValue)>> map;
    private LinkedList<(TKey, TValue)> list;

    public int Capacity { get => capacity;
        set
        {
            if (value < 1) throw new ArgumentOutOfRangeException("Capacity","Capacity must be positive.");
            capacity = value;
            while (list.Count > Capacity)
            {
                var oldKey = list.First.Value.Item1;
                map.Remove(oldKey);
                list.RemoveFirst();
            }
        }
    }

    public LRUCache(int capacity)
    {
        map = new Dictionary<TKey, LinkedListNode<(TKey, TValue)>>();
        list = new LinkedList<(TKey, TValue)>();
        this.Capacity = capacity;
    }

    public TValue this[TKey index]
    {
        get { return Get(index); }
        set { Add(index, value); }
    }

    public TValue Get(TKey key)
    {
        if (map.ContainsKey(key))
        {
            var node = map[key];
            refresh(node);
            return node.Value.Item2;
        }
        else throw new KeyNotFoundException(); //not found
    }

    public bool TryGet(TKey key, out TValue value)
    {
        if (map.ContainsKey(key))
        {
            var node = map[key];
            refresh(node);
            value = node.Value.Item2;
            return true;
        }
        else //key not found
        {
            value = default;
            return false; 
        }
    }

    public void Add(TKey key, TValue value)
    {
        if (map.ContainsKey(key))
        {
            var node = map[key];
            node.Value = (key, value);
            refresh(node);
        }
        else
        {
            if (list.Count >= Capacity)
            {
                var oldKey = list.First.Value.Item1;
                map.Remove(oldKey);
                list.RemoveFirst();
            }
            var node = new LinkedListNode<(TKey, TValue)>((key, value));
            list.AddLast(node);
            map[key] = node;
        }
    }

    private void refresh(LinkedListNode<(TKey, TValue)> node)
    {
        if (list.Last == node) return;
        list.Remove(node);
        list.AddLast(node);
    }
}