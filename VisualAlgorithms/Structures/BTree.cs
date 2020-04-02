using System;
using System.Collections.Generic;
using System.Linq;

namespace VisualAlgorithms.Structures
{
    internal class BPage<TKey, TValue> where TKey : IComparable<TKey>
    {
        private int _degree;

        public List<BPage<TKey, TValue>> Children { get; set; }
        public List<BNode<TKey, TValue>> Nodes { get; set; }

        public BPage(int degree)
        {
            _degree = degree;
            Children = new List<BPage<TKey, TValue>>(degree);
            Nodes = new List<BNode<TKey, TValue>>(degree);
        }

        public bool IsLeaf
        {
            get
            {
                return Children.Count == 0 ? true : false;
            }
        }

        public bool IsFull
        {
            get
            {
                return Nodes.Count == (2 * _degree) - 1 ? true : false;
            }
        }

        public bool IsReachedMin
        {
            get
            {
                return Nodes.Count == _degree - 1 ? true : false;
            }
        }
    }

    internal class BNode<TKey, TValue> : ITreeNode<TKey, TValue> where TKey : IComparable<TKey>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public BNode(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    public class BTree<TKey, TValue> : ITree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private BPage<TKey, TValue> _root;

        public int Count { get; private set; }
        public int Degree { get; private set; }
        public int Height { get; private set; }

        public BTree(int degree)
        {
            if (degree < 2)
                throw new ArgumentException("BTree degree must be at least 2");

            _root = new BPage<TKey, TValue>(degree);
            Count = 0;
            Degree = degree;
            Height = 1;
        }

        public void Add(TKey key, TValue value)
        {
            var node = new BNode<TKey, TValue>(key, value);
            Count++;

            if (!_root.IsFull)
            {
                Add(_root, node);
                return;
            }

            var oldRoot = _root;

            _root = new BPage<TKey, TValue>(Degree);
            _root.Children.Add(oldRoot);
            SplitChild(_root, 0, oldRoot);
            Add(_root, node);
            Height++;
        }

        private void Add(BPage<TKey,TValue> page, BNode<TKey,TValue> node)
        {
            var positionToInsert = page.Nodes.TakeWhile(x => node.Key.CompareTo(x.Key) >= 0).Count();

            if (page.IsLeaf)
            {
                page.Nodes.Insert(positionToInsert, node);
                return;
            }

            var child = page.Children[positionToInsert];

            if (child.IsFull)
            {
                SplitChild(page, positionToInsert, child);

                if (node.Key.CompareTo(page.Nodes[positionToInsert].Key) > 0)
                    positionToInsert++;
            }

            Add(page.Children[positionToInsert], node);
        }

        private void SplitChild(BPage<TKey, TValue> parentPage, int pageToSplitIndex, BPage<TKey, TValue> pageToSplit)
        {
            var newPage = new BPage<TKey, TValue>(Degree);

            parentPage.Nodes.Insert(pageToSplitIndex, pageToSplit.Nodes[Degree - 1]);
            parentPage.Children.Insert(pageToSplitIndex + 1, newPage);
            newPage.Nodes.AddRange(pageToSplit.Nodes.GetRange(Degree, Degree - 1));
            pageToSplit.Nodes.RemoveRange(Degree - 1, Degree);

            if (!pageToSplit.IsLeaf)
            {
                newPage.Children.AddRange(pageToSplit.Children.GetRange(Degree, Degree));
                pageToSplit.Children.RemoveRange(Degree, Degree);
            }
        }

        public bool Remove(TKey key)
        {
            if (!Contains(key))
                return false;

            Remove(_root, key);
            Count--;

            if (_root.Nodes.Count == 0 && !_root.IsLeaf)
            {
                _root = _root.Children.Single();
                Height--;
            }

            return true;
        }

        private void Remove(BPage<TKey,TValue> page, TKey key)
        {
            var index = page.Nodes.TakeWhile(x => key.CompareTo(x.Key) > 0).Count();

            if (index < page.Nodes.Count && page.Nodes[index].Key.CompareTo(key) == 0)
            {
                RemoveFromPage(page, key, index);
                return;
            }

            if (!page.IsLeaf)
                RemoveFromSubtree(page, key, index);
        }

        private void RemoveFromSubtree(BPage<TKey, TValue> parentPage, TKey key, int subtreeIndexInPage)
        {
            var child = parentPage.Children[subtreeIndexInPage];

            if (child.IsReachedMin)
            {
                var leftIndex = subtreeIndexInPage - 1;
                var left = subtreeIndexInPage > 0 ? parentPage.Children[leftIndex] : null;

                int rightIndex = subtreeIndexInPage + 1;
                var right = subtreeIndexInPage < parentPage.Children.Count - 1 ? parentPage.Children[rightIndex] : null;

                if (left != null && left.Nodes.Count > Degree - 1)
                {
                    child.Nodes.Insert(0, parentPage.Nodes[subtreeIndexInPage-1]);
                    parentPage.Nodes[subtreeIndexInPage-1] = left.Nodes.Last();
                    left.Nodes.RemoveAt(left.Nodes.Count - 1);

                    if (!left.IsLeaf)
                    {
                        child.Children.Insert(0, left.Children.Last());
                        left.Children.RemoveAt(left.Children.Count - 1);
                    }
                }
                else if (right != null && right.Nodes.Count > this.Degree - 1)
                {
                    child.Nodes.Add(parentPage.Nodes[subtreeIndexInPage]);
                    parentPage.Nodes[subtreeIndexInPage] = right.Nodes.First();
                    right.Nodes.RemoveAt(0);

                    if (!right.IsLeaf)
                    {
                        child.Children.Add(right.Children.First());
                        right.Children.RemoveAt(0);
                    }
                }
                else
                {
                    if (left != null)
                    {
                        child.Nodes.Insert(0, parentPage.Nodes[subtreeIndexInPage-1]);

                        var oldNodes = child.Nodes;
                        child.Nodes = left.Nodes;
                        child.Nodes.AddRange(oldNodes);

                        if (!left.IsLeaf)
                        {
                            var oldChildren = child.Children;
                            child.Children = left.Children;
                            child.Children.AddRange(oldChildren);
                        }

                        parentPage.Children.RemoveAt(leftIndex);
                        parentPage.Nodes.RemoveAt(subtreeIndexInPage-1);
                    }
                    else
                    {
                        child.Nodes.Add(parentPage.Nodes[subtreeIndexInPage]);
                        child.Nodes.AddRange(right.Nodes);

                        if (!right.IsLeaf)
                        {
                            child.Children.AddRange(right.Children);
                        }

                        parentPage.Children.RemoveAt(rightIndex);
                        parentPage.Nodes.RemoveAt(subtreeIndexInPage);
                    }
                }
            }

            Remove(child, key);
        }

        private void RemoveFromPage(BPage<TKey,TValue> page, TKey key, int keyIndexInPage)
        {
            if (page.IsLeaf)
            {
                page.Nodes.RemoveAt(keyIndexInPage);
                return;
            }

            var predecessorChild = page.Children[keyIndexInPage];

            if (predecessorChild.Nodes.Count >= Degree)
            {
                var predecessorNode = GetLastNode(predecessorChild);
                Remove(predecessorChild, predecessorNode.Key);
                page.Nodes[keyIndexInPage] = predecessorNode;
            }
            else
            {
                var successorChild = page.Children[keyIndexInPage + 1];

                if (successorChild.Nodes.Count >= Degree)
                {
                    var successorNode = GetFirstNode(successorChild);
                    Remove(successorChild, successorNode.Key);
                    page.Nodes[keyIndexInPage] = successorNode;
                }
                else
                {
                    predecessorChild.Nodes.Add(page.Nodes[keyIndexInPage]);
                    predecessorChild.Nodes.AddRange(successorChild.Nodes);
                    predecessorChild.Children.AddRange(successorChild.Children);
                    page.Nodes.RemoveAt(keyIndexInPage);
                    page.Children.RemoveAt(keyIndexInPage + 1);

                    Remove(predecessorChild, key);
                }
            }
        }

        private BNode<TKey, TValue> GetLastNode(BPage<TKey,TValue> page)
        {
            if (page.IsLeaf)
                return page.Nodes.Last();

            return GetLastNode(page.Children.Last());
        }

        private BNode<TKey, TValue> GetFirstNode(BPage<TKey, TValue> page)
        {
            if (page.IsLeaf)
                return page.Nodes.First();

            return GetFirstNode(page.Children.First());
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

        private BNode<TKey, TValue> Find(TKey key, BPage<TKey, TValue> page)
        {
            var index = page.Nodes.TakeWhile(x => key.CompareTo(x.Key) > 0).Count();

            if (index < page.Nodes.Count && page.Nodes[index].Key.CompareTo(key) == 0)
                return page.Nodes[index];

            if (page.IsLeaf)
                return null;

            return Find(key, page.Children[index]);
        }

        public List<KeyValuePair<TKey, TValue>> GetPairs()
        {
            return Traverse(_root);
        }

        private List<KeyValuePair<TKey, TValue>> Traverse(BPage<TKey, TValue> page)
        {
            var nodes = new List<KeyValuePair<TKey, TValue>>();

            foreach (var child in page.Children)
                nodes.AddRange(Traverse(child));

            foreach (var node in page.Nodes)
                nodes.Add(new KeyValuePair<TKey, TValue>(node.Key, node.Value));

            return nodes;
        }
    }
}
