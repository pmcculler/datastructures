﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    public class TopologicalSort
    {
        public class Node
        {
            // "I am dependent on these nodes..."
            public List<Node> dependencies = new List<Node>();

            // toposort attributes
            public bool tempMark = false;
            public bool permMark = false;
        }

        public static List<Node> sort(List<Node> graph)
        {
            if (graph == null)
                throw new ArgumentNullException("graph");
            return TopologicalSort.sort(prepGraph(graph));
        }

        /// <summary>
        /// Creates a dictionary mapping nodes to dependencies.
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private static IDictionary<Node, List<Node>> prepGraph(List<Node> nodes)
        {
            Dictionary<Node, List<Node>> graph = new Dictionary<Node, List<Node>>();
            foreach (Node n in nodes)
                graph[n] = n.dependencies;
            return graph;
        }

        /// <summary>
        /// linear: O(n), n = number of nodes
        /// Implements Tarjan, Robert E. (1976), 
        /// "Edge-disjoint spanning trees and depth-first search",
        /// Acta Informatica 6 (2): 171–185, doi:10.1007/BF00268499
        /// </summary>
        /// <param name="graph">a directed acyclic graph</param>
        /// <returns>list of graph nodes in topological order</returns>
        private static List<Node> sort(IDictionary<Node, List<Node>> graph)
        {
            if (graph == null)
                throw new ArgumentNullException("graph");

            List<Node> result = new List<Node>();

            Action<Node> search = null; // separate defition from assignment to allow recursion
            bool notADAG = false;

            search = item =>
            {
                if (item.tempMark)
                {
                    notADAG = true;
                    return;  // this is not a DAG!
                }
                if (!item.permMark)
                {
                    item.tempMark = true;
                    foreach (Node dependency in item.dependencies)
                        search(dependency);
                    item.permMark = true;
                    item.tempMark = false;
                    result.Add(item);
                }
            };

            // actually we only need to do this for unmarked items
            foreach (var item in graph.Keys.ToList())
                search(item);

            if (notADAG)
                throw new ArgumentException("Must be an acyclic graph.", "graph");

            return result;
        }
    }
}
