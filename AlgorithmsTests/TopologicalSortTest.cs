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
using Algorithms;

namespace AlgorithmsTests
{
    [TestClass]
    public class TopologicalSortTest
    {
        [TestMethod]
        public void TopoTest_Empty()
        {
            TopologicalSort.Sort(new List<TopologicalSort.Node>());
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TopoTest_Null()
        {
            TopologicalSort.Sort(null);        
        }
        [TestMethod]
        public void TopoTest_One()
        {
            TopologicalSort.Node node = new TopologicalSort.Node();
            List<TopologicalSort.Node> nodes = new List<TopologicalSort.Node> {node};

            List<TopologicalSort.Node> sortedNodes = TopologicalSort.Sort(nodes);
            Assert.AreEqual(sortedNodes[0], nodes[0]);
        }
        [TestMethod]
        public void TopoTest_Two()
        {
            TopologicalSort.Node a = new TopologicalSort.Node();
            TopologicalSort.Node b = new TopologicalSort.Node();
            a.Dependencies.Add(b);
            List<TopologicalSort.Node> nodes = new List<TopologicalSort.Node> {a, b};

            List<TopologicalSort.Node> sortedNodes = TopologicalSort.Sort(nodes);
            Assert.AreEqual(sortedNodes[0], b);
            Assert.AreEqual(sortedNodes[1], a);
        }
        [TestMethod]
        public void TopoTest_Three()
        {
            TopologicalSort.Node a = new TopologicalSort.Node();
            TopologicalSort.Node b = new TopologicalSort.Node();
            TopologicalSort.Node c = new TopologicalSort.Node();
            a.Dependencies.Add(b);
            b.Dependencies.Add(c);
            List<TopologicalSort.Node> nodes = new List<TopologicalSort.Node> {a, b, c};
            List<TopologicalSort.Node> sortedNodes = TopologicalSort.Sort(nodes);
            Assert.AreEqual(sortedNodes[0], c);
            Assert.AreEqual(sortedNodes[1], b);
            Assert.AreEqual(sortedNodes[2], a);
        }
        [TestMethod]
        public void TopoTest_Multiple()
        {
            TopologicalSort.Node a = new TopologicalSort.Node();
            TopologicalSort.Node b = new TopologicalSort.Node();
            TopologicalSort.Node c = new TopologicalSort.Node();
            TopologicalSort.Node d = new TopologicalSort.Node();
            a.Dependencies.Add(b);
            a.Dependencies.Add(c);
            b.Dependencies.Add(d);
            List<TopologicalSort.Node> nodes = new List<TopologicalSort.Node> {a, b, c, d};
            List<TopologicalSort.Node> sortedNodes = TopologicalSort.Sort(nodes);
            Assert.IsTrue(sortedNodes.IndexOf(d) < sortedNodes.IndexOf(b));
            Assert.IsTrue(sortedNodes.IndexOf(b) < sortedNodes.IndexOf(a));
            Assert.IsTrue(sortedNodes.IndexOf(c) < sortedNodes.IndexOf(a));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TopoTest_Circular()
        {
            TopologicalSort.Node a = new TopologicalSort.Node();
            TopologicalSort.Node b = new TopologicalSort.Node();
            a.Dependencies.Add(b);
            b.Dependencies.Add(a);
            List<TopologicalSort.Node> nodes = new List<TopologicalSort.Node> {a, b};
            // ReSharper disable once UnusedVariable
            List<TopologicalSort.Node> sortedNodes = TopologicalSort.Sort(nodes);
        }

        [TestMethod]
        public void TopoTest_NoDependencies()
        {
            TopologicalSort.Node a = new TopologicalSort.Node();
            TopologicalSort.Node b = new TopologicalSort.Node();
            List<TopologicalSort.Node> nodes = new List<TopologicalSort.Node> {a, b};
            List<TopologicalSort.Node> sortedNodes = TopologicalSort.Sort(nodes);
            Assert.IsTrue(sortedNodes.Contains(a));
            Assert.IsTrue(sortedNodes.Contains(b));
        }
    }
}
