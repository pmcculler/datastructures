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
            TopologicalSort.sort(new List<TopologicalSort.Node>());
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TopoTest_Null()
        {
            TopologicalSort.sort(null);        
        }
        [TestMethod]
        public void TopoTest_One()
        {
            TopologicalSort.Node node = new TopologicalSort.Node();
            List<TopologicalSort.Node> nodes = new List<TopologicalSort.Node>();
            nodes.Add(node);

            List<TopologicalSort.Node> sortedNodes = TopologicalSort.sort(nodes);
            Assert.AreEqual(sortedNodes[0], nodes[0]);
        }
        [TestMethod]
        public void TopoTest_Two()
        {
            TopologicalSort.Node a = new TopologicalSort.Node();
            TopologicalSort.Node b = new TopologicalSort.Node();
            a.dependencies.Add(b);
            List<TopologicalSort.Node> nodes = new List<TopologicalSort.Node>();
            nodes.Add(a);
            nodes.Add(b);

            List<TopologicalSort.Node> sortedNodes = TopologicalSort.sort(nodes);
            Assert.AreEqual(sortedNodes[0], b);
            Assert.AreEqual(sortedNodes[1], a);
        }
        [TestMethod]
        public void TopoTest_Three()
        {
            TopologicalSort.Node a = new TopologicalSort.Node();
            TopologicalSort.Node b = new TopologicalSort.Node();
            TopologicalSort.Node c = new TopologicalSort.Node();
            a.dependencies.Add(b);
            b.dependencies.Add(c);
            List<TopologicalSort.Node> nodes = new List<TopologicalSort.Node>();
            nodes.Add(a);
            nodes.Add(b);
            nodes.Add(c);
            List<TopologicalSort.Node> sortedNodes = TopologicalSort.sort(nodes);
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
            a.dependencies.Add(b);
            a.dependencies.Add(c);
            b.dependencies.Add(d);
            List<TopologicalSort.Node> nodes = new List<TopologicalSort.Node>();
            nodes.Add(a);
            nodes.Add(b);
            nodes.Add(c);
            nodes.Add(d);
            List<TopologicalSort.Node> sortedNodes = TopologicalSort.sort(nodes);
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
            a.dependencies.Add(b);
            b.dependencies.Add(a);
            List<TopologicalSort.Node> nodes = new List<TopologicalSort.Node>();
            nodes.Add(a);
            nodes.Add(b);
            List<TopologicalSort.Node> sortedNodes = TopologicalSort.sort(nodes);
        }
        [TestMethod]
        public void TopoTest_NoDependencies()
        {
            TopologicalSort.Node a = new TopologicalSort.Node();
            TopologicalSort.Node b = new TopologicalSort.Node();
            List<TopologicalSort.Node> nodes = new List<TopologicalSort.Node>();
            nodes.Add(a);
            nodes.Add(b);
            List<TopologicalSort.Node> sortedNodes = TopologicalSort.sort(nodes);
            Assert.IsTrue(sortedNodes.Contains(a));
            Assert.IsTrue(sortedNodes.Contains(b));
        }
    }
}
