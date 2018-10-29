using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataStructuresTests
{
    [TestClass]
    public class CompactTrieNodeTest
    {
        private static Random _random = new Random(5555);

        private static TrieNode _largeTrieNode;
        private static Trie _largeTrie;
        private static BurstTrie _largeBurstTrie;
        private static TrieNode _wordsLargeTrieNode;
        private static Trie _wordsLargeTrie;
        private static BurstTrie _wordsLargeBurstTrie;

        [ClassInitialize]
        public static void init(TestContext ignored)
        {
            _largeTrieNode = new TrieNode();
            FillInLargeTrie(_largeTrieNode);
            _largeTrie = new Trie();
            FillInLargeTrie(_largeTrie);
            _largeBurstTrie = new BurstTrie();
            FillInLargeTrie(_largeBurstTrie);
            ITrieTest.init();
            _wordsLargeTrie = new Trie();
            _wordsLargeBurstTrie = new BurstTrie();
            _wordsLargeTrieNode = new TrieNode();
        }

        [TestMethod]
        public void MeasureLargeTrieNode()
        {
            MeasureSizeOfLargeTrie(_largeTrieNode);
        }

        [TestMethod]
        public void MeasureLargeTrie()
        {
            MeasureSizeOfLargeTrie(_largeTrie);
        }

        [TestMethod]
        public void MeasureLargeBurstTrie()
        {
            MeasureSizeOfLargeTrie(_largeBurstTrie);
        }

        [TestMethod]
        public void Trie_SpeedTestVerify_LargeTrieNode()
        {
            SpeedTestVerify(_largeTrieNode);
        }

        [TestMethod]
        public void Trie_SpeedTestVerify_LargeTrie()
        {
            SpeedTestVerify(_largeTrie);
        }

        [TestMethod]
        public void Trie_SpeedTestVerify_LargeBurstTrie()
        {
            SpeedTestVerify(_largeBurstTrie);
        }
        [TestMethod]
        public void Trie_SpeedTestIntake_LargeBurstTrie()
        {
            SpeedTestIntake(new BurstTrie());
        }
        [TestMethod]
        public void Trie_SpeedTestIntake_LargeTrie()
        {
            SpeedTestIntake(new Trie());
        }
        [TestMethod]
        public void Trie_SpeedTestIntake_LargeTrieNode()
        {
            SpeedTestIntake(new TrieNode());
        }
        [TestMethod]
        public void Trie_SpeedTestIntakeWords_LargeBurstTrie()
        {
            SpeedTestIntakeWords(new BurstTrie());
        }
        [TestMethod]
        public void Trie_SpeedTestIntakeWords_LargeTrie()
        {
            SpeedTestIntakeWords(new Trie());
        }
        [TestMethod]
        public void Trie_SpeedTestIntakeWords_LargeTrieNode()
        {
            SpeedTestIntakeWords(new TrieNode());
        }

        protected void SpeedTestVerify(ITrie trie)
        {
            foreach (string address in _addresses)
            {
                Assert.IsTrue(trie.Contains(address));
            }
        }

        protected void SpeedTestIntake(ITrie trie)
        {
            foreach (string address in _addresses)
            {
                trie.Add(address);
            }
        }

        protected void SpeedTestIntakeWords(ITrie trie)
        {
            foreach (string s in ITrieTest._englishWords)
                trie.Add(s);
        }

        private static List<string> _addresses;
        private static long _addressesLength = 0;
        private static void FillInLargeTrie(ITrie trie)
        {
            _random = new Random(5555);
            _addresses = new List<string>();
            _addressesLength = 0;
            for (int i = 0; i < 10 * 1000; i++)
            {
                string address = CreateRandomEmail();
                trie.Add(address);
                _addresses.Add(address);
                _addressesLength += address.Length;
            }

            System.Console.Out.WriteLine("Size of addresses (" + _addressesLength + ").");
        }

        private void MeasureSizeOfLargeTrie(Object trie)
        {
            long trieSize = MeasureSize(trie);
            System.Console.Out.WriteLine("Size of trie: " + trieSize + " expansion factor " + (double)trieSize/(double)_addressesLength);
        }

        private long MeasureSize(object o)
        {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, o);
            return stream.Length;
        }

        private static string CreateRandomEmail()
        {
            string[] tlds = {"com", "edu", "net", "co.uk", "org"};

            string prefix = CreateRandomString(10);
            string domain = CreateRandomString(7);

            return prefix + '@' + domain + '.' + tlds[_random.Next(tlds.Length)];
        }

        private static string CreateRandomString(int length)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                builder.Append(RandomChar());
            }
            return builder.ToString();
        }

        private static char RandomChar()
        {
            return (char) (_random.Next(26) + 'a');
        }
    }
}
