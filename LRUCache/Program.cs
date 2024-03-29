﻿using System;
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
        Console.WriteLine(   cache.Get(1)    );   // returns 1 and update 1 to most recently visited at the back end
        cache.Add(3, 3);    // adding 3 should evict key 2 since 2 is least recently visited
        Console.WriteLine(cache.TryGet(2, out int val));       // returns false (not found)
        cache[4]= 4;    // this should  evicts key 1
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
    private Dictionary<TKey, LinkedListNode<(TKey, TValue)>> map; //to ensure O(1) Get, we need a Dictionary
    private LinkedList<(TKey, TValue)> list; //most recently visited element is always at the end of list

    public int Capacity { get => capacity;
        set
        {
            if (value < 1) throw new ArgumentOutOfRangeException("Capacity","Capacity must be positive.");
            capacity = value;
            //while the list is over capacity, keep deleting least recently used elements
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
            refresh(node);//put the most recently visited node to end of list
            return node.Value.Item2;
        }
        else throw new KeyNotFoundException(); //not found
    }

    public bool TryGet(TKey key, out TValue value)
    {
        if (map.ContainsKey(key))
        {
            var node = map[key];
            refresh(node);//put the most recently visited node to end of list
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
        //Update value if key is present
        if (map.ContainsKey(key))
        {
            var node = map[key];
            node.Value = (key, value);
            refresh(node);
        }
        else //Add the new (key,value) to the map and the end of list 
        {
            //if capacity has been reached, delete the head of list, which is the least-recently-used element
            if (list.Count >= Capacity)
            {
                var oldKey = list.First.Value.Item1;
                map.Remove(oldKey);
                list.RemoveFirst();
            }
            var node = new LinkedListNode<(TKey, TValue)>((key, value));
            list.AddLast(node);//add the new node to end of list
            map[key] = node;
        }
    }

    public TKey MostRecentVisitedKey()
    {
        return list.Last.Value.Item1;
    }

    public TValue MostRecentVisitedValue()
    {
        return list.Last.Value.Item2;
    }

    //extrat the most recently visited node and put it to end of list 
    private void refresh(LinkedListNode<(TKey, TValue)> node)
    {
        if (list.Last == node) return;//no need to update if the last node is already the most recently visited one
        list.Remove(node);
        list.AddLast(node);
    }
}