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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataStructures;

namespace DataStructuresTests
{
    [TestClass]
    public class TrieNodeTest
    {
        [TestMethod]
        public void CreateTrieNode()
        {
            TrieNode trie = new TrieNode();
        }
        [TestMethod]
        public void OneTrieNode()
        {
            TrieNode trie = new TrieNode();
            trie.add("con");
        }
        [TestMethod]
        public void ContainsTrieNode()
        {
            TrieNode trie = new TrieNode();
            trie.add("con");
            Assert.IsTrue(trie.Contains("con"));
        }
        [TestMethod]
        public void NotContainsUnrelatedTrieNode()
        {
            TrieNode trie = new TrieNode();
            trie.add("a");
            Assert.IsFalse(trie.Contains("b"));
        }
        [TestMethod]
        public void BlankTrieNode()
        {
            TrieNode trie = new TrieNode();
            trie.add("");
            Assert.IsTrue(trie.Contains(""));
        }
        [TestMethod]
        public void EmptyTrieNode()
        {
            TrieNode trie = new TrieNode();
            Assert.IsFalse(trie.Contains(""));
        }
        [TestMethod]
        public void AllContainedTrieNode()
        {
            TrieNode trie = new TrieNode();
            string[] data = {   "abc", "abd", "abe", 
                                "fonduepot", "fondue", "l",
                                "f", "0", "aaaaaaaaaaaaaaaaa",
                                "jkjkjkjkaaaaa", "aaaaaaaaa", "fabc" };
            List<string> items = new List<string>(data);

            foreach (string s in items)
                trie.add(s);

            verifyPresence(trie, items);
        }
        protected void verifyPresence(TrieNode trie, List<string> items)
        {
            List<string> allEntries = trie.getAllEntries();
            Assert.AreEqual(allEntries.Count, items.Count);

            foreach (string s in items)
                Assert.IsTrue(allEntries.Contains(s));
        }

        [TestMethod]
        public void NotContainsTrieNode()
        {
            TrieNode trie = new TrieNode();
            trie.add("con");
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
        public void SubstringNotContainsTrieNode()
        {
            TrieNode trie = new TrieNode();
            trie.add("abcde");
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
        public void MultiNotContainsTrieNode()
        {
            TrieNode trie = new TrieNode();
            trie.add("con");
            trie.add("cono");
            trie.add("foo");
            trie.add("fon");
            trie.add("fondue");

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
    }
}
