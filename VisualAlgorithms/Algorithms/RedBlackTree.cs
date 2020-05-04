using System;
using System.Collections.Generic;

namespace VisualAlgorithms.Structures
{
    internal enum NodeColor
    {
        Black,
        Red
    }

    internal class RedBLackNode<TKey, TValue> : ITreeNode<TKey, TValue> where TKey : IComparable<TKey>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public RedBLackNode<TKey, TValue> Left { get; set; }
        public RedBLackNode<TKey, TValue> Right { get; set; }
        public RedBLackNode<TKey, TValue> Parent { get; set; }
        public NodeColor Color { get; set; }
        public bool IsLeaf { get; set; }

        public RedBLackNode(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public RedBLackNode()
        {
            Color = NodeColor.Black;
            IsLeaf = true;
        }
    }

    public class RedBlackTree<TKey, TValue> : ITree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private RedBLackNode<TKey, TValue> _root;
        public int Count { get; private set; }

        public RedBlackTree()
        {
            _root = new RedBLackNode<TKey, TValue>();
            Count = 0;
        }

        public void Add(TKey key, TValue value)
        {
            var node = new RedBLackNode<TKey, TValue>(key, value);
            var parent = new RedBLackNode<TKey, TValue>();
            var current = _root;

            while (!current.IsLeaf)
            {
                parent = current;

                if (node.Key.CompareTo(current.Key) < 0)
                    current = current.Left;
                else if (node.Key.CompareTo(current.Key) > 0)
                    current = current.Right;
                else
                    throw new ArgumentException("Item with same key has already been added");
            }

            node.Parent = parent;

            if (parent.IsLeaf)
                _root = node;
            else if (node.Key.CompareTo(parent.Key) < 0)
                parent.Left = node;
            else
                parent.Right = node;

            node.Left = new RedBLackNode<TKey, TValue>();
            node.Right = new RedBLackNode<TKey, TValue>();
            node.Color = NodeColor.Red;
            Count++;

            BalanceAfterAddition(node);
        }

        private void BalanceAfterAddition(RedBLackNode<TKey, TValue> node)
        {
            while (node.Parent.Color == NodeColor.Red)
            {
                if (node.Parent == node.Parent.Parent.Left)
                {
                    var uncle = node.Parent.Parent.Right;

                    if (uncle.Color == NodeColor.Red)
                    {
                        node.Parent.Color = NodeColor.Black;
                        uncle.Color = NodeColor.Black;
                        node.Parent.Parent.Color = NodeColor.Red;
                        node = node.Parent.Parent;
                    }
                    else
                    {
                        if (node == node.Parent.Right)
                        {
                            node = node.Parent;
                            RotateLeft(node);
                        }

                        node.Parent.Color = NodeColor.Black;
                        node.Parent.Parent.Color = NodeColor.Red;
                        RotateRight(node.Parent.Parent);
                    }
                }
                else
                {
                    var uncle = node.Parent.Parent.Left;

                    if (uncle.Color == NodeColor.Red)
                    {
                        node.Parent.Color = NodeColor.Black;
                        uncle.Color = NodeColor.Black;
                        node.Parent.Parent.Color = NodeColor.Red;
                        node = node.Parent.Parent;
                    }
                    else
                    {
                        if (node == node.Parent.Left)
                        {
                            node = node.Parent;
                            RotateRight(node);
                        }

                        node.Parent.Color = NodeColor.Black;
                        node.Parent.Parent.Color = NodeColor.Red;
                        RotateLeft(node.Parent.Parent);
                    }
                }
            }

            _root.Color = NodeColor.Black;
        }

        private void RotateLeft(RedBLackNode<TKey, TValue> node)
        {
            var current = node.Right;
            node.Right = current.Left;

            if (!current.Left.IsLeaf)
                current.Left.Parent = node;

            current.Parent = node.Parent;

            if (node.Parent.IsLeaf)
                _root = current;
            else
            {
                if (node == node.Parent.Left)
                    node.Parent.Left = current;
                else
                    node.Parent.Right = current;
            }

            current.Left = node;
            node.Parent = current;
        }

        private void RotateRight(RedBLackNode<TKey, TValue> node)
        {
            var current = node.Left;
            node.Left = current.Right;

            if (!current.Right.IsLeaf)
                current.Right.Parent = node;

            current.Parent = node.Parent;

            if (node.Parent.IsLeaf)
                _root = current;
            else
            {
                if (node == node.Parent.Right)
                    node.Parent.Right = current;
                else
                    node.Parent.Left = current;
            }

            current.Right = node;
            node.Parent = current;
        }

