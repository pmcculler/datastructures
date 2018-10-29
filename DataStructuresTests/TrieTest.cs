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
    public class TrieTest
    {
        [TestMethod]
        public void Trie_Create()
        {
            new ITrieTest().Trie_Create(new Trie());
        }

        [TestMethod]
        public void Trie_One()
        {
            new ITrieTest().Trie_One(new Trie());
        }

        [TestMethod]
        public void Trie_Contains()
        {
            new ITrieTest().Trie_Contains(new Trie());
        }

        [TestMethod]
        public void Trie_NotContainsUnrelated()
        {
            new ITrieTest().Trie_NotContainsUnrelated(new Trie());
        }

        [TestMethod]
        public void Trie_Blank()
        {
            new ITrieTest().Trie_Blank(new Trie());
        }

        [TestMethod]
        public void Trie_Empty()
        {
            new ITrieTest().Trie_Empty(new Trie());
        }

        [TestMethod]
        public void Trie_AllContained()
        {
            new ITrieTest().Trie_AllContained(new Trie());
        }

        [TestMethod]
        public void Trie_NotContains()
        {
            new ITrieTest().Trie_NotContains(new Trie());
        }

        [TestMethod]
        public void Trie_SubstringNotContains()
        {
            new ITrieTest().Trie_SubstringNotContains(new Trie());
        }

        [TestMethod]
        public void Trie_MultiNotContains()
        {
            new ITrieTest().Trie_MultiNotContains(new Trie());
        }

        [ClassInitialize]
        public static void init(TestContext contextIgnored)
        {
            ITrieTest.init();
        }

        [TestMethod]
        public void Trie_EnglishWordsPerf()
        {
            Trie trie = new Trie();
            new ITrieTest().Trie_EnglishWordsPerf(trie);
            trie.Visit();
            int breakHere = 1;
        }

        [TestMethod]
        public void Trie_SingleContained()
        {
            new ITrieTest().Trie_SingleContained(new Trie());
        }

        [TestMethod]
        public void Trie_SingleContainedOppositeOrder()
        {
            new ITrieTest().Trie_SingleContainedOppositeOrder(new Trie());
        }

        [TestMethod]
        public void Trie_ResultingResultsResultings()
        {
            new ITrieTest().Trie_ResultingResultsResultings(new Trie());
        }

        [TestMethod]
        public void Trie_FooFonFondue()
        {
            new ITrieTest().Trie_FooFonFondue(new Trie());
        }
    }
}
