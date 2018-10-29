
namespace DataStructures
{
    public class ActivePoint
    {
        private readonly UNode _activeUNode;
        private readonly byte _activeEdgeSymbol;
        private readonly int _activeLength;

        private UEdge ActiveEdge => _activeUNode.Edges[_activeEdgeSymbol];

        public ActivePoint(UNode activeUNode, byte activeEdgeSymbol, int activeLength)
        {
            _activeUNode = activeUNode;
            _activeEdgeSymbol = activeEdgeSymbol;
            _activeLength = activeLength;
        }

        public bool ActiveNodeHasEdgeFor(byte symbol)
        {
            return _activeUNode.Edges[symbol] != null;
        }

        public void AddEdgeToActiveNode(byte symbol, UEdge edge)
        {
            _activeUNode.Edges[symbol] = edge;
        }

        public bool HasASuffixLink()
        {
            return _activeUNode.SuffixLink != null;
        }

        public bool IsThisTheActiveNode(UNode uNode)
        {
            return _activeUNode == uNode;
        }

        public ActivePoint MoveToEdgeStartingWith(byte symbol)
        {
            return new ActivePoint(_activeUNode, symbol, 1);
        }

        public ActivePoint MoveToNextNodeOfActiveEdge()
        {
            return new ActivePoint(ActiveEdge.Next, 0, 0);
        }

        public ActivePoint MoveToSuffixLink()
        {
            return new ActivePoint(_activeUNode.SuffixLink, _activeEdgeSymbol, _activeLength);
        }

        public ActivePoint MoveTo(UNode uNode)
        {
            return new ActivePoint(uNode, _activeEdgeSymbol, _activeLength);
        }

        public ActivePoint MoveOneSymbol()
        {
            return new ActivePoint(_activeUNode, _activeEdgeSymbol, _activeLength + 1);
        }

        public ActivePoint MoveToEdgeStartingWithAndByActiveLengthLessOne(UNode uNode, byte character)
        {
            return new ActivePoint(uNode, character, _activeLength - 1);
        }

        public ActivePoint MoveToNextNodeOfActiveEdge(byte[] store, int index)
        {
            return new ActivePoint(ActiveEdge.Next, store[(index - _activeLength + ActiveEdge.Length)], _activeLength - ActiveEdge.Length);
        }

        public bool PointsAfterTheEndOfActiveEdge()
        {
            return ActiveEdge.Length < _activeLength;
        }

        public bool PointsToActiveEdge(byte[] store, byte symbol)
        {
            return store[ActiveEdge.From + _activeLength] == symbol;
        }

        public bool PointsToActiveNode()
        {
            return _activeLength == 0;
        }

        public bool PointsToTheEndOfActiveEdge()
        {
            return ActiveEdge.Length == _activeLength;
        }

        public void SetSuffixLinkTo(UNode uNodeWhoseSuffixLinkToSet, UNode uNode)
        {
            if (uNodeWhoseSuffixLinkToSet != null)
            {
                uNodeWhoseSuffixLinkToSet.SuffixLink = uNode;
            }
        }

        public UNode SetSuffixLinkToActiveNodeAndReturnActiveNode(UNode uNode)
        {
            SetSuffixLinkTo(uNode, _activeUNode);
            return _activeUNode;
        }

        public void SplitActiveEdge(byte[] store, UNode uNodeToAdd, int index, byte symbol)
        {
            UEdge activeEdge = ActiveEdge;
            UEdge splitEdge = new UEdge(activeEdge.From, activeEdge.From + _activeLength, uNodeToAdd);

            uNodeToAdd.Edges[store[activeEdge.From + _activeLength]] =
                new UEdge(activeEdge.From + _activeLength, activeEdge.To, activeEdge.Next);

            uNodeToAdd.Edges[symbol] = new UEdge(index, store.Length, null);
            _activeUNode.Edges[_activeEdgeSymbol] = splitEdge;
        }
    }
}
