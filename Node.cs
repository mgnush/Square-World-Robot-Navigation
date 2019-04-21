using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ai_ass1
{
    public class Node : IDisposable
    {
        private Coords _coords; // Current coords
        private Move _move;   // The move that got us here
        private Node _parentNode;   // The node which created this node from expansion
        private int _depth, _f;

        public Node(Coords coords0, Move move0, Node node0)
        {
            _move = move0;
            _parentNode = node0;
            _f = 0;

            // Increment depth if node has a parent node
            if (node0 == null)
            {
                _depth = 0;
            }
            else
            {
                _depth = node0.Depth + 1;
            }

            // Determine new coords from old coords and the selected move
            switch (_move)
            {
                case Move.UP:
                    _coords = new Coords(coords0.x, coords0.y - 1);
                    break;
                case Move.LEFT:
                    _coords = new Coords(coords0.x - 1, coords0.y);
                    break;
                case Move.DOWN:
                    _coords = new Coords(coords0.x, coords0.y + 1);
                    break;
                case Move.RIGHT:
                    _coords = new Coords(coords0.x + 1, coords0.y);
                    break;
                default:
                    _coords = coords0;
                    break;
            }
        }

        public Coords Coords { get => _coords; }
        public Move Move { get => _move; set => _move = value; }
        public Node ParentNode { get => _parentNode; }
        public int Depth { get => _depth; }
        public int F { get => _f; set => _f = value; }

        /* @return Whether the node has an ancestor with identical coords
        */
        public bool IsRepeatedState()
        {
            Node parentNode = _parentNode;
            while (parentNode != null)
            {
                if (parentNode.Coords.IsEqual(_coords)) { return true; }
                parentNode = parentNode.ParentNode;
            }
            return false;
        }

        /* Dispose of node in memory
        */
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
        }
    }
}
