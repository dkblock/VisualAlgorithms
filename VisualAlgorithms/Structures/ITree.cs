using System;
using System.Collections.Generic;

namespace VisualAlgorithms.Structures
{
    public interface ITree<TKey, TValue> where TKey : IComparable<TKey>
    {
        void Add(TKey key, TValue value);
        void Clear();
        int Count { get; }
        bool Contains(TKey key);
        List<KeyValuePair<TKey, TValue>> GetPairs();
        bool Remove(TKey key);
    }

    public interface ITreeNode<TKey, TValue> where TKey : IComparable<TKey>
    {
        TKey Key { get; set; }
        TValue Value { get; set; }
    }
}
