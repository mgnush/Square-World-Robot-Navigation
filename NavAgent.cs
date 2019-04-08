using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ai_ass1
{
    public class Node
    {
        private Coords _coords; // Current coords
        private Move _move;   // The move that got us here
        private Node _parentNode;

        public Node(Coords coords0, Move move0, Node node0)
        {
            _move = move0;
            _parentNode = node0;

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
    }

    public abstract class NavAgent
    {
        protected GridMap _map;
        protected Coords _redCoords;

        public NavAgent(GridMap map)
        {
            _map = map;
            _redCoords = _map.GetRedCell();
        }

        public abstract List<Node> TreeSearch();
        public abstract List<Node> Expand(Node node);
    }
}

    
