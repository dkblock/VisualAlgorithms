using System;
using System.Collections.Generic;

namespace VisualAlgorithms.Structures
{
    internal class AVLNode<TKey, TValue> : ITreeNode<TKey, TValue> where TKey : IComparable<TKey>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public AVLNode<TKey, TValue> Left { get; set; }
        public AVLNode<TKey, TValue> Right { get; set; }
        public AVLNode<TKey, TValue> Parent { get; set; }
        public int Balance { get; set; }

        public AVLNode(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Left = null;
            Right = null;
            Parent = null;
            Balance = 0;
        }
    }

    public class AVLTree<TKey, TValue> : ITree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private AVLNode<TKey, TValue> _root;
        public int Count { get; private set; }

        public AVLTree()
        {
            Count = 0;
        }

        public void Add(TKey key, TValue value)
        {
            var node = new AVLNode<TKey, TValue>(key, value);

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

            node.Parent = parent;

            if (node.Key.CompareTo(parent.Key) < 0)
            {
                parent.Left = node;
                BalanceAfterAddition(node.Parent, 1);
            }
            else
            {
                parent.Right = node;
                BalanceAfterAddition(node.Parent, -1);
            }

            Count++;
        }

        private void BalanceAfterAddition(AVLNode<TKey, TValue> node, int balance)
        {
            while (node != null)
            {
                node.Balance += balance;
                balance = node.Balance;

                if (balance == 0)
                    return;

                if (balance == 2)
                {
                    if (node.Left.Balance == 1)
                        RotateRight(node);
                    else
                        RotateLeftRight(node);

                    return;
                }

                if (balance == -2)
                {
                    if (node.Right.Balance == -1)
                        RotateLeft(node);
                    else
                        RotateRightLeft(node);

                    return;
                }

                var parent = node.Parent;

                if (parent != null)
                {
                    if (parent.Left == node)
                        balance = 1;
                    else
                        balance = -1;
                }

                node = parent;
            }
        }

        private AVLNode<TKey, TValue> RotateRight(AVLNode<TKey, TValue> node)
        {
            var left = node.Left;
            var leftRight = left.Right;
            var parent = node.Parent;

            left.Parent = parent;
            left.Right = node;
            node.Left = leftRight;
            node.Parent = left;

            if (leftRight != null)
                leftRight.Parent = node;

            if (node == _root)
                _root = left;
            else if (parent.Left == node)
                parent.Left = left;
            else
                parent.Right = left;

            left.Balance--;
            node.Balance = -left.Balance;

            return left;
        }

        private AVLNode<TKey, TValue> RotateLeftRight(AVLNode<TKey, TValue> node)
        {
            var left = node.Left;
            var leftRight = left.Right;
            var parent = node.Parent;
            var leftRightRight = leftRight.Right;
            var leftRightLeft = leftRight.Left;

            leftRight.Parent = parent;
            node.Left = leftRightRight;
            left.Right = leftRightLeft;
            leftRight.Left = left;
            leftRight.Right = node;
            left.Parent = leftRight;
            node.Parent = leftRight;

            if (leftRightRight != null)
                leftRightRight.Parent = node;

            if (leftRightLeft != null)
                leftRightLeft.Parent = left;

            if (node == _root)
                _root = leftRight;
            else if (parent.Left == node)
                parent.Left = leftRight;
            else
                parent.Right = leftRight;

            if (leftRight.Balance == -1)
            {
                node.Balance = 0;
                left.Balance = 1;
            }
            else if (leftRight.Balance == 0)
            {
                node.Balance = 0;
                left.Balance = 0;
            }
            else
            {
                node.Balance = -1;
                left.Balance = 0;
            }

            leftRight.Balance = 0;

            return leftRight;
        }

        private AVLNode<TKey, TValue> RotateLeft(AVLNode<TKey, TValue> node)
        {
            var right = node.Right;
            var rightLeft = right.Left;
            var parent = node.Parent;

            right.Parent = parent;
            right.Left = node;
            node.Right = rightLeft;
            node.Parent = right;

            if (rightLeft != null)
                rightLeft.Parent = node;

            if (node == _root)
                _root = right;
            else if (parent.Right == node)
                parent.Right = right;
            else
                parent.Left = right;

            right.Balance++;
            node.Balance = -right.Balance;

            return right;
        }

        private AVLNode<TKey, TValue> RotateRightLeft(AVLNode<TKey, TValue> node)
        {
            var right = node.Right;
            var rightLeft = right.Left;
            var parent = node.Parent;
            var rightLeftLeft = rightLeft.Left;
            var rightLeftRight = rightLeft.Right;

            rightLeft.Parent = parent;
            node.Right = rightLeftLeft;
            right.Left = rightLeftRight;
            rightLeft.Right = right;
            rightLeft.Left = node;
            right.Parent = rightLeft;
            node.Parent = rightLeft;

            if (rightLeftLeft != null)
                rightLeftLeft.Parent = node;

            if (rightLeftRight != null)
                rightLeftRight.Parent = right;

            if (node == _root)
                _root = rightLeft;
            else if (parent.Right == node)
                parent.Right = rightLeft;
            else
                parent.Left = rightLeft;

            if (rightLeft.Balance == 1)
            {
                node.Balance = 0;
                right.Balance = -1;
            }
            else if (rightLeft.Balance == 0)
            {
                node.Balance = 0;
                right.Balance = 0;
            }
            else
            {
                node.Balance = 1;
                right.Balance = 0;
            }

            rightLeft.Balance = 0;

            return rightLeft;
        }

        public bool Remove(TKey key)
        {
            var node = _root;

            while (node != null)
            {
                if (key.CompareTo(node.Key) < 0)
                    node = node.Left;
                else if (key.CompareTo(node.Key) > 0)
                    node = node.Right;
                else
                {
                    var left = node.Left;
                    var right = node.Right;

                    if (left == null)
                    {
                        if (right == null)
                        {
                            if (node == _root)
                                _root = null;
                            else
                            {
                                var parent = node.Parent;

                                if (parent.Left == node)
                                {
                                    parent.Left = null;
                                    BalanceAfterRemoving(parent, -1);
                                }
                                else
                                {
                                    parent.Right = null;
                                    BalanceAfterRemoving(parent, 1);
                                }
                            }
                        }
                        else
                        {
                            Replace(node, right);
                            BalanceAfterRemoving(node, 0);
                        }
                    }
                    else if (right == null)
                    {
                        Replace(node, left);
                        BalanceAfterRemoving(node, 0);
                    }
                    else
                    {
                        var next = right;

                        if (next.Left == null)
                        {
                            var parent = node.Parent;

                            next.Parent = parent;
                            next.Left = left;
                            next.Balance = node.Balance;
                            left.Parent = next;

                            if (node == _root)
                                _root = next;
                            else
                            {
                                if (parent.Left == node)
                                    parent.Left = next;
                                else
                                    parent.Right = next;
                            }

                            BalanceAfterRemoving(next, 1);
                        }
                        else
                        {
                            while (next.Left != null)
                                next = next.Left;

                            var parent = node.Parent;
                            var nextParent = next.Parent;
                            var nextRight = next.Right;

                            if (nextParent.Left == next)
                                nextParent.Left = nextRight;
                            else
                                nextParent.Right = nextRight;

                            if (nextRight != null)
                                nextRight.Parent = nextParent;

                            next.Parent = parent;
                            next.Left = left;
                            next.Balance = node.Balance;
                            next.Right = right;
                            right.Parent = next;
                            left.Parent = next;

                            if (node == _root)
                                _root = next;
                            else
                            {
                                if (parent.Left == node)
                                    parent.Left = next;
                                else
                                    parent.Right = next;
                            }

                            BalanceAfterRemoving(nextParent, -1);
                        }
                    }

                    Count--;

                    return true;
                }
            }

            return false;
        }

        private void BalanceAfterRemoving(AVLNode<TKey, TValue> node, int balance)
        {
            while (node != null)
            {
                node.Balance += balance;
                balance = node.Balance;

                if (balance == 2)
                {
                    if (node.Left.Balance >= 0)
                    {
                        node = RotateRight(node);

                        if (node.Balance == -1)
                            return;
                    }
                    else
                        node = RotateLeftRight(node);
                }
                else if (balance == -2)
                {
                    if (node.Right.Balance <= 0)
                    {
                        node = RotateLeft(node);

                        if (node.Balance == 1)
                            return;
                    }
                    else
                        node = RotateRightLeft(node);
                }
                else if (balance != 0)
                    return;

                var parent = node.Parent;

                if (parent != null)
                {
                    if (parent.Left == node)
                        balance = -1;
                    else
                        balance = 1;
                }

                node = parent;
            }
        }

        private void Replace(AVLNode<TKey, TValue> node, AVLNode<TKey, TValue> sourceNode)
        {
            node.Key = sourceNode.Key;
            node.Value = sourceNode.Value;
            node.Left = sourceNode.Left;
            node.Right = sourceNode.Right;
            node.Balance = sourceNode.Balance;

            if (sourceNode.Left != null)
                sourceNode.Left.Parent = node;

            if (sourceNode.Right != null)
                sourceNode.Right.Parent = node;
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

        private AVLNode<TKey, TValue> Find(TKey key, AVLNode<TKey, TValue> node)
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

        private List<KeyValuePair<TKey, TValue>> Traverse(AVLNode<TKey, TValue> node)
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
