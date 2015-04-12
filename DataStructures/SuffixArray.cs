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
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStructures
{
    public class SuffixArray
    {
        private int[] indexes = new int[] { };
        private char[] source = new char[] { };

        public SuffixArray(string s)
        {
            source = s.ToCharArray();
            updateIndex();
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
            int k = Array.BinarySearch<int>(indexes, 0, search);
            return search.sourceIndex;
        }
        // Searching for a zero-length string gives undefined results. Don't.
        public bool Contains(string s)
        {
            return (Find(s) > -1);
        }

        protected void updateIndex()
        {
            indexes = new int[source.Length];
            for (int i = 0; i < indexes.Length; i++)
                indexes[i] = i;
            Sorter sort = new Sorter(this);
            System.Array.Sort<int>(indexes, sort);
        }

        public class Sorter : IComparer<int>
        {
            private long comparisons = 0;
            private long char_comparisons = 0;

            private SuffixArray sa;
            public Sorter(SuffixArray sa)
            {
                this.sa = sa;
            }

            public int Compare(int a, int b)
            {
                comparisons++;
                // consider each int an index into sa.source
                // do a linear char by char comparison to see which is alphabetically before which
                // jumping out as soon as we can of course.

                if (a == b) // why compare with yourself again?
                    return 0;

                int i = a, j = b;

                StringBuilder sb = new StringBuilder();
                for (; i < sa.source.Length && j < sa.source.Length; i++, j++)
                {
                    char_comparisons++;
                    char ca = sa.source[i];
                    char cb = sa.source[j];
                    sb.Append(ca);
                    if (ca < cb)
                        return -1;
                    else if (ca > cb)
                        return 1;
                }
                // the same all the way to the end of the source? seems kind of unlikely.
                // but alphabetically it's the shorter one that comes first. that's going to be 'end'
                return -1;
            }
        }

        public class Searcher : IComparer<int>
        {
            private SuffixArray sa;
            private char[] sought;
            public Searcher(SuffixArray sa, char[] sought)
            {
                this.sa = sa;
                this.sought = sought;
            }

            public int sourceIndex = -1;
            // argument b is always ignored.
            // instead, we compare source[a] with sought[0] and move forwards
            public int Compare(int a, int b)
            {
                int i = a, j = 0;

                for (; i < sa.source.Length && j < sought.Length; i++, j++)
                {
                    char ca = sa.source[i];
                    char cb = sought[j];
                    if (ca < cb)
                        return -1;
                    else if (ca > cb)
                        return 1;
                }
                sourceIndex = a;
                return 0;
            }
        }
    }
}
