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
using System.Diagnostics;
using DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataStructuresTests
{
    [TestClass]
    public class CuckooHashTest
    {
        [TestMethod]
        public void CuckooHash_Compare_Large_Dictionary_Miss_Speeds()
        {
            int howLarge = 50000;
            Stopwatch cuckooStopwatch = new Stopwatch();
            CuckooHash<string,string> cuckooHash = new CuckooHash<string, string>();
            new IDictionaryTest().Insert_Random(cuckooHash, howLarge);
            cuckooStopwatch.Start();
            for (int i = 0; i < howLarge; i++)
            {
                // ReSharper disable once UnusedVariable
                bool holder = cuckooHash.ContainsKey(Util.RandomString(10));
            }
            cuckooStopwatch.Stop();
            Stopwatch dictionaryStopwatch = new Stopwatch();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            new IDictionaryTest().Insert_Random(dictionary, howLarge);
            dictionaryStopwatch.Start();
            for (int i = 0; i < howLarge; i++)
            {
                // ReSharper disable once UnusedVariable
                bool holder = cuckooHash.ContainsKey(Util.RandomString(10));
            }
            dictionaryStopwatch.Stop();
            Console.Out.WriteLine("Cuckoo hash took " + cuckooStopwatch.ElapsedMilliseconds + " milliseconds.");
            Console.Out.WriteLine("Dictionary took " + dictionaryStopwatch.ElapsedMilliseconds + " milliseconds.");
            Console.Out.WriteLine("Difference is " + (Math.Max(cuckooStopwatch.ElapsedMilliseconds, dictionaryStopwatch.ElapsedMilliseconds) - Math.Min(cuckooStopwatch.ElapsedMilliseconds, dictionaryStopwatch.ElapsedMilliseconds)) + " milliseconds.");
        }

        [TestMethod]
        public void CuckooHash_Compare_Dictionary_Insert_Speeds()
        {
            int howLarge = 20000;
            Stopwatch cuckooStopwatch = new Stopwatch();
            CuckooHash<string, string> cuckooHash = new CuckooHash<string, string>();
            cuckooStopwatch.Start();
            new IDictionaryTest().Insert_Random(cuckooHash, howLarge);
            cuckooStopwatch.Stop();
            Stopwatch dictionaryStopwatch = new Stopwatch();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionaryStopwatch.Start();
            new IDictionaryTest().Insert_Random(dictionary, howLarge);
            dictionaryStopwatch.Stop();
            Console.Out.WriteLine("Cuckoo hash took " + cuckooStopwatch.ElapsedMilliseconds + " milliseconds.");
            Console.Out.WriteLine("Dictionary took " + dictionaryStopwatch.ElapsedMilliseconds + " milliseconds.");
            Console.Out.WriteLine("Difference is " + (Math.Max(cuckooStopwatch.ElapsedMilliseconds, dictionaryStopwatch.ElapsedMilliseconds) - Math.Min(cuckooStopwatch.ElapsedMilliseconds, dictionaryStopwatch.ElapsedMilliseconds)) + " milliseconds.");
        }

        [TestMethod]
        public void CuckooHash_Insert_Random_Very_Large()
        {
            new IDictionaryTest().Insert_Random(new CuckooHash<string, string>(), 100000);
        }

        [TestMethod]
        public void CuckooHash_Count_Up_Large_From_Zero()
        {
            new IDictionaryTest().Count_Up_Large_From_Zero(new CuckooHash<int, string>());
        }

        [TestMethod]
        public void CuckooHash_Count_Up_Large_From_Random()
        {
            new IDictionaryTest().Count_Up_Large_From_Random(new CuckooHash<int, string>());
        }

        [TestMethod]
        public void CuckooHash_Count_Up_Small()
        {
            new IDictionaryTest().Count_Up_Small(new CuckooHash<int, string>());
        }

        [TestMethod]
        public void CuckooHash_Count_Random()
        {
            new IDictionaryTest().Count_Random(new CuckooHash<int, string>());
        }

        [TestMethod]
        public void CuckooHash_IsReadOnlyReturnsFalse()
        {
            new IDictionaryTest().IsReadOnlyReturnsFalse(new CuckooHash<int, int>());
        }

        [TestMethod]
        public void CuckooHash_Add_And_Get()
        {
            new IDictionaryTest().Add_And_Get(new CuckooHash<int, string>());
        }

        [TestMethod]
        public void CuckooHash_Add_Many_Times_Remove_Once()
        {
            new IDictionaryTest().Add_Many_Times_Remove_Once(new CuckooHash<int, string>());
        }

        [TestMethod]
        public void CuckooHash_Enumerate()
        {
            new IDictionaryTest().Enumerate(new CuckooHash<int, string>());
        }

        [TestMethod]
        public void CuckooHash_EnumerateKeys()
        {
            new IDictionaryTest().EnumerateKeys(new CuckooHash<int, string>());
        }

        [TestMethod]
        public void CuckooHash_EnumerateValues()
        {
            new IDictionaryTest().EnumerateValues(new CuckooHash<int, string>());
        }

        [TestMethod]
        public void CuckooHash_Add_Remove_Lots_To_Test_Shrink()
        {
            CuckooHash<int, string> hash = new CuckooHash<int, string>();
            int originalNumberOfStorageSlots = hash.StorageSlots;
            new IDictionaryTest().Add_Remove_Lots_To_Test_Shrink(hash);
            int newNumberOfStorageSlots = hash.StorageSlots;
            Assert.IsTrue(newNumberOfStorageSlots <= (originalNumberOfStorageSlots * 2));
        }

        [TestMethod]
        public void CuckooHash_Add_And_TryGet()
        {
            new IDictionaryTest().Add_And_TryGet(new CuckooHash<int, string>());
        }

        [TestMethod]
        public void CuckooHash_Add_And_Remove()
        {
            new IDictionaryTest().Add_And_Remove(new CuckooHash<int, string>());
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void CuckooHash_Add_Remove_Get_Throws()
        {
            new IDictionaryTest().Add_Remove_Get_Throws(new CuckooHash<int, string>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CuckooHash_Add_Null_Throws()
        {
            new IDictionaryTest().Add_Null_Throws(new CuckooHash<string, string>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CuckooHash_Get_Null_Throws()
        {
            new IDictionaryTest().Get_Null_Throws(new CuckooHash<string, string>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CuckooHash_Get_Empty_Null_Throws()
        {
            new IDictionaryTest().Get_Empty_Null_Throws(new CuckooHash<string, string>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CuckooHash_Get_Try_Null_Throws()
        {
            new IDictionaryTest().Get_Try_Null_Throws(new CuckooHash<string, string>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CuckooHash_Get_Try_Emtpy_Null_Throws()
        {
            new IDictionaryTest().Get_Try_Emtpy_Null_Throws(new CuckooHash<string, string>());
        }

        [TestMethod]
        public void CuckooHash_Clear_Empty()
        {
            new IDictionaryTest().Clear_Empty(new CuckooHash<string, string>());
        }

        [TestMethod]
        public void CuckooHash_Clear()
        {
            new IDictionaryTest().Clear(new CuckooHash<string, string>());
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void CuckooHash_Clear_Then_Get_Throws()
        {
            new IDictionaryTest().Clear_Then_Get_Throws(new CuckooHash<string, string>());
        }
    }
}
