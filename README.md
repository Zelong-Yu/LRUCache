# LRUCache (Least recently used Cache)
Use Dictionary&lt;Key,Value> and (Doubly) LinkedList to achieve O(1) read/add with set capacity

---------------------------------
Usage:

LRUCache cache = new LRUCache( 2 /* capacity */ );
The cache is initialized with a positive capacity.

__Get(key)__ - Get the value (will always be positive) of the key if the key exists in the cache, otherwise return -1.

__Put(key, value)__ - Set or insert the value if the key is not already present. When the cache reached its capacity, it should invalidate the least recently used item before inserting a new item.

__indexer:__ cache[somekey] 

https://en.wikipedia.org/wiki/Cache_replacement_policies#LRU
