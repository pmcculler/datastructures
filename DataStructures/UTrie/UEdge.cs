
namespace DataStructures
{
    public class UEdge
    {
        public short From { get; }
        public short To { get; }
        public UNode Next { get; set; }

        public short Length => (short)(To - From);

        public UEdge(int from, int to, UNode next)
        {
            From = (short)from;
            To = (short)to;
            Next = next;
        }
    }
}
