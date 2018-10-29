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

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataStructures;

namespace DataStructuresTests
{
    public class ITrieTest
    {
        public void Trie_Create(ITrie trieIgnored)
        {
        }

        public void Trie_One(ITrie trie)
        {
            trie.Add("con");
        }

        public void Trie_Contains(ITrie trie)
        {
            trie.Add("con");
            Assert.IsTrue(trie.Contains("con"));
        }

        public void Trie_NotContainsUnrelated(ITrie trie)
        {
            trie.Add("a");
            Assert.IsFalse(trie.Contains("b"));
        }

        public void Trie_Blank(ITrie trie)
        {
            trie.Add("");
            Assert.IsTrue(trie.Contains(""));
        }

        public void Trie_Empty(ITrie trie)
        {
            Assert.IsFalse(trie.Contains(""));
        }

        public void Trie_SingleContained(ITrie trie)
        {
            string[] data = {"fonduepot", "fondue"};
            VerifyContains(data, trie);
        }

        public void Trie_SingleContainedOppositeOrder(ITrie trie)
        {
            string[] data = { "fondue", "fonduepot" };
            VerifyContains(data, trie);
        }

        public void Trie_AllContained(ITrie trie)
        {
            string[] data = {   "abc", "abd", "abe", 
                                "fonduepot", "fondue", "l",
                                "f", "0", "aaaaaaaaaaaaaaaaa",
                                "jkjkjkjkaaaaa", "aaaaaaaaa", "fabc" };
            VerifyContains(data, trie);
        }

        public void Trie_ResultingResultsResultings(ITrie trie)
        {
            string[] data = { "resulting", /*"result",*/ "results", "resultings" };
            VerifyContains(data, trie);
        }

        public void VerifyContains(string[] data, ITrie trie)
        {
            List<string> items = new List<string>(data);

            foreach (string s in items)
                trie.Add(s);

            VerifyPresence(trie, items);
        }

        protected void VerifyPresence(ITrie trie, List<string> items)
        {
            List<string> allEntries = trie.GetAllEntries();
            Assert.AreEqual(items.Count, allEntries.Count);

            foreach (string s in items)
                Assert.IsTrue(allEntries.Contains(s));
        }

        public void Trie_NotContains(ITrie trie)
        {
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

        public void Trie_SubstringNotContains(ITrie trie)
        {
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

        public void Trie_MultiNotContains(ITrie trie)
        {
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

        public void Trie_FooFonFondue(ITrie trie)
        {
            trie.Add("foo");
            trie.Add("fon");
            trie.Add("fondue");

            Assert.IsTrue(trie.Contains("foo"));
            Assert.IsTrue(trie.Contains("fon"));
            Assert.IsTrue(trie.Contains("fondue"));
            Assert.IsFalse(trie.Contains("f"));
            Assert.IsFalse(trie.Contains("fo"));
            Assert.IsFalse(trie.Contains("fond"));
            Assert.IsFalse(trie.Contains("fondu"));
        }

        //// This test can take 9+ seconds.
        //[TestMethod]
        //public void Trie_EnglishPerf()
        //{
        //    string[] strings = englishStrings;
        //    TrieNode trie = new TrieNode();
        //    foreach (string s in strings)
        //        trie.Add(s);

        //    foreach (string s in strings)
        //        Assert.IsTrue(trie.Contains(s));
        //}

        public static string[] _englishWords;

        //[ClassInitialize]
        public static void init()
        {
            Util.SetupBook();
            Util.GetManyEnglishStrings();
            _englishWords = Util.GetManyEnglishWords();
        }

        public void Trie_EnglishWordsPerf(ITrie trie)
        {
            string[] strings = _englishWords;
            foreach (string s in strings)
                trie.Add(s);
            foreach (string s in strings)
                Assert.IsTrue(trie.Contains(s));
        }
    }
}
