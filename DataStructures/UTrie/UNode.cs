using System;

namespace DataStructures
{
    public class UNode
    {
        // A dictionary<byte,UEdge> is also an option, but I found it was usually a little slower.
        public UEdge[] Edges => _edges.Value;
        public UNode SuffixLink { get; set; }

        private readonly Lazy<UEdge[]> _edges = new Lazy<UEdge[]>( () => new UEdge[256]);
    }
}
