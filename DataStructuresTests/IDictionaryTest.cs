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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataStructuresTests
{
    public class IDictionaryTest
    {
        public void Insert_Random(IDictionary<string, string> dictionary, int howMany)
        {
            Assert.AreEqual(0, dictionary.Count);
            string key = Util.RandomString(10);
            for (int i = 0; i < howMany; i++)
            {
                Assert.AreEqual(i, dictionary.Count);
                while (dictionary.ContainsKey(key))
                {
                    key = Util.RandomString(10);
                }
                dictionary.Add(key, Util.RandomString(5));
            }
        }

        public void Count_Up_Large_From_Random(IDictionary<int, string> dictionary)
        {
            Assert.AreEqual(0, dictionary.Count);
            int key = new Random().Next();
            for (int i = 0; i < 100 * 1000; i++)
            {
                Assert.AreEqual(i, dictionary.Count);
                dictionary.Add(key++, Util.RandomString(5));           
            }
        }

        public void Count_Up_Large_From_Zero(IDictionary<int, string> dictionary)
        {
            Assert.AreEqual(0, dictionary.Count);
            for (int i = 0; i < 100 * 1000; i++)
            {
                Assert.AreEqual(i, dictionary.Count);
                dictionary.Add(i, Util.RandomString(5));
            }
        }

        public void Count_Up_Small(IDictionary<int, string> dictionary)
        {
            Assert.AreEqual(0, dictionary.Count);
            int key = new Random().Next();
            for (int i = 0; i < 50; i++)
            {
                Assert.AreEqual(i, dictionary.Count);
                dictionary.Add(key++, Util.RandomString(5));
            }
        }

        public void Count_Random(IDictionary<int, string> dictionary)
        {
            Assert.AreEqual(0, dictionary.Count);
            Random random = new Random();
            for (int i = 0; i < 5000; i++)
            {
                int key = random.Next();
                while(dictionary.ContainsKey(key))
                {
                    key = random.Next();
                }
                Assert.AreEqual(i, dictionary.Count);
                dictionary.Add(key, Util.RandomString(5));
            }
        }

        public void IsReadOnlyReturnsFalse(IDictionary<int, int> dictionary)
        {
            Assert.IsFalse(dictionary.IsReadOnly);
        }

        public void Add_And_Get(IDictionary<int, string> dictionary)
        {
            int key = new Random().Next();
            string val = Util.RandomString(20);
            dictionary.Add(key, val);
            Assert.IsTrue(dictionary.ContainsKey(key));
            Assert.AreEqual(val, dictionary[key]);
        }

        public void Add_Many_Times_Remove_Once(IDictionary<int, string> dictionary)
        {
            int key = new Random().Next();
            string val = Util.RandomString(21);

            for (int i = 0; i < 50; i++)
            {
                dictionary[key] = val;
            }
            Assert.AreEqual(dictionary[key], val);
            dictionary.Remove(key);
            Assert.IsFalse(dictionary.ContainsKey(key));
        }

        public void Enumerate(IDictionary<int, string> dictionary)
        {
            List<KeyValuePair<int,string>> originalKvps = new List<KeyValuePair<int, string>>();
            Random rand = new Random();
            for (int i = 0; i < 50; i++)
            {
                int key = rand.Next();
                while (dictionary.ContainsKey(key))
                {
                    key = rand.Next();
                }
                string val = Util.RandomString(21);         
                dictionary[key] = val;
                originalKvps.Add(new KeyValuePair<int, string>(key, val));
            }

            foreach (KeyValuePair<int, string> kvp in dictionary)
            {
                originalKvps.Remove(kvp);
            }
            Assert.IsFalse(originalKvps.Any());
        }

        public void EnumerateKeys(IDictionary<int, string> dictionary)
        {
            List<int> originalKeys = new List<int>();
            Random rand = new Random();
            for (int i = 0; i < 50; i++)
            {
                int key = rand.Next();
                while (dictionary.ContainsKey(key))
                {
                    key = rand.Next();
                }
                string val = Util.RandomString(21);
                dictionary[key] = val;
                originalKeys.Add(key);
            }

            foreach (int key in dictionary.Keys)
            {
                originalKeys.Remove(key);
            }
            Assert.IsFalse(originalKeys.Any());
        }

        public void EnumerateValues(IDictionary<int, string> dictionary)
        {
            List<string> originalValues = new List<string>();
            Random rand = new Random();
            for (int i = 0; i < 50; i++)
            {
                int key = rand.Next();
                while (dictionary.ContainsKey(key))
                {
                    key = rand.Next();
                }
                string val = Util.RandomString(21);
                while (originalValues.Contains(val))
                {
                    val = Util.RandomString(21);
                }
                dictionary[key] = val;
                originalValues.Add(val);
            }

            foreach (string val in dictionary.Values)
            {
                originalValues.Remove(val);
            }
            Assert.IsFalse(originalValues.Any());
        }

        public void Add_Remove_Lots_To_Test_Shrink(IDictionary<int, string> dictionary)
        {
            Random rand = new Random();
            for (int i = 0; i < 100; i++)
            {
                List<int> keys = new List<int>();
                for (int j = 0; j < 5000; j++)
                {
                    int next = rand.Next();
                    keys.Add(next);
                    dictionary.Add(next, Util.RandomString(3));
                }
                foreach (int key in keys)
                {
                    dictionary.Remove(key);
                }
            }
        }

        public void Add_And_TryGet(IDictionary<int, string> dictionary)
        {
            int key = new Random().Next();
            string val = Util.RandomString(20);
            dictionary.Add(key, val);
            string holder;
            Assert.IsTrue(dictionary.TryGetValue(key, out holder));
            Assert.IsTrue(dictionary.ContainsKey(key));
            Assert.AreEqual(holder, val);
        }
        
        public void Add_And_Remove(IDictionary<int, string> dictionary)
        {
            int key = new Random().Next();
            dictionary.Add(key, Util.RandomString(20));
            dictionary.Remove(key);
            Assert.IsFalse(dictionary.ContainsKey(key));
            string holder;
            Assert.IsFalse(dictionary.TryGetValue(key, out holder));            
        }
        
        public void Add_Remove_Get_Throws(IDictionary<int, string> dictionary)
        {
            Random random = new Random();
            int key = random.Next();
            string val = Util.RandomString(20);
            dictionary.Add(key, val);
            dictionary.Remove(key);
            // ReSharper disable once UnusedVariable
            string holder = dictionary[key];
        }
        
        public void Add_Null_Throws(IDictionary<string, string> dictionary)
        {
            dictionary.Add(null, Util.RandomString(20));
        }
        
        public void Get_Null_Throws(IDictionary<string, string> dictionary)
        {
            dictionary.Add(string.Empty, Util.RandomString(20));
            // ReSharper disable once UnusedVariable
            string holder = dictionary[null];
        }
        
        public void Get_Empty_Null_Throws(IDictionary<string, string> dictionary)
        {
            // ReSharper disable once UnusedVariable
            string holder = dictionary[null];
        }

        public void Get_Try_Null_Throws(IDictionary<string, string> dictionary)
        {
            dictionary.Add(string.Empty, Util.RandomString(20));
            string holder;
            dictionary.TryGetValue(null, out holder);
        }

        public void Get_Try_Emtpy_Null_Throws(IDictionary<string, string> dictionary)
        {
            string holder;
            dictionary.TryGetValue(null, out holder);
        }
        
        public void Clear_Empty(IDictionary<string, string> dictionary)
        {
            Assert.AreEqual(0, dictionary.Count);
            dictionary.Clear();
            Assert.AreEqual(0, dictionary.Count);
        }
        
        public void Clear(IDictionary<string, string> dictionary)
        {
            dictionary.Add(Util.RandomString(20), Util.RandomString(30));
            dictionary.Clear();
            Assert.AreEqual(0, dictionary.Count);
        }
        
        public void Clear_Then_Get_Throws(IDictionary<string, string> dictionary)
        {
            string key = Util.RandomString(20);
            dictionary.Add(key, Util.RandomString(30));
            dictionary.Clear();
            // ReSharper disable once UnusedVariable
            string holder = dictionary[key];
        }
    }
}
