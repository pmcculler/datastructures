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

namespace DataStructures
{
    public class Hashes<TKey>
    {
        private Hashes()
        {
        }

        protected static int ToPositive(int val)
        {
            if (val >= 0)
            {
                return val;
            }
            return val * -1;
        }

        public static int Hash(TKey key, int modulus = -1)
        {
            int hash = key.GetHashCode();
            return ToPositive(hash % modulus);
        }

        public static int Hash2(TKey key, int modulus = -1)
        {
            // DJB hash function, woot
            int hash = 5381;

            foreach (char c in key.ToString())
            {
                hash = (hash << 5) + hash + c;
            }
            return ToPositive(hash % modulus);
        }

        public static int Hash3(TKey key, int modulus = -1)
        {
            // Sedgewick hash function, woot
            int b = 378551;
            int a = 63689;
            int hash = 0;  // was long

            foreach (char c in key.ToString())
            {
                hash = hash * a + c;
                a = a * b;
            }

            return ToPositive(hash % modulus);
        }
    }
}
