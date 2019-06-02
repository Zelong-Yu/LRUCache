using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        LRUCache cache = new LRUCache(2 /* capacity */ );

        cache.Put(1, 1);
        cache.Put(2, 2);
        Console.WriteLine(   cache.Get(1)    );   // returns 1
        cache.Put(3, 3);    // evicts key 2
        Console.WriteLine(cache.Get(2));       // returns -1 (not found)
        cache.Put(4, 4);    // evicts key 1
        Console.WriteLine(cache.Get(1));       // returns -1 (not found)
        Console.WriteLine(cache.Get(3));       // returns 3
        Console.WriteLine(cache.Get(4));       // returns 4
    }
}

public class LRUCache
{
    private int capacity;
    private Dictionary<int, LinkedListNode<(int, int)>> map;
    private LinkedList<(int, int)> list;
    public LRUCache(int capacity)
    {
        this.capacity = capacity;
        map = new Dictionary<int, LinkedListNode<(int, int)>>();
        list = new LinkedList<(int, int)>();
    }

    public int Get(int key)
    {
        if (map.ContainsKey(key))
        {
            var node = map[key];
            refresh(node);
            return node.Value.Item2;
        }
        else return -1;
    }

    public void Put(int key, int value)
    {
        if (map.ContainsKey(key))
        {
            var node = map[key];
            node.Value = (key, value);
            refresh(node);
        }
        else
        {
            if (list.Count >= capacity)
            {
                var oldKey = list.First.Value.Item1;
                map.Remove(oldKey);
                list.RemoveFirst();
            }
            var node = new LinkedListNode<(int, int)>((key, value));
            list.AddLast(node);
            map[key] = node;
        }
    }

    private void refresh(LinkedListNode<(int, int)> node)
    {
        if (list.Last == node) return;
        list.Remove(node);
        list.AddLast(node);
    }
}