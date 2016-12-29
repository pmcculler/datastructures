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
using System.Text;

namespace DataStructures
{
    public class SuffixArray
    {
        private int[] _indexes = { };
        private readonly char[] _source = { };

        public SuffixArray(string s)
        {
            _source = s.ToCharArray();
            UpdateIndex();
        }

        // This creates it in a useless state.
        public SuffixArray() { }

        // Find an instance, not necessarily the first one in the source.
        // Searching for a zero-length string gives undefined results. Don't.
        public int Find(string s)
        {
            return Find(s.ToCharArray());
        }

        // Find an instance, not necessarily the first one in the source.
        // Searching for a zero-length string gives undefined results. Don't.
        public int Find(char[] chars)
        {
            // TODO: avoid creating a new searcher object every time.
            Searcher search = new Searcher(this, chars);
            // ReSharper disable once UnusedVariable
            int k = Array.BinarySearch(_indexes, 0, search);
            return search.SourceIndex;
        }

        // Searching for a zero-length string gives undefined results. Don't.
        public bool Contains(string s)
        {
            return (Find(s) > -1);
        }

        protected void UpdateIndex()
        {
            _indexes = new int[_source.Length];
            for (int i = 0; i < _indexes.Length; i++)
                _indexes[i] = i;
            Sorter sort = new Sorter(this);
            Array.Sort(_indexes, sort);
        }

        public class Sorter : IComparer<int>
        {
            private long _comparisons;
            private long _charComparisons;

            private readonly SuffixArray _sa;
            public Sorter(SuffixArray sa)
            {
                _sa = sa;
            }

            public int Compare(int a, int b)
            {
                _comparisons++;
                // consider each int an index into sa.source
                // do a linear char by char comparison to see which is alphabetically before which
                // jumping out as soon as we can of course.

                if (a == b) // why compare with yourself again?
                    return 0;

                int i = a, j = b;

                StringBuilder sb = new StringBuilder();
                for (; i < _sa._source.Length && j < _sa._source.Length; i++, j++)
                {
                    _charComparisons++;
                    char ca = _sa._source[i];
                    char cb = _sa._source[j];
                    sb.Append(ca);
                    if (ca < cb)
                        return -1;
                    if (ca > cb)
                        return 1;
                }
                // the same all the way to the end of the source? seems kind of unlikely.
                // but alphabetically it's the shorter one that comes first. that's going to be 'end'
                return -1;
            }
        }

        public class Searcher : IComparer<int>
        {
            private readonly SuffixArray _sa;
            private readonly char[] _sought;
            public Searcher(SuffixArray sa, char[] sought)
            {
                _sa = sa;
                _sought = sought;
            }

            public int SourceIndex = -1;
            // argument b is always ignored.
            // instead, we compare source[a] with sought[0] and move forwards
            public int Compare(int a, int b)
            {
                int i = a, j = 0;

                for (; i < _sa._source.Length && j < _sought.Length; i++, j++)
                {
                    char ca = _sa._source[i];
                    char cb = _sought[j];
                    if (ca < cb)
                        return -1;
                    if (ca > cb)
                        return 1;
                }
                SourceIndex = a;
                return 0;
            }
        }
    }
}
