using System;
using System.IO;
using DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataStructuresTests
{
    // A lot of this looks repetitive, but each of these tests saved bacon, so to speak.
    [TestClass]
    public class UTrieTest
    {
        [TestMethod]
        public void T_Empty()
        {
            UTree uTree = new UTree("");
        }

        [TestMethod]
        public void T_EmptyContainsNothing()
        {
            UTree uTree = new UTree("");
            Assert.IsFalse(uTree.Contains(" "));
        }

        [TestMethod]
        public void T_EmptyContains()
        {
            // searching for nothing always returns false, sorry.
            UTree uTree = new UTree("");
            Assert.IsFalse(uTree.Contains(""));
        }

        [TestMethod]
        public void T_OneCharContains()
        {
            UTree uTree = new UTree("c");
            Assert.IsTrue(uTree.Contains("c"));
        }
        [TestMethod]
        public void T_OneCharNotContains()
        {
            UTree uTree = new UTree("c");
            Assert.IsFalse(uTree.Contains("a"));
        }

        [TestMethod]
        public void T_OneCharNotContainsTwo()
        {
            UTree uTree = new UTree("c");
            Assert.IsFalse(uTree.Contains("ca"));
        }

        [TestMethod]
        public void T_TwoChar()
        {
            UTree uTree = new UTree("ca");
            Assert.IsTrue(uTree.Contains("ca"));
        }

        [TestMethod]
        public void T_TwoCharHasFirst()
        {
            UTree uTree = new UTree("ca");
            Assert.IsTrue(uTree.Contains("c"));
        }

        [TestMethod]
        public void T_TwoCharHasSecond()
        {
            UTree uTree = new UTree("ca");
            Assert.IsTrue(uTree.Contains("a"));
        }

        [TestMethod]
        public void T_ThreeCharRepeatedFirst()
        {
            UTree uTree = new UTree("cac");
            Assert.IsTrue(uTree.Contains("c"));
        }

        [TestMethod]
        public void T_ThreeCharRepeatedFirstAll()
        {
            UTree uTree = new UTree("cac");
            Assert.IsTrue(uTree.Contains("cac"));
        }

        [TestMethod]
        public void T_FourChar()
        {
            UTree uTree = new UTree("caca");
            Assert.IsTrue(uTree.Contains("a"));
            Assert.IsTrue(uTree.Contains("ca"));
            Assert.IsTrue(uTree.Contains("aca"));
            Assert.IsTrue(uTree.Contains("caca"));
        }

        [TestMethod]
        public void T_FiveChar()
        {
            UTree uTree = new UTree("cacao");
            Assert.IsTrue(uTree.Contains("o"));
            Assert.IsTrue(uTree.Contains("ao"));
            Assert.IsTrue(uTree.Contains("cao"));
            Assert.IsTrue(uTree.Contains("acao"));
            Assert.IsTrue(uTree.Contains("cacao"));
            Assert.IsTrue(uTree.Contains("ca"));
            Assert.IsTrue(uTree.Contains("cac"));
            Assert.IsTrue(uTree.Contains("c"));
        }

        [TestMethod]
        public void T_Abca()
        {
            UTree uTree = new UTree("abca");
            Assert.IsTrue(uTree.Contains("a"));
            Assert.IsTrue(uTree.Contains("b"));
            Assert.IsTrue(uTree.Contains("c"));
            Assert.IsTrue(uTree.Contains("bc"));
            Assert.IsTrue(uTree.Contains("abc"));
            Assert.IsTrue(uTree.Contains("bca"));
            Assert.IsTrue(uTree.Contains("abca"));
        }

        [TestMethod]
        public void T_Abcab()
        {
            UTree uTree = new UTree("abcab");
            Assert.IsTrue(uTree.Contains("a"));
            Assert.IsTrue(uTree.Contains("b"));
            Assert.IsTrue(uTree.Contains("c"));
            Assert.IsTrue(uTree.Contains("bc"));
            Assert.IsTrue(uTree.Contains("abc"));
            Assert.IsTrue(uTree.Contains("cab"));
            Assert.IsTrue(uTree.Contains("bca"));
            Assert.IsTrue(uTree.Contains("abca"));
            Assert.IsTrue(uTree.Contains("abcab"));
        }

        [TestMethod]
        public void T_Abcabx()
        {
            UTree uTree = new UTree("abcabx");
            Assert.IsTrue(uTree.Contains("a"));
            Assert.IsTrue(uTree.Contains("b"));
            Assert.IsTrue(uTree.Contains("c"));
            Assert.IsTrue(uTree.Contains("x"));
            Assert.IsTrue(uTree.Contains("bc"));
            Assert.IsTrue(uTree.Contains("bx"));
            Assert.IsTrue(uTree.Contains("abc"));
            Assert.IsTrue(uTree.Contains("cab"));
            Assert.IsTrue(uTree.Contains("cabx"));
            Assert.IsTrue(uTree.Contains("bca"));
            Assert.IsTrue(uTree.Contains("abca"));
            Assert.IsTrue(uTree.Contains("abcab"));
            Assert.IsTrue(uTree.Contains("abcabx"));
        }

        [TestMethod]
        public void T_Abcabxa()
        {
            UTree uTree = new UTree("abcabxa");
            Assert.IsTrue(uTree.Contains("a"));
            Assert.IsTrue(uTree.Contains("b"));
            Assert.IsTrue(uTree.Contains("c"));
            Assert.IsTrue(uTree.Contains("x"));
            Assert.IsTrue(uTree.Contains("xa"));
            Assert.IsTrue(uTree.Contains("bc"));
            Assert.IsTrue(uTree.Contains("bx"));
            Assert.IsTrue(uTree.Contains("bxa"));
            Assert.IsTrue(uTree.Contains("abc"));
            Assert.IsTrue(uTree.Contains("cab"));
            Assert.IsTrue(uTree.Contains("cabx"));
            Assert.IsTrue(uTree.Contains("bca"));
            Assert.IsTrue(uTree.Contains("bcabx"));
            Assert.IsTrue(uTree.Contains("abca"));
            Assert.IsTrue(uTree.Contains("abcab"));
            Assert.IsTrue(uTree.Contains("abcabx"));
            Assert.IsTrue(uTree.Contains("abcabxa"));
        }

        [TestMethod]
        public void T_Abcabxabc()
        {
            UTree uTree = new UTree("abcabxabc");
            Assert.IsTrue(uTree.Contains("a"));
            Assert.IsTrue(uTree.Contains("b"));
            Assert.IsTrue(uTree.Contains("c"));
            Assert.IsTrue(uTree.Contains("x"));
            Assert.IsTrue(uTree.Contains("xa"));
            Assert.IsTrue(uTree.Contains("bc"));
            Assert.IsTrue(uTree.Contains("bx"));
            Assert.IsTrue(uTree.Contains("bxa"));
            Assert.IsTrue(uTree.Contains("abc"));
            Assert.IsTrue(uTree.Contains("cab"));
            Assert.IsTrue(uTree.Contains("cabx"));
            Assert.IsTrue(uTree.Contains("bca"));
            Assert.IsTrue(uTree.Contains("bcabx"));
            Assert.IsTrue(uTree.Contains("xabc"));
            Assert.IsTrue(uTree.Contains("abca"));
            Assert.IsTrue(uTree.Contains("abcab"));
            Assert.IsTrue(uTree.Contains("abcabx"));
            Assert.IsTrue(uTree.Contains("abcabxa"));
        }

        [TestMethod]
        public void T_Abcabxabcz_MatchLength()
        {
            UTree uTree = new UTree("abcabxabcz");

            int startingIndex = -1;
            int matchLength = uTree.Match(Util.StringToBytes("abc"), 64, out startingIndex);
            Assert.AreEqual(3, matchLength);
            Assert.IsTrue(startingIndex == 0 || startingIndex == 6);

            matchLength = uTree.Match(Util.StringToBytes("a"), 64, out startingIndex);
            Assert.AreEqual(1, matchLength);
            Assert.IsTrue(startingIndex == 0 || startingIndex == 3 || startingIndex == 6);

            matchLength = uTree.Match(Util.StringToBytes("abcz"), 64, out startingIndex);
            Assert.AreEqual(4, matchLength);
            Assert.IsTrue(startingIndex == 6);

            matchLength = uTree.Match(Util.StringToBytes("m"), 64, out startingIndex);
            Assert.AreEqual(-1, matchLength);
            Assert.IsTrue(startingIndex == -1);

            matchLength = uTree.Match(Util.StringToBytes("abcab"), 64, out startingIndex);
            Assert.AreEqual(5, matchLength);
            Assert.IsTrue(startingIndex == 0);

            matchLength = uTree.Match(Util.StringToBytes("bx"), 64, out startingIndex);
            Assert.AreEqual(2, matchLength);
            Assert.IsTrue(startingIndex == 4);

            matchLength = uTree.Match(Util.StringToBytes("cz"), 64, out startingIndex);
            Assert.AreEqual(2, matchLength);
            Assert.IsTrue(startingIndex == 8);

            matchLength = uTree.Match(Util.StringToBytes("z"), 64, out startingIndex);
            Assert.AreEqual(1, matchLength);
            Assert.IsTrue(startingIndex == 9);
        }

        [TestMethod]
        public void T_Abcabxabcd()
        {
            UTree uTree = new UTree("abcabxabcd");
            Assert.IsTrue(uTree.Contains("a"));
            Assert.IsTrue(uTree.Contains("b"));
            Assert.IsTrue(uTree.Contains("c"));
            Assert.IsTrue(uTree.Contains("d"));
            Assert.IsTrue(uTree.Contains("x"));
            Assert.IsTrue(uTree.Contains("cd"));
            Assert.IsTrue(uTree.Contains("xa"));
            Assert.IsTrue(uTree.Contains("bc"));
            Assert.IsTrue(uTree.Contains("bcd"));
            Assert.IsTrue(uTree.Contains("abcd"));
            Assert.IsTrue(uTree.Contains("bx"));
            Assert.IsTrue(uTree.Contains("bxa"));
            Assert.IsTrue(uTree.Contains("abc"));
            Assert.IsTrue(uTree.Contains("cab"));
            Assert.IsTrue(uTree.Contains("cabx"));
            Assert.IsTrue(uTree.Contains("bca"));
            Assert.IsTrue(uTree.Contains("xabc"));
            Assert.IsTrue(uTree.Contains("bcabx"));
            Assert.IsTrue(uTree.Contains("abca"));
            Assert.IsTrue(uTree.Contains("abcab"));
            Assert.IsTrue(uTree.Contains("abcabx"));
            Assert.IsTrue(uTree.Contains("abcabxa"));
            Assert.IsTrue(uTree.Contains("abcabxabcd"));
        }


        [TestMethod]
        public void T_Abcabxabcd_Added()
        {
            UTree uTree = new UTree(20);
            uTree.Add('a');
            uTree.Add('b');
            uTree.Add('c');
            uTree.Add('a');
            uTree.Add('b');
            uTree.Add('x');
            uTree.Add('a');
            uTree.Add('b');
            uTree.Add('c');
            uTree.Add('d');
            Assert.IsTrue(uTree.Contains("a"));
            Assert.IsTrue(uTree.Contains("b"));
            Assert.IsTrue(uTree.Contains("c"));
            Assert.IsTrue(uTree.Contains("d"));
            Assert.IsTrue(uTree.Contains("x"));
            Assert.IsTrue(uTree.Contains("cd"));
            Assert.IsTrue(uTree.Contains("xa"));
            Assert.IsTrue(uTree.Contains("bc"));
            Assert.IsTrue(uTree.Contains("bcd"));
            Assert.IsTrue(uTree.Contains("abcd"));
            Assert.IsTrue(uTree.Contains("bx"));
            Assert.IsTrue(uTree.Contains("bxa"));
            Assert.IsTrue(uTree.Contains("abc"));
            Assert.IsTrue(uTree.Contains("cab"));
            Assert.IsTrue(uTree.Contains("cabx"));
            Assert.IsTrue(uTree.Contains("bca"));
            Assert.IsTrue(uTree.Contains("xabc"));
            Assert.IsTrue(uTree.Contains("bcabx"));
            Assert.IsTrue(uTree.Contains("abca"));
            Assert.IsTrue(uTree.Contains("abcab"));
            Assert.IsTrue(uTree.Contains("abcabx"));
            Assert.IsTrue(uTree.Contains("abcabxa"));
            Assert.IsTrue(uTree.Contains("abcabxabcd"));
        }

        [TestMethod]
        public void T_Fourpete()
        {
            UTree uTree = new UTree("aaaa");
            Assert.IsTrue(uTree.Contains("a"));
            Assert.IsTrue(uTree.Contains("aa"));
            Assert.IsTrue(uTree.Contains("aaa"));
            Assert.IsTrue(uTree.Contains("aaaa"));
        }

        [TestMethod]
        public void T_FourpeteFiveExpectNo()
        {
            UTree uTree = new UTree("aaaa");
            Assert.IsFalse(uTree.Contains("aaaaa"));
        }

        [TestMethod]
        public void T_Cacaoract()
        {
            UTree uTree = new UTree("cacaoract");
            Assert.IsTrue(uTree.Contains("cacaoract"));
            Assert.IsTrue(uTree.Contains("ract"));
            Assert.IsTrue(uTree.Contains("orac"));
            Assert.IsFalse(uTree.Contains("acaoractc"));
            Assert.IsFalse(uTree.Contains("ctc"));
            Assert.IsFalse(uTree.Contains("actc"));
            Assert.IsFalse(uTree.Contains("aa"));
            Assert.IsFalse(uTree.Contains("rr"));
            Assert.IsFalse(uTree.Contains("tt"));
            Assert.IsFalse(uTree.Contains("cc"));
            Assert.IsFalse(uTree.Contains("oo"));
            Assert.IsFalse(uTree.Contains("acc"));
            Assert.IsFalse(uTree.Contains("cca"));
        }

        [TestMethod]
        public void T_CacaoDoubles()
        {
            UTree uTree = new UTree("cacao");
            Assert.IsFalse(uTree.Contains("aa"));
            Assert.IsFalse(uTree.Contains("cc"));
            Assert.IsFalse(uTree.Contains("oo"));
        }

        [TestMethod]
        public void T_CacaocNoDoubles()
        {
            UTree uTree = new UTree("cacaoc");
            Assert.IsFalse(uTree.Contains("aa"));
            Assert.IsFalse(uTree.Contains("cc"));
            Assert.IsFalse(uTree.Contains("oo"));
        }

        [TestMethod]
        public void T_CacaoctNoDoubles()
        {
            UTree uTree = new UTree("Cacaoct");
            Assert.IsFalse(uTree.Contains("cc"));
            Assert.IsFalse(uTree.Contains("aa"));
            Assert.IsFalse(uTree.Contains("oo"));
            Assert.IsFalse(uTree.Contains("tt"));
        }

        [TestMethod]
        public void T_CacoctNoDoubles()
        {
            UTree uTree = new UTree("cacoct");
            Assert.IsFalse(uTree.Contains("aa"));
            Assert.IsFalse(uTree.Contains("cc"));
            Assert.IsFalse(uTree.Contains("oo"));
            Assert.IsFalse(uTree.Contains("tt"));
        }

        [TestMethod]
        public void T_JustAddALot()
        {
            UTree uTree = new UTree(20000);
            for (int i = 0; i < 5000; i++)
            {
                uTree.Add((byte)(i%256));
            }
        }

        [TestMethod]
        public void T_JustAddALotRandom()
        {
            Random rand = new Random();
            UTree uTree = new UTree(20000);
            for (int i = 0; i < 5000; i++)
            {
                uTree.Add((byte)(rand.Next() % 256));
            }
        }

        [TestMethod]
        public void T_StartWithBlock()
        {
            byte[] chunk = new byte[5000];
            for (int i = 0; i < 5000; i++)
            {
                chunk[i] = (byte)(i % 256);
            }
            UTree uTree = new UTree(chunk);
        }

        [TestMethod]
        public void T_StartWithLargeRandomBlock()
        {
            Random rand = new Random();
            byte[] chunk = new byte[25000];
            for (int i = 0; i < 25000; i++)
            {
                chunk[i] = (byte)(rand.Next() % 256);
            }
            UTree uTree = new UTree(chunk);
        }

        [TestMethod]
        public void T_StartWithLargeRandomBlock2()
        {
            Random rand = new Random();
            byte[] chunk = new byte[20000];
            for (int i = 0; i < 20000; i++)
            {
                chunk[i] = (byte)(rand.Next() % 256);
            }
            UTree uTree = new UTree(chunk);
        }

        // Shakespeare. Sonnet #1.
        [TestMethod]
        public void T_Sonnet()
        {
            UTree uTree = new UTree(@"From fairest creatures we desire increase,
        That thereby beauty's rose might never die,
        But as the riper should by time decease,
        His tender heir might bear his memory:
        But thou contracted to thine own bright eyes,
        Feed'st thy light's flame with self-substantial fuel,
        Making a famine where abundance lies,
        Thy self thy foe, to thy sweet self too cruel:
        Thou that art now the world's fresh ornament,
        And only herald to the gaudy spring,
        Within thine own bud buriest thy content,
        And tender churl mak'st waste in niggarding:
          Pity the world, or else this glutton be,
          To eat the world's due, by the grave and thee.");
            Assert.IsTrue(uTree.Contains("From"));
            Assert.IsTrue(uTree.Contains("eat"));
            Assert.IsTrue(uTree.Contains("Pity"));
            Assert.IsTrue(uTree.Contains("world's"));
            Assert.IsFalse(uTree.Contains("oorrnnaammeenntt"));
        }
    }
}
