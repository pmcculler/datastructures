using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStructures
{
    [Serializable]
    public class Trie : ITrie
    {
        public static int SingleNodes;
        public static int DictionaryNodes;
        public static int PNodes;
        public static int SmallDictionaryNodes;
        public static int TerminalNodes;
        public static Dictionary<int, int> SizesOfDictionaryNodes = new Dictionary<int, int>();
        public static Dictionary<int, int> SizeOfPNodes = new Dictionary<int, int>();

        protected interface INode
        {
            void Reconstruct(Stack<char> trail, List<string> pile);
            bool IsPresent(char[] word, int start);
            void Add(char[] word, int start);
            bool End { get; set; }
            INode Split();
            void Visit();
        }

        [Serializable]
        protected class MustBeSplitException : Exception {}

        [Serializable]
        protected class PNode : INode
        {
            public char[] Chars;
            public bool End { get { return true; }
                // ReSharper disable once ValueParameterNotUsed
                set
                {
                    ;
                }
            }
            #region Statistics
            public void Visit()
            {
                PNodes++;
                if (SizeOfPNodes.ContainsKey(Chars.Length))
                {
                    SizeOfPNodes[Chars.Length] = SizeOfPNodes[Chars.Length] + 1;
                }
                else
                {
                    SizeOfPNodes[Chars.Length] = 1;
                }
            }
            #endregion
            public PNode(char[] arr, int start)
            {
                Chars = new char[arr.Length-start];
                for (int i = 0, j = start; i < Chars.Length && j < arr.Length; i++, j++)
                {
                    Chars[i] = arr[j];
                }
            }

            public void Reconstruct(Stack<char> trail, List<string> pile)
            {
                foreach (char c in Chars)
                {
                    trail.Push(c);
                }
                pile.Add(BuildString(trail));
                foreach (char ignored in Chars)
                {
                    trail.Pop();
                }
            }

            public bool IsPresent(char[] word, int start)
            {
                if (word.Length - start != Chars.Length)
                {
                    return false;
                }

                for (int i = 0, j = start; i < Chars.Length && j < word.Length - start; i++, j++)
                {
                    if (word[j] != Chars[i])
                    {
                        return false;
                    }
                }
                return true;
            }

            public void Add(char[] word, int start)
            {
                if (Chars.Length != word.Length - start)
                {
                    throw new MustBeSplitException();
                }
                for (int i = 0, j = start; i < Chars.Length && j < word.Length; i++, j++)
                {
                    if (word[j] != Chars[i])
                    {
                        throw new MustBeSplitException();
                    }
                }
            }

            // breaks PNode into singlechildnode + remainder
            public INode Split()
            {
                INode next = new SingleChildNode();
                next.Add(Chars, 0);
                return next;
            }
        }

        [Serializable]
        protected class TerminalNode : INode
        {
            public void Visit()
            {
                TerminalNodes++;
            }

            public void Reconstruct(Stack<char> trail, List<string> pile)
            {
                pile.Add(BuildString(trail));
            }

            public bool End
            {
                get { return true; }
                set { }
            }

            public INode Split()
            {
                INode node = new SingleChildNode();
                node.End = true;
                return node;
            }

            public void Add(char[] word, int start)
            {
                if (word.Length - start == 0)
                {
                    return;
                }
                throw new MustBeSplitException();
            }

            public bool IsPresent(char[] word, int start)
            {
                return word.Length - start <= 0;
            }
        }

        [Serializable]
        protected class SingleChildNode : INode
        {
            public char NextChar;
            public INode NextNode;
            public bool End { get; set; }

            #region Statistics
            public void Visit()
            {
                SingleNodes++;
                NextNode?.Visit();
            }
            #endregion
            public void Reconstruct(Stack<char> trail, List<string> pile)
            {
                if (NextNode == null)
                {
                    pile.Add(BuildString(trail));
                    // end of trail. build string and Add to pile.
                    return;
                }
                if (End)
                {
                    pile.Add(BuildString(trail));
                }

                trail.Push(NextChar);
                NextNode.Reconstruct(trail, pile);
                trail.Pop();
            }

            public bool IsPresent(char[] word, int start)
            {
                if (word.Length - start == 0)
                {
                    return End;
                }

                if (NextNode == null)
                {
                    return false; // there's no further to look!
                }

                return NextChar == word[start] && NextNode.IsPresent(word, start + 1);
            }

            public void Add(char[] word, int start)
            {
                if (word.Length - start <= 0)
                {
                    End = true;
                    return;
                }

                char c = word[start];

                if (NextNode == null)
                {
                    NextChar = c;
                    if (word.Length - start == 1)
                    {
                        NextNode = new TerminalNode();
                    }
                    else if (word.Length - start < 3)
                    {
                        NextNode = new SingleChildNode();
                    }
                    else
                    {
                        NextNode = new PNode(word, start + 1);
                    }
                }
                else if (NextChar != c)
                {
                    throw new MustBeSplitException();
                }

                try
                {
                    NextNode.Add(word, start + 1);
                }
                catch (MustBeSplitException)
                {
                    NextNode = NextNode.Split();
                    Add(word, start);
                }
            }

            public INode Split()
            {
                return new SmallDictionaryNode(this);
            }
        }

        [Serializable]
        protected class DictionaryNode : INode
        {
            protected Dictionary<char, INode> Children;
            public bool End { get; set; }
            // note: can save a lot of space by moving this to a static / separate 'ends' hash/flag set

            public DictionaryNode(SingleChildNode source)
            {
                Children = new Dictionary<char, INode> { [source.NextChar] = source.NextNode };
                End = source.End;
            }

            public DictionaryNode(SmallDictionaryNode source)
            {
                Children = new Dictionary<char, INode>
                {
                    [source.NextChar1] = source.NextNode1,
                    [source.NextChar2] = source.NextNode2,
                    [source.NextChar3] = source.NextNode3,
                    [source.NextChar4] = source.NextNode4
                };
                End = source.End;
            }

            public DictionaryNode() { }

            #region Statistics
            public void Visit()
            {
                DictionaryNodes++;
                if (Children != null)
                {
                    foreach (INode child in Children.Values)
                    {
                        child.Visit();
                    }
                    if (SizesOfDictionaryNodes.ContainsKey(Children.Count))
                    {
                        SizesOfDictionaryNodes[Children.Count] = SizesOfDictionaryNodes[Children.Count] + 1;
                    }
                    else
                    {
                        SizesOfDictionaryNodes[Children.Count] = 1;
                    }
                }
                else
                {
                    if (SizesOfDictionaryNodes.ContainsKey(0))
                    {
                        SizesOfDictionaryNodes[0] = SizesOfDictionaryNodes[0] + 1;
                    }
                    else
                    {
                        SizesOfDictionaryNodes[0] = 1;
                    }
                }

            }
            #endregion
            /// <summary>
            /// Exports the whole trie into the supplied list
            /// </summary>
            public void Reconstruct(Stack<char> trail, List<string> pile)
            {
                if (Children == null)
                {
                    pile.Add(BuildString(trail));
                    // end of trail. build string and Add to pile.
                    return;
                }
                if (End)
                {
                    pile.Add(BuildString(trail));
                }

                foreach (KeyValuePair<char, INode> kvp in Children)
                {
                    trail.Push(kvp.Key);
                    kvp.Value.Reconstruct(trail, pile);
                    trail.Pop();
                }
            }

            /// <summary>
            /// Searches compacted and uncompacted trie.
            /// </summary>
            public bool IsPresent(char[] word, int start)
            {
                if (word.Length - start == 0)
                {
                    return End;
                }

                if (Children == null)
                {
                    return false; // there's no further to look!
                }
                INode node;

                if (!Children.TryGetValue(word[start], out node))
                {
                    return false;
                }

                return node.IsPresent(word, start + 1);
            }

            public void Add(string word)
            {
                Add(word.ToCharArray(), 0);
            }

            public void Add(char[] word, int start)
            {
                if (word.Length - start == 0)
                {
                    End = true;
                    return;
                }
                char c = word[start];

                if (Children == null)
                {
                    Children = new Dictionary<char, INode>();
                }
                INode target;
                if (!Children.TryGetValue(c, out target))
                {
                    target = new SingleChildNode();
                    Children[c] = target;
                }
                try
                {
                    target.Add(word, start + 1);
                }
                catch (MustBeSplitException)
                {
                    Children[c] = target.Split();
                    Add(word, start);
                }
            }

            public INode Split()
            {
                return this;
            }
            public bool IsPresent(string word) { return IsPresent(word.ToCharArray(), 0); }
            public bool Contains(string word) { return IsPresent(word.ToCharArray(), 0); }
            public bool Contains(char[] word) { return IsPresent(word, 0); }
        }

        [Serializable]
        protected class SmallDictionaryNode : INode
        {
            public Char NextChar1;
            public Char NextChar2;
            public Char NextChar3;
            public Char NextChar4;
            public INode NextNode1;
            public INode NextNode2;
            public INode NextNode3;
            public INode NextNode4;

            public bool End { get; set; }
            // note: can save a lot of space by moving this to a static / separate 'ends' hash/flag set

            public SmallDictionaryNode(SingleChildNode source)
            {
                NextChar1 = source.NextChar;
                NextNode1 = source.NextNode;
                End = source.End;
            }

            public SmallDictionaryNode() {}
            #region Statistics
            public void Visit()
            {
                SmallDictionaryNodes++;
                NextNode1?.Visit();
                NextNode2?.Visit();
                NextNode3?.Visit();
                NextNode4?.Visit();
            }
            #endregion
            /// <summary>
            /// Exports the whole trie into the supplied list
            /// </summary>
            public void Reconstruct(Stack<char> trail, List<string> pile)
            {
                if (End)
                {
                    pile.Add(BuildString(trail));
                }

                if (NextNode1 != null)
                {
                    trail.Push(NextChar1);
                    NextNode1.Reconstruct(trail, pile);
                    trail.Pop();
                }
                if (NextNode2 != null)
                {
                    trail.Push(NextChar2);
                    NextNode2.Reconstruct(trail, pile);
                    trail.Pop();
                }
                if (NextNode3 != null)
                {
                    trail.Push(NextChar3);
                    NextNode3.Reconstruct(trail, pile);
                    trail.Pop();
                }
                if (NextNode4 != null)
                {
                    trail.Push(NextChar4);
                    NextNode4.Reconstruct(trail, pile);
                    trail.Pop();
                }
            }

            /// <summary>
            /// Searches compacted and uncompacted trie.
            /// </summary>
            public bool IsPresent(char[] word, int start)
            {
                if (word.Length - start == 0)
                {
                    return End;
                }
                if (NextNode1 != null && NextChar1 == word[start])
                {
                    return NextNode1.IsPresent(word, start + 1);
                }
                if (NextNode2 != null && NextChar2 == word[start])
                {
                    return NextNode2.IsPresent(word, start + 1);
                }
                if (NextNode3 != null && NextChar3 == word[start])
                {
                    return NextNode3.IsPresent(word, start + 1);
                }
                if (NextNode4 != null && NextChar4 == word[start])
                {
                    return NextNode4.IsPresent(word, start + 1);
                }
                return false;
            }

            public void Add(string word)
            {
                Add(word.ToCharArray(), 0);
            }

            public void Add(char[] word, int start)
            {
                if (word.Length - start == 0)
                {
                    End = true;
                    return;
                }
                char c = word[start];

                INode next = null;
                if (NextNode1 != null && NextChar1 == c)
                {
                    next = NextNode1;
                }
                else if (NextNode2 != null && NextChar2 == c)
                {
                    next = NextNode2;
                }
                else if (NextNode3 != null && NextChar3 == c)
                {
                    next = NextNode3;
                }
                else if (NextNode4 != null && NextChar4 == c)
                {
                    next = NextNode4;
                }

                if (next == null)
                {
                    if (NextNode1 == null)
                    {
                        NextChar1 = c;
                        NextNode1 = new SingleChildNode();
                        next = NextNode1;
                    }
                    else if (NextNode2 == null)
                    {
                        NextChar2 = c;
                        NextNode2 = new SingleChildNode();
                        next = NextNode2;
                    }
                    else if (NextNode3 == null)
                    {
                        NextChar3 = c;
                        NextNode3 = new SingleChildNode();
                        next = NextNode3;
                    }
                    else if (NextNode4 == null)
                    {
                        NextChar4 = c;
                        NextNode4 = new SingleChildNode();
                        next = NextNode4;
                    }
                }
                if (next == null)
                {
                    throw new MustBeSplitException();
                }
                try
                {
                    next.Add(word, start + 1);
                }
                catch (MustBeSplitException)
                {
                    INode replacement = next.Split();
                    if (NextChar1 == c)
                        NextNode1 = replacement;
                    else if (NextChar2 == c)
                        NextNode2 = replacement;
                    else if (NextChar3 == c)
                        NextNode3 = replacement;
                    else if (NextChar4 == c)
                        NextNode4 = replacement;
                    Add(word, start);
                }
            }

            public INode Split()
            {
                return new DictionaryNode(this);
            }
            public bool IsPresent(string word) { return IsPresent(word.ToCharArray(), 0); }
            public bool Contains(string word) { return IsPresent(word.ToCharArray(), 0); }
            public bool Contains(char[] word) { return IsPresent(word, 0); }
        }

        protected DictionaryNode _root = new DictionaryNode();

        public void Visit()
        {
            _root.Visit();
        }

        public void Add(string word) { _root.Add(word); }
        public bool Contains(string word) { return _root.IsPresent(word.ToCharArray(), 0); }

        public List<string> GetAllEntries()
        {
            List<string> items = new List<string>();
            _root.Reconstruct(new Stack<char>(), items);
            return items;
        }

        #region utility       
        protected static string BuildString(Stack<char> chunks)
        {
            var v = chunks.Reverse();
            StringBuilder sb = new StringBuilder();

            foreach (char c in v)
            {
                sb.Append(c);
            }
            return sb.ToString();
        }
        #endregion
    }
}
