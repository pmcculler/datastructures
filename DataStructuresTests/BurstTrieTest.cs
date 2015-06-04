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
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataStructures;

namespace DataStructuresTests
{
    [TestClass]
    public class BurstBurstTrieTest
    {
        [TestMethod]
        public void BurstTrie_Create()
        {
            BurstTrie trie = new BurstTrie();
        }
        [TestMethod]
        public void BurstTrie_OneNoCheck()
        {
            BurstTrie trie = new BurstTrie();
            trie.Add("con");
        }
        [TestMethod]
        public void BurstTrie_ContainsOne()
        {
            BurstTrie trie = new BurstTrie();
            trie.Add("con");
            Assert.IsTrue(trie.Contains("con"));
        }
        [TestMethod]
        public void BurstTrie_NotContainsUnrelated()
        {
            BurstTrie trie = new BurstTrie();
            trie.Add("a");
            Assert.IsFalse(trie.Contains("b"));
        }
        [TestMethod]
        public void BurstTrie_Blank()
        {
            BurstTrie trie = new BurstTrie();
            trie.Add("");
            Assert.IsTrue(trie.Contains(""));
        }
        [TestMethod]
        public void BurstTrie_Empty()
        {
            BurstTrie trie = new BurstTrie();
            Assert.IsFalse(trie.Contains(""));
        }

        [TestMethod]
        public void BurstTrie_ReconstructSimple()
        {
            BurstTrie trie = new BurstTrie();
            string s = "cat";
            trie.Add(s);
            List<string> all = trie.GetAllEntries();
            Assert.IsTrue(all.Contains(s));
        }

        [TestMethod]
        public void BurstTrie_ReconstructBucket()
        {
            BurstTrie trie = new BurstTrie();
            BurstTrie.BurstThreshold = 5;

            string s = "TE";
            for (int i = 0; i < 7; i++)
            {
                trie.Add(s + (char)(65 + i));
            }

            List<string> all = trie.GetAllEntries();
            Assert.IsTrue(all.Contains("TEA"));
            Assert.IsTrue(all.Contains("TEF"));
        }

        [TestMethod]
        public void BurstTrie_AllContained()
        {
            BurstTrie trie = new BurstTrie();
            string[] data = {   "abc", "abd", "abe", 
                                "fonduepot", "fondue", "l",
                                "f", "0", "aaaaaaaaaaaaaaaaa",
                                "jkjkjkjkaaaaa", "aaaaaaaaa", "fabc" };
            List<string> items = new List<string>(data);

            foreach (string s in items)
                trie.Add(s);

            verifyPresence(trie, items);
        }
        protected void verifyPresence(BurstTrie trie, List<string> items)
        {
            List<string> allEntries = trie.GetAllEntries();
            Assert.AreEqual(allEntries.Count, items.Count);

            foreach (string s in items)
                Assert.IsTrue(allEntries.Contains(s));
        }

        [TestMethod]
        public void BurstTrie_NotContains()
        {
            BurstTrie trie = new BurstTrie();
            trie.Add("con");
            Assert.IsFalse(trie.Contains("coo"));
            Assert.IsFalse(trie.Contains("cona"));
            Assert.IsFalse(trie.Contains("ocon"));
            Assert.IsFalse(trie.Contains("noc"));
            Assert.IsFalse(trie.Contains("cno"));
            Assert.IsFalse(trie.Contains("ocn"));
            Assert.IsFalse(trie.Contains("ccc"));
            Assert.IsFalse(trie.Contains("nnn"));
            Assert.IsFalse(trie.Contains("ooo"));
        }
        [TestMethod]
        public void BurstTrie_SubstringNotContains()
        {
            BurstTrie trie = new BurstTrie();
            trie.Add("abcde");
            Assert.IsFalse(trie.Contains("a"));
            Assert.IsFalse(trie.Contains("ab"));
            Assert.IsFalse(trie.Contains("abc"));
            Assert.IsFalse(trie.Contains("abcd"));
            Assert.IsFalse(trie.Contains("bcde"));
            Assert.IsFalse(trie.Contains("cde"));
            Assert.IsFalse(trie.Contains("de"));
            Assert.IsFalse(trie.Contains("e"));
            Assert.IsFalse(trie.Contains("b"));
            Assert.IsFalse(trie.Contains("c"));
            Assert.IsFalse(trie.Contains("d"));
        }
        [TestMethod]
        public void BurstTrie_MultiNotContains()
        {
            BurstTrie trie = new BurstTrie();
            trie.Add("con");
            trie.Add("cono");
            trie.Add("foo");
            trie.Add("fon");
            trie.Add("fondue");

            Assert.IsTrue(trie.Contains("con"));
            Assert.IsTrue(trie.Contains("fon"));
            Assert.IsTrue(trie.Contains("fondue"));
            Assert.IsFalse(trie.Contains("fo"));
            Assert.IsFalse(trie.Contains("cor"));
            Assert.IsFalse(trie.Contains("corse"));
            Assert.IsFalse(trie.Contains("fondu"));
            Assert.IsFalse(trie.Contains("f"));
            Assert.IsFalse(trie.Contains("x"));
        }

        [TestMethod]
        public void BurstTrie_FailedString()
        {
            string s = @"  You may copy it, give it away or\r\nre-use it under the terms of the Project Gutenberg License included\r\nwith this eBook or online at www";
            BurstTrie trie = new BurstTrie();
            trie.Add(s);
            Assert.IsTrue(trie.Contains(s));
        }

