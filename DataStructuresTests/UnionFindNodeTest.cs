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
    public class UnionFindNodeTest
    {
        [TestMethod]
        public void UnionFindNode_CreateNode()
        {
            UnionFindNode<int> a = new UnionFindNode<int> {Label = 6};
            Assert.AreEqual(6, a.Label);
        }

        public UnionFindNode<string> CreateStringNode()
        {
            return CreateStringNode("foo");
        }

        public UnionFindNode<string> CreateStringNode(string label)
        {
            UnionFindNode<string> node = new UnionFindNode<string> {Label = label};
            Assert.AreEqual(label, node.Label);
            return node;
        }

        [TestMethod]
        public void UnionFindNode_ConfirmRootNodeIsDistanceZero()
        {
            UnionFindNode<string> node = CreateStringNode();
            Assert.AreEqual(node.DistanceFromRoot, 0);
        }

        [TestMethod]
        public void UnionFindNode_MergeTwo()
        {
            UnionFindNode<string> nodeA = CreateStringNode("A");
            UnionFindNode<string> nodeB = CreateStringNode("B");
            UnionFindNode<string>.Merge(nodeA, nodeB);
            Assert.AreEqual(nodeB.Label, nodeA.FindSet());
        }

        [TestMethod]
        public void UnionFindNode_MergeThree_AllToOne()
        {
            UnionFindNode<string> nodeA = CreateStringNode("A");
            UnionFindNode<string> nodeB = CreateStringNode("B");
            UnionFindNode<string> nodeC = CreateStringNode("C");
            UnionFindNode<string>.Merge(nodeA, nodeB);
            UnionFindNode<string>.Merge(nodeC, nodeB);
            Assert.AreEqual(nodeB.Label, nodeA.FindSet());
            Assert.AreEqual(nodeB.Label, nodeB.FindSet());
            Assert.AreEqual(nodeB.Label, nodeC.FindSet());
        }

        [TestMethod]
        public void UnionFindNode_MergeThree_Chained()
        {
            UnionFindNode<string> nodeA = CreateStringNode("A");
            UnionFindNode<string> nodeB = CreateStringNode("B");
            UnionFindNode<string> nodeC = CreateStringNode("C");
            UnionFindNode<string>.Merge(nodeA, nodeB);
            UnionFindNode<string>.Merge(nodeB, nodeC);
            Assert.AreEqual(nodeC.Label, nodeA.FindSet());
            Assert.AreEqual(nodeC.Label, nodeB.FindSet());
            Assert.AreEqual(nodeC.Label, nodeC.FindSet());
        }

        [TestMethod]
        public void UnionFindNode_MergeFour_TwoSets()
        {
            UnionFindNode<string> nodeA = CreateStringNode("A");
            UnionFindNode<string> nodeB = CreateStringNode("B");
            UnionFindNode<string> nodeC = CreateStringNode("C");
            UnionFindNode<string> nodeD = CreateStringNode("D");
            UnionFindNode<string>.Merge(nodeA, nodeB);
            UnionFindNode<string>.Merge(nodeC, nodeD);
            UnionFindNode<string>.Merge(nodeB, nodeD);
            Assert.AreEqual(nodeD.Label, nodeA.FindSet());
            Assert.AreEqual(nodeD.Label, nodeB.FindSet());
            Assert.AreEqual(nodeD.Label, nodeC.FindSet());
            Assert.AreEqual(nodeD.Label, nodeD.FindSet());
        }
    }
}
