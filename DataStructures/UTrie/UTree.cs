using System;

namespace DataStructures
{
    /*
     * Note that some functions are named for the *state* in which they are called, not for the actions they perform.
     * In this case, it made the code easier to understand while I wrote it, but it's unusual and should perhaps
     * be refactored so the name describes the action it takes, not the circumstances under which it's called.
     * */

    public class UTree
    {
        private readonly byte[] _store;
        private readonly UNode _root;
        private ActivePoint _activePoint;
        private int _remainder;

        public int InsertionPoint { get; private set; }
        public long AbsoluteStartTime { get; set; }

        public UTree(string store) : this(Util.StringToBytes(store)) { }

        public UTree(int length) : this(new byte[length], 0) { }

        public UTree(byte[] store) : this(store, store.Length) { }

        public UTree(byte[] store, int count)
        {
            _store = store;
            _root = new UNode();
            _activePoint = new ActivePoint(_root, 0, 0);
            ConstructFromInitialStoreValues(count);
        }

        private void ConstructFromInitialStoreValues(int count)
        {
            for (int i = 0; i < _store.Length && i < count; i++)
            {
                Add(i, _store[InsertionPoint]);
            }
        }

        public void Add(byte[] source, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Add(source[i]);
            }
        }

        public void Add(char symbol)
        {
            Add((byte)symbol);
        }

        public void Add(byte symbol)
        {
            _store[InsertionPoint] = symbol;
            Add(InsertionPoint, symbol);
        }

        private void Add(int index, byte symbol)
        {
            bool symbolFound = false;
            UNode lastAddedUNode = null;
            _remainder++;

            while (!symbolFound && _remainder > 0)
            {
                if (_activePoint.PointsToActiveNode())
                {
                    if (_activePoint.ActiveNodeHasEdgeFor(symbol))
                    {
                        ActiveNodeHasEdgeForSymbol(symbol, lastAddedUNode);
                        symbolFound = true;
                    }
                    else
                    {
                        if (_activePoint.IsThisTheActiveNode(_root))
                        {
                            RootHasNoEdgeForSymbol(index, symbol);
                        }
                        else
                        {
                            lastAddedUNode = InternalWithoutEdgeForSymbol(index, symbol, lastAddedUNode);
                        }
                        _remainder--;
                    }
                }
                else
                {
                    if (_activePoint.PointsToActiveEdge(_store, symbol))
                    {
                        ActiveEdgeWithSymbol();
                        symbolFound = true;
                    }
                    else
                    {
                        lastAddedUNode = _activePoint.IsThisTheActiveNode(_root) 
                            ? EdgeFromRootWithoutSymbol(index, symbol, lastAddedUNode) 
                            : EdgeFromInternalWithoutSymbol(index, symbol, lastAddedUNode);
                        _remainder--;
                    }
                }
            }
            InsertionPoint++;
        }

        private void ActiveEdgeWithSymbol()
        {
            _activePoint = _activePoint.MoveOneSymbol();
            if (_activePoint.PointsToTheEndOfActiveEdge())
            {
                _activePoint = _activePoint.MoveToNextNodeOfActiveEdge();
            }
        }

        private void ActiveNodeHasEdgeForSymbol(byte symbol, UNode uNode)
        {
            _activePoint.SetSuffixLinkToActiveNodeAndReturnActiveNode(uNode);
            _activePoint = _activePoint.MoveToEdgeStartingWith(symbol);
            if (_activePoint.PointsToTheEndOfActiveEdge())
            {
                _activePoint = _activePoint.MoveToNextNodeOfActiveEdge();
            }
        }

        private UNode EdgeFromInternalWithoutSymbol(int index, byte symbol, UNode oldUNode)
        {
            UNode newUNode = new UNode();
            _activePoint.SplitActiveEdge(_store, newUNode, index, symbol);
            _activePoint.SetSuffixLinkTo(oldUNode, newUNode);
            _activePoint = _activePoint.HasASuffixLink()
                ? _activePoint.MoveToSuffixLink()
                : _activePoint.MoveTo(_root);
            _activePoint = WalkDownTree(index);
            return newUNode;
        }