        public bool Remove(TKey key)
        {
            var node = Find(key, _root);

            if (node.IsLeaf)
                return false;

            Count--;

            RedBLackNode<TKey, TValue> linkedNode;
            RedBLackNode<TKey, TValue> workNode;

            if (node.Left.IsLeaf || node.Right.IsLeaf)
                workNode = node;
            else
                workNode = Successor(node);

            if (!workNode.Left.IsLeaf)
                linkedNode = workNode.Left;
            else
                linkedNode = workNode.Right;

            linkedNode.Parent = workNode.Parent;

            if (workNode.Parent.IsLeaf)
                _root = linkedNode;
            else
            {
                if (workNode == workNode.Parent.Left)
                    workNode.Parent.Left = linkedNode;
                else
                    workNode.Parent.Right = linkedNode;
            }

            if (workNode != node)
            {
                node.Key = workNode.Key;
                node.Value = workNode.Value;
            }

            if (workNode.Color == NodeColor.Black)
                BalanceAfterRemoving(linkedNode);

            return true;
        }

        private void BalanceAfterRemoving(RedBLackNode<TKey, TValue> node)
        {
            while (node != _root && node.Color == NodeColor.Black)
            {
                if (node == node.Parent.Left)
                {
                    var current = node.Parent.Right;

                    if (current.Color == NodeColor.Red)
                    {
                        current.Color = NodeColor.Black;
                        node.Parent.Color = NodeColor.Red;
                        RotateLeft(node.Parent);
                        current = node.Parent.Right;
                    }

                    if (current.Left.Color == NodeColor.Black && current.Right.Color == NodeColor.Black)
                    {
                        current.Color = NodeColor.Red;
                        node = node.Parent;
                    }
                    else
                    {
                        if (current.Right.Color == NodeColor.Black)
                        {
                            current.Left.Color = NodeColor.Black;
                            current.Color = NodeColor.Red;
                            RotateRight(current);
                            current = node.Parent.Right;
                        }

                        current.Color = node.Parent.Color;
                        node.Parent.Color = NodeColor.Black;
                        current.Right.Color = NodeColor.Black;
                        RotateLeft(node.Parent);
                        node = _root;
                    }
                }
                else
                {
                    var current = node.Parent.Left;

                    if (current.Color == NodeColor.Red)
                    {
                        current.Color = NodeColor.Black;
                        node.Parent.Color = NodeColor.Red;
                        RotateRight(node.Parent);
                        current = node.Parent.Left;
                    }

                    if (current.Left.Color == NodeColor.Black && current.Right.Color == NodeColor.Black)
                    {
                        current.Color = NodeColor.Red;
                        node = node.Parent;
                    }
                    else
                    {
                        if (current.Left.Color == NodeColor.Black)
                        {
                            current.Right.Color = NodeColor.Black;
                            current.Color = NodeColor.Red;
                            RotateLeft(current);
                            current = node.Parent.Left;
                        }

                        current.Color = node.Parent.Color;
                        node.Parent.Color = NodeColor.Black;
                        current.Left.Color = NodeColor.Black;
                        RotateRight(node.Parent);
                        node = _root;
                    }
                }
            }

            node.Color = NodeColor.Black;
        }

        public void Clear()
        {
            _root = new RedBLackNode<TKey, TValue>();
            Count = 0;
        }

        public bool Contains(TKey key)
        {
            var node = Find(key, _root);

            if (!node.IsLeaf)
                return true;

            return false;
        }

        private RedBLackNode<TKey, TValue> Find(TKey key, RedBLackNode<TKey, TValue> node)
        {
            if (node.IsLeaf || key.CompareTo(node.Key) == 0)
                return node;
            else
            {
                if (key.CompareTo(node.Key) < 0)
                    return Find(key, node.Left);
                else
                    return Find(key, node.Right);
            }
        }

        private RedBLackNode<TKey, TValue> Successor(RedBLackNode<TKey, TValue> node)
        {
            if (!node.Right.IsLeaf)
            {
                node = node.Right;

                while (!node.Left.IsLeaf)
                    node = node.Left;

                return node;
            }

            var parent = node.Parent;

            while (!parent.IsLeaf && node == parent.Right)
            {
                node = parent;
                parent = parent.Parent;
            }

            return parent;
        }

        public List<KeyValuePair<TKey, TValue>> GetPairs()
        {
            return Traverse(_root);
        }

        private List<KeyValuePair<TKey, TValue>> Traverse(RedBLackNode<TKey, TValue> node)
        {
            var nodes = new List<KeyValuePair<TKey, TValue>>();

            if (!node.IsLeaf)
            {
                nodes.AddRange(Traverse(node.Left));
                nodes.Add(new KeyValuePair<TKey, TValue>(node.Key, node.Value));
                nodes.AddRange(Traverse(node.Right));
            }

            return nodes;
        }
    }
}
