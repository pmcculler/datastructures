/* The MIT License (MIT)

Copyright (c) 2015 Patrick McCuller

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStructures
{
    [Serializable]
    public class TrieNode : ITrie
    {
        private static char ARBITRARY_ROOT_VALUE = '_';

        // 'me' is the current value or values. Subclasses may take advantage of this structure to create compact representations
        // such as patricia or radix tries, as they are not limited to one value per node.
        protected char[] me = { ARBITRARY_ROOT_VALUE };  // arbitrary root node
        protected TrieNode[] next;
        protected bool end; // note: can save a lot of space by moving this to a static / separate 'ends' hash/flag set

        /// <summary>
        /// Exports the whole trie into the supplied list
        /// </summary>
        /// <param name="trail"></param>
        /// <param name="pile"></param>
        protected void Reconstruct(Stack<char[]> trail, List<string> pile)
        {
            char[] textHere = StringRepresentation().ToCharArray(); // roundabout, to let subclasses fiddle with storage

            if (textHere[0] != ARBITRARY_ROOT_VALUE)  // arbitrary root
                trail.Push(textHere);
            
            if (next == null)
            {
                pile.Add(BuildString(trail));
                // end of trail. build string and Add to pile.
                trail.Pop();
                return;
            }
            if (end)
                pile.Add(BuildString(trail));

            foreach (TrieNode node in next)
                node.Reconstruct(trail, pile);

            if (trail.Count > 0)
                trail.Pop();
        }

        public List<string> GetAllEntries()
        {
            List<string> items = new List<string>();
            Reconstruct(new Stack<char[]>(), items);
            return items;
        }

        public bool IsPresent(string word) { return IsPresent(word.ToCharArray(), 0); }
        public bool Contains(string word) { return IsPresent(word.ToCharArray(), 0); }
        public bool Contains(char[] word) { return IsPresent(word, 0); }

        /// <summary>
        /// Searches compacted and uncompacted trie.
        /// </summary>
        /// <returns></returns>
        public bool IsPresent(char[] word, int start)
        {
            if (word.Length-start == 0)
                return end;

            if (next == null)
                return false; // there's no further to look!

            foreach (TrieNode node in next)
            {
                if (node.me.Length > word.Length) // too short, can't match.
                    continue;

                bool found = false;
                for (int i = start, j = 0; j < node.me.Length; i++, j++)
                {
                    if (node.me[j] != word[i])
                    {
                        found = false;
                        break;
                    }
                    found = true;
                }
                if (found) // this node is the correct node.
                {
                    return node.IsPresent(word, start+1);
                }
            }
            return false;
        }

        public void Add(string word) { Add(word.ToCharArray(), 0); }

        /// <summary>
        /// Adds word to tree; only adds singles, does not maintain patricia/radix style.
        /// </summary>
        public void Add(char[] word, int start)
        {
            if (word.Length == start)
            {
                end = true;
                return;
            }
            char c = word[start];

            TrieNode target = null;
            if (next == null)
                next = new TrieNode[0];

            foreach (TrieNode node in next)
            {
                if (node.me[0] == c)
                {
                    target = node;
                    break;
                }
            }

            if (target == null)
            {
                target = new TrieNode {me = new[] {c}};
                TrieNode[] newNext = new TrieNode[next.Length + 1];  // expands the array of children
                Array.Copy(next, newNext, next.Length);
                newNext[next.Length] = target;
                next = newNext;
            }

            target.Add(word, start +1);
        }

        #region utility       
        protected static string BuildString(Stack<char[]> chunks)
        {
            var v = chunks.Reverse();
            StringBuilder sb = new StringBuilder();

            foreach (char[] cs in v)
                sb.Append(cs);

            return sb.ToString();
        }
        // This method is to let subclasses fiddle with storage.
        protected virtual string StringRepresentation()
        {
            return new string(me);
        }
        #endregion
    }
}