        private UNode EdgeFromRootWithoutSymbol(int index, byte symbol, UNode oldUNode)
        {
            UNode newUNode = new UNode();
            _activePoint.SplitActiveEdge(_store, newUNode, index, symbol);
            _activePoint.SetSuffixLinkTo(oldUNode, newUNode);
            _activePoint = _activePoint.MoveToEdgeStartingWithAndByActiveLengthLessOne(_root, _store[index - _remainder + 2]);
            _activePoint = WalkDownTree(index);
            return newUNode;
        }

        private UNode InternalWithoutEdgeForSymbol(int index, byte symbol, UNode uNode)
        {
            _activePoint.AddEdgeToActiveNode(symbol, new UEdge(index, _store.Length, null));
            UNode activeUNode = _activePoint.SetSuffixLinkToActiveNodeAndReturnActiveNode(uNode);
            _activePoint = _activePoint.HasASuffixLink()
                ? _activePoint.MoveToSuffixLink()
                : _activePoint.MoveTo(_root);
            return activeUNode;
        }

        private void RootHasNoEdgeForSymbol(int index, byte symbol)
        {
            _activePoint.AddEdgeToActiveNode(symbol, new UEdge(index, _store.Length, null));
            _activePoint = _activePoint.MoveTo(_root);
        }

        private ActivePoint WalkDownTree(int index)
        {
            while (!_activePoint.PointsToActiveNode() && (_activePoint.PointsAfterTheEndOfActiveEdge() || _activePoint.PointsToTheEndOfActiveEdge()))
            {
                _activePoint = _activePoint.PointsAfterTheEndOfActiveEdge()
                    ? _activePoint.MoveToNextNodeOfActiveEdge(_store, index)
                    : _activePoint.MoveToNextNodeOfActiveEdge();
            }
            return _activePoint;
        }

        public bool Contains(string suffix)
        {
            return Contains(Util.StringToBytes(suffix));
        }

        public bool Contains(byte[] suffix)
        {
            int lengthOfMatch = Match(suffix, suffix.Length, out int _);
            return lengthOfMatch >= suffix.Length;
        }

        public int Match(byte[] suffix, int start, int searchLimit, out int startingIndex)
        {
            byte[] section = new byte[searchLimit];
            Array.Copy(suffix, start, section, 0, searchLimit);  // TODO eliminate copy to optimize
            return Match(section, searchLimit, out startingIndex);
        }

        // searchLimit = how much of suffix to match, maximally
        public int Match(byte[] suffix, int searchLimit, out int startingIndex)
        {
            UNode currentUNode = _root;
            UEdge currentEdge = null;

            for (int i = 0; i < suffix.Length && i < searchLimit; i++)
            {
                byte matchMe = suffix[i];
                UEdge nextEdge;

                nextEdge = currentUNode.Edges[matchMe];
                if (nextEdge == null)
                {
                    if (currentEdge == null)
                    {
                        // in this case, we're at the root uNode. no match is made. return -1.
                        startingIndex = -1;
                        return -1;
                    }
                    startingIndex = currentEdge.From - (i - 1);
                    return i - 1;  // matched every letter except this one.
                }
                currentEdge = nextEdge;
                int j;
                int jLimit = Math.Min(currentEdge.Length, suffix.Length - i);
                for (j = 0; j < jLimit; j++)
                {
                    if (_store[currentEdge.From + j] != suffix[i + j])
                    {
                        if (i + j < suffix.Length)
                        {
                            // we only got part way through this edge. include what we have and end here.
                            startingIndex = currentEdge.From - i;
                            return i + j;
                        }
                        break;
                    }
                }
                // j is the amount of the suffix the edge represents
                i += j - 1;

                if (i == suffix.Length - 1)
                {
                    // matched whole string
                    startingIndex = currentEdge.From - (i - (j - 1));
                    return suffix.Length;
                }
                if (currentEdge.Next == null)
                {
                    // end of the line. return what we have.
                    startingIndex = currentEdge.From - (i - (j - 1));
                    return i;
                }
                currentUNode = currentEdge.Next;
            }
            startingIndex = -1;
            return -1;  // no match at all? we might not get here actually.
        }
    }
}
