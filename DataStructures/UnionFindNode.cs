using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    public class UnionFindNode<T>
    {
            public UnionFindNode<T> Parent { get; set; }
            public T Label { get; set; }
            public int DistanceFromRoot { get; set; }        

            public T FindSet()
            {
                if (Parent == null)
                {
                    return Label;
                }
                
                return FindRootNode().Label;
            }

            private UnionFindNode<T> FindRootNode()
            {
                if (Parent == null)
                {
                    return this;
                }
                Parent = Parent.FindRootNode();
                DistanceFromRoot = 1;
                return Parent;
            }

        public static void Merge(UnionFindNode<T> fromLeft, UnionFindNode<T> toRight)
        {
            UnionFindNode<T> source = fromLeft.DistanceFromRoot > toRight.DistanceFromRoot ? fromLeft : toRight;
            UnionFindNode<T> destination = source == fromLeft ? toRight : fromLeft;
            source.FindRootNode().Parent = destination;
            source.DistanceFromRoot++;
            destination.FindRootNode().Label = toRight.Label;
        }
    }
}
