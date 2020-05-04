using System;
using System.Collections.Generic;

namespace VisualAlgorithms.Structures
{
    internal class BinaryNode<TKey, TValue> : ITreeNode<TKey, TValue> where TKey : IComparable<TKey>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public BinaryNode<TKey, TValue> Left { get; set; }
        public BinaryNode<TKey, TValue> Right { get; set; }
        public BinaryNode<TKey, TValue> Parent { get; set; }

        public BinaryNode(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Left = null;
            Right = null;
            Parent = null;
        }
    }

    public class BinaryTree<TKey, TValue> : ITree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private BinaryNode<TKey, TValue> _root;
        public int Count { get; private set; }

        public BinaryTree()
        {
            Count = 0;
        }

        public void Add(TKey key, TValue value)
        {
            var node = new BinaryNode<TKey, TValue>(key, value);

            if (_root == null)
            {
                _root = node;
                Count++;
                return;
            }

            var current = _root;
            var parent = _root;

            while (current != null)
            {
                parent = current;

                if (node.Key.CompareTo(current.Key) < 0)
                    current = current.Left;
                else if (node.Key.CompareTo(current.Key) > 0)
                    current = current.Right;
                else if (node.Key.CompareTo(current.Key) == 0)
                    throw new ArgumentException("Item with same key has already been added");
            }

            if (node.Key.CompareTo(parent.Key) < 0)
                parent.Left = node;
            else
                parent.Right = node;

            node.Parent = parent;
            Count++;
        }

        public bool Remove(TKey key)
        {
            var node = Find(key, _root);

            if (node == null)
                return false;

            Count--;

            if (node == _root)
                _root = Next(node);

            if (node.Left == null && node.Right == null)
            {
                if (node.Parent.Left == node)
                    node.Parent.Left = null;
                else
                    node.Parent.Right = null;
            }
            else
            {
                if (node.Left == null || node.Right == null)
                {
                    if (node.Right != null)
                    {
                        node.Right.Parent = node.Parent;

                        if (node.Parent != null)
                        {
                            if (node.Parent.Left == node)
                                node.Parent.Left = node.Right;
                            else
                                node.Parent.Right = node.Right;
                        }
                    }
                    else
                    {
                        node.Left.Parent = node.Parent;

                        if (node.Parent != null)
                        {
                            if (node.Parent.Left == node)
                                node.Parent.Left = node.Left;
                            else
                                node.Parent.Right = node.Left;
                        }
                    }
                }
                else
                {
                    var next = Next(node);

                    if (next.Parent == node)
                    {
                        next.Left = node.Left;
                        node.Left.Parent = next;
                        next.Parent = node.Parent;

                        if (node == node.Parent.Left)
                            node.Parent.Left = next;
                        else if (node == node.Parent.Right)
                            node.Parent.Right = next;
                    }
                    else
                    {
                        if (next.Right != null)
                            next.Right.Parent = next.Parent;

                        next.Parent.Left = next.Right;
                        next.Right = node.Right;
                        next.Left = node.Left;
                        node.Left.Parent = next;
                        node.Right.Parent = next;
                        next.Parent = node.Parent;

                        if (node.Parent != null)
                        {
                            if (node == node.Parent.Left)
                                node.Parent.Left = next;
                            else if (node == node.Parent.Right)
                                node.Parent.Right = next;
                        }
                    }
                }
            }

            return true;
        }

        private BinaryNode<TKey, TValue> Next(BinaryNode<TKey, TValue> node)
        {
            var current = _root;
            BinaryNode<TKey, TValue> next = null;

            while (current != null)
            {
                if (node.Key.CompareTo(current.Key) < 0)
                {
                    next = current;
                    current = current.Left;
                }
                else
                    current = current.Right;
            }

            return next;
        }

        public void Clear()
        {
            _root = null;
            Count = 0;
        }

        public bool Contains(TKey key)
        {
            var node = Find(key, _root);

            if (node != null)
                return true;

            return false;
        }

        private BinaryNode<TKey, TValue> Find(TKey key, BinaryNode<TKey, TValue> node)
        {
            if (node == null || key.CompareTo(node.Key) == 0)
                return node;
            else
            {
                if (key.CompareTo(node.Key) < 0)
                    return Find(key, node.Left);
                else
                    return Find(key, node.Right);
            }
        }

        public List<KeyValuePair<TKey, TValue>> GetPairs()
        {
            return Traverse(_root);
        }

        private List<KeyValuePair<TKey, TValue>> Traverse(BinaryNode<TKey, TValue> node)
        {
            var nodes = new List<KeyValuePair<TKey, TValue>>();

            if (node != null)
            {
                nodes.AddRange(Traverse(node.Left));
                nodes.Add(new KeyValuePair<TKey, TValue>(node.Key, node.Value));
                nodes.AddRange(Traverse(node.Right));
            }

            return nodes;
        }
    }
}