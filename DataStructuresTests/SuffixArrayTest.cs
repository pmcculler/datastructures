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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataStructures;

namespace DataStructuresTests
{
    [TestClass]
    public class SuffixArrayTest
    {
        [TestMethod]
        public void CreateSuffixArray()
        {
            SuffixArray sa = new SuffixArray("");
        }
        [TestMethod]
        public void DeadSuffixArray()
        {
            SuffixArray sa = new SuffixArray();
        }
        [TestMethod]
        public void EmptySuffixArray()
        {
            SuffixArray sa = new SuffixArray("");
            Assert.IsFalse(sa.Contains("a"));
        }
        [TestMethod]
        public void ItemsSuffixArray()
        {
            SuffixArray sa = new SuffixArray("items");
            Assert.IsTrue(sa.Contains("items"));
            Assert.IsTrue(sa.Contains("item"));
            Assert.IsTrue(sa.Contains("ite"));
            Assert.IsTrue(sa.Contains("it"));
            Assert.IsTrue(sa.Contains("i"));
            Assert.IsTrue(sa.Contains(""));
            Assert.IsTrue(sa.Contains("tems"));
            Assert.IsTrue(sa.Contains("ems"));
            Assert.IsTrue(sa.Contains("ms"));
            Assert.IsTrue(sa.Contains("s"));
            Assert.IsTrue(sa.Contains("tem"));
            Assert.IsTrue(sa.Contains("t"));
            Assert.IsTrue(sa.Contains("e"));
            Assert.IsTrue(sa.Contains("m"));
        }
        [TestMethod]
        public void NotItemsSuffixArray()
        {
            SuffixArray sa = new SuffixArray("items");
            for (int i = 0; i < 20; i++)
            {
                Assert.IsFalse(sa.Contains(i.ToString()));
            }
        }
        [TestMethod]
        public void NotBackwardsItemsSuffixArray()
        {
            SuffixArray sa = new SuffixArray("items");
            Assert.IsFalse(sa.Contains("met"));
        }
        [TestMethod]
        public void IndexSuffixArray()
        {
            SuffixArray sa = new SuffixArray("items");
            Assert.AreEqual(0, sa.Find("items"));
            Assert.AreEqual(1, sa.Find("tems"));
            Assert.AreEqual(2, sa.Find("ems"));
            Assert.AreEqual(3, sa.Find("ms"));
            Assert.AreEqual(4, sa.Find("s"));
        }
        [TestMethod]
        public void MultiIndexSuffixArray()
        {
            SuffixArray sa = new SuffixArray("itemsitems");
            Assert.IsTrue(0 <= sa.Find("items"));
            Assert.IsTrue(1 <= sa.Find("tems"));
            Assert.IsTrue(2 <= sa.Find("ems"));
            Assert.IsTrue(3 <= sa.Find("ms"));
            Assert.IsTrue(4 <= sa.Find("s"));
        }
    }
}
