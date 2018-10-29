using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataStructuresTests
{
    [TestClass]
    public class TrieNodeTest
    {
        [TestMethod]
        public void TrieNode_Create()
        {
            new ITrieTest().Trie_Create(new TrieNode());
        }

        [TestMethod]
        public void TrieNode_One()
        {
            new ITrieTest().Trie_One(new TrieNode());
        }

        [TestMethod]
        public void TrieNode_Contains()
        {
            new ITrieTest().Trie_Contains(new TrieNode());
        }

        [TestMethod]
        public void TrieNode_NotContainsUnrelated()
        {
            new ITrieTest().Trie_NotContainsUnrelated(new TrieNode());
        }

        [TestMethod]
        public void TrieNode_Blank()
        {
            new ITrieTest().Trie_Blank(new TrieNode());
        }

        [TestMethod]
        public void TrieNode_Empty()
        {
            new ITrieTest().Trie_Empty(new TrieNode());
        }

        [TestMethod]
        public void TrieNode_AllContained()
        {
            new ITrieTest().Trie_AllContained(new TrieNode());
        }

        [TestMethod]
        public void TrieNode_NotContains()
        {
            new ITrieTest().Trie_NotContains(new TrieNode());
        }

        [TestMethod]
        public void TrieNode_SubstringNotContains()
        {
            new ITrieTest().Trie_SubstringNotContains(new TrieNode());
        }

        [TestMethod]
        public void TrieNode_MultiNotContains()
        {
            new ITrieTest().Trie_MultiNotContains(new TrieNode());
        }

        [ClassInitialize]
        public static void init(TestContext contextIgnored)
        {
            ITrieTest.init();
        }

        [TestMethod]
        public void TrieNode_EnglishWordsPerf()
        {
            new ITrieTest().Trie_EnglishWordsPerf(new TrieNode());
        }

        [TestMethod]
        public void TrieNode_SingleContained()
        {
            new ITrieTest().Trie_SingleContained(new TrieNode());
        }

        [TestMethod]
        public void TrieNode_SingleContainedOppositeOrder()
        {
            new ITrieTest().Trie_SingleContainedOppositeOrder(new TrieNode());
        }

        [TestMethod]
        public void TrieNode_ResultingResultsResultings()
        {
            new ITrieTest().Trie_ResultingResultsResultings(new TrieNode());
        }

        [TestMethod]
        public void Trie_FooFonFondue()
        {
            new ITrieTest().Trie_FooFonFondue(new TrieNode());
        }

    }
}
