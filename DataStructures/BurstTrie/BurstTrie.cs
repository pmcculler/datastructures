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
    public class BurstTrie
    {
        // Burst Tries have a root trie node, each of which can resolve to either buckets or another trie node.

        BurstTrieNode root = new BurstTrieNode();

        public void Add(string word) { Add(word.ToCharArray()); }
        public void Add(char[] word)
        {
            root.Add(word,0);
        }
        public bool isPresent(char[] word)
        {
            return root.isPresent(word);
        }
        public bool isPresent(string word) { return isPresent(word.ToCharArray()); }
        public bool Contains(string word) { return isPresent(word.ToCharArray()); }
        public bool Contains(char[] word) { return isPresent(word); }

        /// <summary>
        /// NOTE THIS METHOD HAS A BEHAVIOR YOU MAY CARE ABOUT.
        /// Duplicate words are not always tracked separately,
        /// so they may be 'collapsed' into single entries here.
        /// To fix this, BurstTrieNodes must carry 'end' as an integer, not just a boolean.
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllEntries()
        {
            List<string> items = new List<string>();
            root.reconstruct(new char[128], 0, items);
            return items;
        }

        public static short BurstThreshold
        {
            get
            {  return BurstCacheBucket.burstThreshold;  }
            set
            {  BurstCacheBucket.burstThreshold = value;  }
        }

    }
}