        [TestMethod]
        public void BurstTrie_BurstBucket()
        {
            // 32 is current limit; revisit?
            string s = "test";
            BurstTrie trie = new BurstTrie();
            for (int i = 0; i < 33; i++)
            {
                trie.Add(s);
                s += i.ToString();
            }
            Assert.IsTrue(trie.Contains("test"));
            Assert.IsTrue(trie.Contains("test0"));
            Assert.IsTrue(trie.Contains("test01"));
            Assert.IsFalse(trie.Contains("est"));
            Assert.IsFalse(trie.Contains("est0"));
        }

        [TestMethod]
        public void BurstTrie_BurstBucketSimple()
        {
            // 32 is current limit; revisit?
            BurstTrie trie = new BurstTrie();
            BurstTrie.BurstThreshold = 5;

            string s = "TE";
            for (int i = 0; i < 7; i++)
            {
                trie.Add(s + (char)(65+i));
            }
            Assert.IsTrue(trie.Contains("TEA"));
            Assert.IsTrue(trie.Contains("TEB"));
            Assert.IsTrue(trie.Contains("TEC"));
            Assert.IsTrue(trie.Contains("TED"));
            Assert.IsTrue(trie.Contains("TEE"));
            Assert.IsTrue(trie.Contains("TEF"));
            Assert.IsFalse(trie.Contains("TE"));
            Assert.IsFalse(trie.Contains("EA"));
            Assert.IsFalse(trie.Contains("A"));
        }

        [TestMethod]
        public void BurstTrie_NonBurstBucketSimple()
        {
            // 32 is current limit; revisit?
            BurstTrie trie = new BurstTrie();
            BurstTrie.BurstThreshold = 10;

            string s = "TE";
            for (int i = 0; i < 7; i++)
            {
                trie.Add(s + (char)(65 + i));
            }
            Assert.IsTrue(trie.Contains("TEA"));
            Assert.IsTrue(trie.Contains("TEB"));
            Assert.IsTrue(trie.Contains("TEC"));
            Assert.IsTrue(trie.Contains("TED"));
            Assert.IsTrue(trie.Contains("TEE"));
            Assert.IsTrue(trie.Contains("TEF"));
            Assert.IsFalse(trie.Contains("TE"));
            Assert.IsFalse(trie.Contains("EA"));
            Assert.IsFalse(trie.Contains("A"));
        }

        [TestMethod]
        public void BurstTrie_EnglishPerf()
        {
            string[] strings = englishStrings;
            BurstTrie trie = new BurstTrie();
            foreach (string s in strings)
                trie.Add(s);

            foreach (string s in strings)                
                Assert.IsTrue(trie.Contains(s));
        }

        static string[] englishWords;
        static string[] englishStrings;

        static public void init()
        {
            Util.setupBook();
            englishStrings = Util.getManyEnglishStrings();
            englishWords = Util.getManyEnglishWords();

        }
        [ClassInitialize]
        static public void init(TestContext context)
        {
            BurstBurstTrieTest.init();
        }

        [TestMethod]
        public void BurstTrie_EnglishWordsPerf()
        {
            string[] strings = englishWords;
            BurstTrie trie = new BurstTrie();
            foreach (string s in strings)
                trie.Add(s);
            foreach (string s in strings)
                Assert.IsTrue(trie.Contains(s));
        }

        [TestMethod]
        public void BurstTrie_NullSortEnglishWords()
        {
            //string[] dest = new string[englishWords.Length];
            //System.Array.Copy(englishWords, dest, englishWords.Length);
            //System.Array.Sort(dest);

            //System.Console.Out.WriteLine("Words founnd from input and sorted: " + dest.Length);

            Dictionary<string, int> uniques = new Dictionary<string, int>();
            //foreach (string s in dest)

            foreach (string s in englishWords)
                if (!uniques.ContainsKey(s))
                    uniques.Add(s, 1);
                else
                    uniques[s] = uniques[s] + 1;

            string[] toSort = new string[uniques.Keys.Count];
            uniques.Keys.CopyTo(toSort, 0);
            System.Array.Sort(toSort);

            System.Console.Out.WriteLine("unique words found: " + uniques.Keys.Count);
        }

        [TestMethod]
        public void BurstTrie_SortEnglishWords()
        {
            string[] strings = englishWords;
            BurstTrie trie = new BurstTrie();
            foreach (string s in strings)
                trie.Add(s); ;

            List<string> sorted = trie.GetAllEntries();
            System.Console.Out.WriteLine("Words found through reconstruction: " + sorted.Count);

            Dictionary<string, int> uniques = new Dictionary<string, int>();
            foreach (string s in sorted)
                if (!uniques.ContainsKey(s))
                    uniques.Add(s, 1);
                else
                    uniques[s] = uniques[s] + 1;

            System.Console.Out.WriteLine("unique words found: " + uniques.Keys.Count);
        }

        //[TestMethod]
        //public void BurstTrie_SortEnglishWordsCharArrays()
        //{
        //    string[] strings = englishWords;
        //    BurstTrie trie = new BurstTrie();
        //    foreach (string s in strings)
        //        trie.add(s);

        //    List<char[]> sorted = trie.GetAllEntriesAsArrays();
        //    System.Console.Out.WriteLine("Words found through reconstruction: " + sorted.Count);

        //    //Dictionary<string, int> uniques = new Dictionary<string, int>();
        //    //foreach (char[] s in sorted)
        //    //    if (!uniques.ContainsKey(s))
        //    //        uniques.Add(s, 1);
        //    //    else
        //    //        uniques[s] = uniques[s] + 1;

        //    //System.Console.Out.WriteLine("unique words found: " + uniques.Keys.Count);
        //}
    }
}
