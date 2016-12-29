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

namespace Algorithms
{
    public class TopologicalSort
    {
        public class Node
        {
            // "I am dependent on these nodes..."
            public List<Node> Dependencies = new List<Node>();

            // toposort attributes
            public bool tempMark;
            public bool permMark;
        }

        /// <summary>
        /// linear: O(n), n = number of nodes
        /// Implements Tarjan, Robert E. (1976), 
        /// "Edge-disjoint spanning trees and depth-first search",
        /// Acta Informatica 6 (2): 171–185, doi:10.1007/BF00268499
        /// </summary>
        /// <param name="graph">a directed acyclic graph</param>
        /// <returns>list of graph nodes in topological order</returns>
        public static List<Node> Sort(List<Node> graph)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            List<Node> result = new List<Node>();

            Action<Node> search = null; // separate defition from assignment to allow recursion
            bool notAdag = false;

            search = item =>
            {
                if (item.tempMark)
                {
                    notAdag = true;
                    return;  // this is not a DAG!
                }
                if (!item.permMark)
                {
                    item.tempMark = true;
                    foreach (Node dependency in item.Dependencies)
                        search(dependency);
                    item.permMark = true;
                    item.tempMark = false;
                    result.Add(item);
                }
            };

            // actually we only need to do this for unmarked items
            foreach (var item in graph)
                search(item);

            if (notAdag)
                throw new ArgumentException("Must be an acyclic graph.", nameof(graph));

            return result;
        }
    }
}
