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
using System.Threading.Tasks;

namespace DataStructures
{
    [Serializable]
    public class TrieNode
    {
        private static char ARBITRARY_ROOT_VALUE = '_';

        // 'me' is the current value or values. Subclasses may take advantage of this structure to create compact representations
        // such as patricia or radix tries, as they are not limited to one value per node.
        protected char[] me = new char[] { ARBITRARY_ROOT_VALUE };  // arbitrary root node
        protected TrieNode[] next = null;
        protected bool end = false; // note: can save a lot of space by moving this to a static / separate 'ends' hash/flag set

        /// <summary>
        /// Exports the whole trie into the supplied list
        /// </summary>
        /// <param name="trail"></param>
        /// <param name="pile"></param>
        protected void reconstruct(Stack<char[]> trail, List<string> pile)
        {
            char[] textHere = stringRepresentation().ToCharArray(); // roundabout, to let subclasses fiddle with storage

            if (textHere[0] != ARBITRARY_ROOT_VALUE)  // arbitrary root
                trail.Push(textHere);
            
            if (next == null)
            {
                pile.Add(buildString(trail));
                // end of trail. build string and add to pile.
                trail.Pop();
                return;
            }
            if (end)
                pile.Add(buildString(trail));

            foreach (TrieNode node in next)
                node.reconstruct(trail, pile);

            if (trail.Count > 0)
                trail.Pop();
        }

        public List<string> getAllEntries()
        {
            List<string> items = new List<string>();
            reconstruct(new Stack<char[]>(), items);
            return items;
        }

        public bool isPresent(string word) { return isPresent(word.ToCharArray()); }
        public bool Contains(string word) { return isPresent(word.ToCharArray()); }
        public bool Contains(char[] word) { return isPresent(word); }

        /// <summary>
        /// Searches compacted and uncompacted trie.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool isPresent(char[] word)
        {
            if (word.Length == 0)
                return end;

            if (next == null)
                return false; // there's no further to look!

            foreach (TrieNode node in next)
            {
                if (node.me.Length > word.Length) // too short, can't match.
                    continue;

                bool found = false;
                for (int i = 0; i < node.me.Length; i++)
                {
                    if (node.me[i] != word[i])
                    {
                        found = false;
                        break;
                    }
                    found = true;
                }
                if (found) // this node is the correct node. copy the word remainder.
                {
                    char[] newWord = new char[word.Length - node.me.Length];
                    for (int i = node.me.Length, j = 0; j < word.Length - node.me.Length; i++, j++)
                        newWord[j] = word[i];
                    return node.isPresent(newWord);
                }
            }
            return false;
        }
        public void add(string word) { add(word.ToCharArray()); }

        /// <summary>
        /// Adds word to tree; only adds singles, does not maintain patricia/radix style.
        /// </summary>
        /// <param name="word"></param>
        public void add(char[] word)
        {
            if (word.Length == 0)
            {
                end = true;
                return;
            }
            char c = word[0];

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
                target = new TrieNode();
                target.me = new char[] { c };
                TrieNode[] newNext = new TrieNode[next.Length + 1];  // expands the array of children
                System.Array.Copy(next, newNext, next.Length);
                newNext[next.Length] = target;
                next = newNext;
            }
            char[] newWord = new char[word.Length - 1];

            for (int i = 1, j = 0; j < word.Length - 1; i++, j++)
                newWord[j] = word[i];

            target.add(newWord);
        }

        #region utility       
        protected static string buildString(Stack<char[]> chunks)
        {
            var v = chunks.Reverse();
            StringBuilder sb = new StringBuilder();

            foreach (char[] cs in v)
                sb.Append(cs);

            return sb.ToString();
        }
        // This method is to let subclasses fiddle with storage.
        virtual protected string stringRepresentation()
        {
            return new string(me);
        }
        #endregion
    }
}
