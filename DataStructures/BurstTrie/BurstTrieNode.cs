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
    sealed class BurstTrieNode : BurstNavigable
    {
        // static char ARBITRARY_ROOT_VALUE = '_';  // debugging
        //        protected char me = ARBITRARY_ROOT_VALUE;  // arbitrary root node  // only useful for debug really
        
        readonly static int TRIE_WIDTH = 128;
        readonly static int MINIMAL_WIDTH_USE = 32;  // standard ascii
        readonly static int MAXIMAL_WIDTH_USE = 126;  // standard ascii
        BurstNavigable[] next = null;

        /// <summary>
        /// Exports the whole trie into the supplied list.
        /// </summary>
        /// <param name="trail"></param>
        /// <param name="pile"></param>
        public override void reconstruct(char[] trail, int length, List<string> pile)
        {
            if (next == null)
            {
                pile.Add(new string(trail, 0, length));
                // end of trail. build string and add to pile.
                return;
            }
            if (end)
                pile.Add(new string(trail, 0, length));

            for (int i = MINIMAL_WIDTH_USE; i < MAXIMAL_WIDTH_USE; i++) // TODO: optimize by holding onto minimum, maximum values actually used in this node
            {
                BurstNavigable node = next[i];
                if (node != null)
                {
                    trail[length] = (char)i;
                    node.reconstruct(trail, length + 1, pile);
                }
            }
        }

        public List<string> getAllEntries()
        {
            List<string> items = new List<string>();
            reconstruct(new char[128], 0, items);
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
        public override bool isPresent(char[] word)
        {
            return isPresent(word, 0);
        }

        public override bool isPresent(char[] word, int start)
        {
            if (word.Length == start)
                return end;

            if (next == null)
                return false; // there's no further to look!

            BurstNavigable target = next[word[start]];
            if (target == null)
                return false;  // no potentially matching suffix

            return target.isPresent(word, start + 1);  // 1 = me.Length
        }
        public void Add(string word) { Add(word.ToCharArray()); }

        /// <summary>
        /// Adds word to tree; only adds singles, does not maintain patricia/radix style.
        /// </summary>
        /// <param name="word"></param>
        public override void Add(char[] word)
        {
            Add(word, 0);
        }
        
        public override void Add(char[] word, int start)
        {
            if (word.Length == start)
            {
                end = true;
                return;
            }
            int c = (int)word[start];

            if (next == null)
                next = new BurstNavigable[TRIE_WIDTH];

            BurstNavigable target = next[c];

            if (target == null)
            {
                // always start with a bucket.
                target = new BurstCacheBucket();
                next[c] = target;
            }
            else if (target.shouldBurst)  // else OK because there's never any need to burst a brand new bucket
            {
                target = burstThisBucket(target);
                next[c] = target;
            }

            target.Add(word, start + 1);
        }

        BurstTrieNode burstThisBucket(BurstNavigable bucketIn)
        {
            BurstCacheBucket bucket = (BurstCacheBucket)bucketIn;
            BurstTrieNode node = new BurstTrieNode();
//            node.me = key;  // debug only
            node.next = new BurstNavigable[TRIE_WIDTH];
            node.end = bucket.end;

            // TODO maybe make this a higher performance construct?
            foreach(ArraySegment<char> item in bucket)
            {

                if (item.Count == 1) // in this case the suffix is consumed by the new trie node + entry (empty destination bucket, maybe, or now creating and adding to)
                {
                    if (node.next[(int)item.Array[item.Offset]] == null)
                    {
                        BurstCacheBucket emptyBucket = new BurstCacheBucket();
                        emptyBucket.end = true;
                        node.next[(int)item.Array[item.Offset]] = emptyBucket;  // only needed if it was not there, so maybe save this assignment
                        continue;
                    }
                    else // already a bucket there, from previous iteration in this loop
                    {
                        BurstCacheBucket reusingBucket = (BurstCacheBucket)node.next[(int)item.Array[item.Offset]];
                        reusingBucket.end = true;
                        continue;
                    }
                }
                
                else // there is more suffix that's not consumed by the new trie node + entry, so create / fill bucket
                {
                    BurstNavigable nav = node.next[(int)item.Array[item.Offset]];
                    if (nav == null)
                    {
                        BurstCacheBucket newBucket = new BurstCacheBucket();
                        char[] arr = item.Array;
                        int offset = item.Offset;
                        newBucket.Add(arr, offset + 1, item.Count - 1);
                        node.next[(int)arr[offset]] = newBucket;
                    }
                    else
                    {
                        ((BurstCacheBucket)nav).Add(item.Array, item.Offset + 1, item.Count - 1);
                    }
                }
            }
            return node;
        }
    }
}
