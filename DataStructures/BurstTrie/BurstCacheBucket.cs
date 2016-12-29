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

namespace DataStructures
{
    [Serializable]
    sealed class BurstCacheBucket : BurstNavigable, IEnumerable
    {
        public static short BurstThreshold = 32; // TODO update to suggestions in BurstSort papers, sub-buckets

        char[] _itemSpace = new char[256];
        int[] _itemPositions;
        short _itemCount;
        int _nextFreePosition;

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < _itemCount - 1; i++)
            {
                ArraySegment<char> segment = new ArraySegment<char>(_itemSpace, _itemPositions[i], (_itemPositions[i + 1] - _itemPositions[i]) - 1);
                yield return segment;
            }

            ArraySegment<char> lastSegment = new ArraySegment<char>(_itemSpace, _itemPositions[_itemCount-1], (_nextFreePosition - _itemPositions[_itemCount -1]) -1);//  (itemPositions[i + 1] - itemPositions[i]) - 1);
            yield return lastSegment;
        }

        public BurstCacheBucket()
        {
            _itemPositions = new int[BurstThreshold];
//            itemPositions[0] = 0;  // implied/default
        }

        public override bool IsPresent(char[] word)
        {
            return IsPresent(word, 0);
        }

        /// <summary>
        /// Searches bucket.
        /// </summary>
        public override bool IsPresent(char[] word, int start)
        {
            if (word.Length == start)
                return End;

            if (_itemCount == 0)
                return false;

            for (int i = 0; i < _itemCount -1; i++)
            {
                int suffixStart = _itemPositions[i];
                int suffixLength = (_itemPositions[i + 1] - suffixStart) -1; // -1 for null terminator

                if (suffixLength != (word.Length - start))
                    continue;

                int j = 0;
                for (; j < suffixLength; j++)
                {
                    if (word[j + start] != _itemSpace[suffixStart + j])
                        break;
                }
                if (j == suffixLength)
                    return true;
            }

            // now do the last one; its length is the distance to 'nextfreePostion' - 1
            int lastSuffixStart = _itemPositions[_itemCount - 1];
            int lastSuffixLength = (_nextFreePosition - lastSuffixStart) -1;  // for null terminator

            if (lastSuffixLength != (word.Length - start))
                return false;

            int k = 0;
            for (; k < lastSuffixLength; k++)
            {
                if (word[k + start] != _itemSpace[lastSuffixStart + k])
                    break;
            }
            return k == lastSuffixLength;
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
                End = true;
                return;
            }

            // expand to hold value
            int idealSpaceSize = _itemSpace.Length;
            int neededSpace = _nextFreePosition + (word.Length - start) + 1;
            while (idealSpaceSize < neededSpace)
                idealSpaceSize *= 2;

            if (_itemSpace.Length != idealSpaceSize)
            {
                char[] newSpace = new char[idealSpaceSize];
                Array.Copy(_itemSpace, newSpace, _nextFreePosition);
                _itemSpace = newSpace;
            }

            // TODO does not look for unique strings, that's standard yes?
            Array.Copy(word, start, _itemSpace, _nextFreePosition, word.Length - start);
            _itemPositions[_itemCount++] = _nextFreePosition;
            _nextFreePosition += (word.Length - start) + 1;

            if (_itemCount >= BurstThreshold)
                ShouldBurst = true;
        }

        // NOTE copied code from other Add for perf, instead of currying etc.
        public void Add(char[] word, int start, int length)
        {
            if (length == 0)
            {
                End = true;
                return;
            }

            // expand to hold value
            int idealSpaceSize = _itemSpace.Length;
            int neededSpace = _nextFreePosition + (length) + 1;
            while (idealSpaceSize < neededSpace)
                idealSpaceSize *= 2;

            if (_itemSpace.Length != idealSpaceSize)
            {
                char[] newSpace = new char[idealSpaceSize];
                Array.Copy(_itemSpace, newSpace, _nextFreePosition);
                _itemSpace = newSpace;
            }

            // TODO does not look for unique strings, that's standard yes?
            Array.Copy(word, start, this._itemSpace, this._nextFreePosition, length);
            _itemPositions[_itemCount++] = _nextFreePosition;
            _nextFreePosition += (length) + 1;

            if (_itemCount >= BurstThreshold)
                ShouldBurst = true;
        }

        // TODO there are probably faster ways to do this, c.f. grunholm
        int CompareBucketItems(int a, int b)
        {
            char ca = _itemSpace[a];
            char cb = _itemSpace[b];

            int i = 0;
            while (ca != (char)0 && cb != (char)0)
            {
                if (ca != cb)
                    return ca.CompareTo(cb);
                ca = _itemSpace[a + i];
                cb = _itemSpace[b + i];
                i++;
            }

            if (ca == (char)0 && cb == (char)0)
                return 0;
            if (ca == (char)0)
                return -1;
            return 1;
        }

        // TODO candidate for its own unit tests.
        // List<int> might be faster?
        void QuicksortBucket(int[] elements, int left, int right)
        {
            int i = left, j = right;
            int pivot = elements[(left + right) / 2];

            while (i <= j)
            {
                while (CompareBucketItems(elements[i], pivot) < 0)
                    i++;

                while (CompareBucketItems(elements[j], pivot) > 0)
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
        public override void Reconstruct(char[] trail, int length, List<string> pile)
        {
            if (End)
                pile.Add(new string(trail, 0, length));

            // make copy of positions array, sort it, report in that order.
            if (_itemCount == 0) // there's nothing here
                return;

            int[] sortCopy = new int[_itemCount];
            for(int i = 0; i < _itemCount; i++)
                sortCopy[i] = _itemPositions[i];

            // TODO try alternate algorithms: radix MSD, etc.
            QuicksortBucket(sortCopy, 0, sortCopy.Length - 1);

            foreach (int suffixStart in sortCopy)
            {
                char c = _itemSpace[suffixStart];

                int j = 0;
                while (c != (char)0)
                {
                    trail[length + j] = c;
                    j++;
                    c = _itemSpace[suffixStart + j];
                }

                if (j != 0)
                    pile.Add(new string(trail, 0, length + j));
            }
        }
    }
}
