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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    [Serializable]
    sealed class BurstCacheBucket : BurstNavigable, IEnumerable
    {
        public static short burstThreshold = 32; // TODO update to suggestions in BurstSort papers, sub-buckets

        char[] itemSpace = new char[256];
        int[] itemPositions;
        short itemCount = 0;
        int nextFreePosition = 0;

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < itemCount - 1; i++)
            {
                ArraySegment<char> segment = new ArraySegment<char>(itemSpace, itemPositions[i], (itemPositions[i + 1] - itemPositions[i]) - 1);
                yield return segment;
            }

            ArraySegment<char> lastSegment = new ArraySegment<char>(itemSpace, itemPositions[itemCount-1], (nextFreePosition - itemPositions[itemCount -1]) -1);//  (itemPositions[i + 1] - itemPositions[i]) - 1);
            yield return lastSegment;
        }

        public BurstCacheBucket()
        {
            itemPositions = new int[burstThreshold];
//            itemPositions[0] = 0;  // implied/default
        }

        public override bool isPresent(char[] word)
        {
            return isPresent(word, 0);
        }

        /// <summary>
        /// Searches bucket.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public override bool isPresent(char[] word, int start)
        {
            if (word.Length == start)
                return end;

            for (int i = 0; i < itemCount -1; i++)
            {
                int suffixStart = itemPositions[i];
                int suffixLength = (itemPositions[i + 1] - suffixStart) -1; // -1 for null terminator

                if (suffixLength != (word.Length - start))
                    continue;

                int j = 0;
                for (; j < suffixLength; j++)
                {
                    if (word[j + start] != itemSpace[suffixStart + j])
                        break;
                }
                if (j == suffixLength)
                    return true;
            }

            // now do the last one; its length is the distance to 'nextfreePostion' - 1
            int lastSuffixStart = itemPositions[itemCount - 1];
            int lastSuffixLength = (nextFreePosition - lastSuffixStart) -1;  // for null terminator

            if (lastSuffixLength != (word.Length - start))
                return false;

            int k = 0;
            for (; k < lastSuffixLength; k++)
            {
                if (word[k + start] != itemSpace[lastSuffixStart + k])
                    break;
            }
            if (k == lastSuffixLength)
                return true;
            else
                return false;
        }

        public void Add(string word) { Add(word.ToCharArray()); }

        public override void Add(char[] word)
        {
            Add(word, 0);
        }

        public override void Add(char[] word, int start = 0)
        {
            if (word.Length == start)
            {
                end = true;
                return;
            }

            // expand to hold value
            int idealSpaceSize = itemSpace.Length;
            int neededSpace = nextFreePosition + (word.Length - start) + 1;
            while (idealSpaceSize < neededSpace)
                idealSpaceSize *= 2;

            if (itemSpace.Length != idealSpaceSize)
            {
                char[] newSpace = new char[idealSpaceSize];
                System.Array.Copy(itemSpace, newSpace, nextFreePosition);
                itemSpace = newSpace;
            }

            // TODO does not look for unique strings, that's standard yes?
            System.Array.Copy(word, start, this.itemSpace, this.nextFreePosition, word.Length - start);
            itemPositions[itemCount++] = nextFreePosition;
            nextFreePosition += (word.Length - start) + 1;

            if (itemCount >= burstThreshold)
                shouldBurst = true;
        }

        // NOTE copied code from other add for perf, instead of currying etc.
        public void Add(char[] word, int start, int length)
        {
            if (length == 0)
            {
                end = true;
                return;
            }

            // expand to hold value
            int idealSpaceSize = itemSpace.Length;
            int neededSpace = nextFreePosition + (length) + 1;
            while (idealSpaceSize < neededSpace)
                idealSpaceSize *= 2;

            if (itemSpace.Length != idealSpaceSize)
            {
                char[] newSpace = new char[idealSpaceSize];
                System.Array.Copy(itemSpace, newSpace, nextFreePosition);
                itemSpace = newSpace;
            }

            // TODO does not look for unique strings, that's standard yes?
            System.Array.Copy(word, start, this.itemSpace, this.nextFreePosition, length);
            itemPositions[itemCount++] = nextFreePosition;
            nextFreePosition += (length) + 1;

            if (itemCount >= burstThreshold)
                shouldBurst = true;
        }

        // TODO there are probably faster ways to do this, c.f. grunholm
        int compareBucketItems(int a, int b)
        {
            char ca = itemSpace[a];
            char cb = itemSpace[b];

            int i = 0;
            while (ca != (char)0 && cb != (char)0)
            {
                if (ca != cb)
                    return ca.CompareTo(cb);
                ca = itemSpace[a + i];
                cb = itemSpace[b + i];
                i++;
            }

            if (ca == (char)0 && cb == (char)0)
                return 0;
            else if (ca == (char)0)
                return -1;
            else return 1;
        }

        void QuicksortBucket(List<int> elements, int left, int right)
        {
            int i = left, j = right;
            int pivot = elements[(left + right) / 2];

            while (i <= j)
            {
                while (compareBucketItems(elements[i], pivot) < 0)
                    i++;

                while (compareBucketItems(elements[j], pivot) > 0)
                    j--;

                if (i <= j)
                {
                    // Swap
                    int tmp = elements[i];
                    elements[i] = elements[j];
                    elements[j] = tmp;

                    i++;
                    j--;
                }
            }

            if (left < j)
                QuicksortBucket(elements, left, j);

            if (i < right)
                QuicksortBucket(elements, i, right);
        }

        /// <summary>
        /// Exports the bucket into the supplied list
        /// </summary>
        /// <param name="trail"></param>
        /// <param name="pile"></param>
        public override void reconstruct(char[] trail, int length, List<string> pile)
        {
            if (end)
                pile.Add(new string(trail, 0, length));

            // make copy of positions array, sort it, report in that order.
            if (itemCount == 0) // there's nothing here
                return;

            List<int> sortCopy = new List<int>();
            for(int i = 0; i < itemCount; i++)  // last is not a word start
                sortCopy.Add(itemPositions[i]);

            // TODO try alternate algorithms
            QuicksortBucket(sortCopy, 0, sortCopy.Count - 1);
            for (int i = 0; i < sortCopy.Count; i++)
            {
                int suffixStart = sortCopy[i];
                char c = itemSpace[suffixStart];

                int j = 0;
                while (c != (char)0)
                {
                    trail[length + j] = c;
                    j++;
                    c = itemSpace[suffixStart + j];
                }

                if (j != 0)
                    pile.Add(new string(trail, 0, length + j));
            }
        }
    }
}
