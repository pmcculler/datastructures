using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataStructuresTests;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            BurstBurstTrieTest test = new BurstBurstTrieTest();
            BurstBurstTrieTest.init();

            test.BurstTrie_SortEnglishWords();
        }
    }
}
