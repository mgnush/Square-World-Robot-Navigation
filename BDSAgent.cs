using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ai_ass1
{
    public class BDSAgent : NavAgent
    {
        public BDSAgent(GridMap map) : base(map)
        {

        }

        public override List<Node> Expand(Node node)
        {
            List<Node> expandedNode = new List<Node>();
            expandedNode.AddRange(_map.GetNodes(node));

            return expandedNode;
        }

        public override List<Node> TreeSearch()
        {
            List<Node> moves = new List<Node>();
            List<Node> frontierForward = new List<Node>();
            List<List<Node>> frontiersBackward = new List<List<Node>>();
            Node nodeForward = new Node(_redCoords, Move.NOOP, null);
            Node nodeBackward;
            Node touchNode = null;
            frontierForward.Add(nodeForward);   // Add initial state to frontier

            foreach (Coords c in _map.GetGreenCells())
            {
                List<Node> frontierBackward = new List<Node>();
                nodeBackward = new Node(c, Move.NOOP, null);
                frontierBackward.Add(nodeBackward);
                frontiersBackward.Add(frontierBackward);
            }
            
            bool touch = false;

            do
            {
                if (frontierForward.Count == 0) { return null; }
                if (frontiersBackward.Count == 0) { return null; }
                List<List<Node>> deadEnds = new List<List<Node>>();
                // Remove backward frontiers that are dead ends, i.e. the goal is enclosed by walls
                foreach(List<Node> ln in frontiersBackward)
                {
                    if (ln.Count == 0) { deadEnds.Add(ln); }
                }
                foreach(List<Node> ln in deadEnds)
                {
                    frontiersBackward.Remove(ln);
                }
                deadEnds.Clear();

                while (frontierForward.First().IsRepeatedState())
                {
                    frontierForward.RemoveAt(0);
                    if (frontierForward.Count == 0) { return null; }
                }

                foreach (List<Node> ln in frontiersBackward)
                {
                    while (ln.First().IsRepeatedState())
                    {
                        ln.RemoveAt(0);
                        if (ln.Count == 0)
                        {
                            deadEnds.Add(ln);
                            break;
                        }
                    }
                }
                foreach (List<Node> ln in deadEnds)
                {
                    frontiersBackward.Remove(ln);
                }

                nodeForward = frontierForward.First();
                frontierForward.RemoveAt(0);
                frontierForward.AddRange(Expand(nodeForward));

                foreach (List<Node> ln in frontiersBackward)
                {
                    nodeBackward = ln.First();
                    ln.RemoveAt(0);
                    if (nodeForward.Coords.IsEqual(nodeBackward.Coords))
                    {
                        touch = true;
                        touchNode = nodeBackward;
                        break;
                    }                    
                    ln.AddRange(Expand(nodeBackward));
                } 

            } while (!touch);

            while (nodeForward.ParentNode != null)
            {
                moves.Add(nodeForward);
                nodeForward = nodeForward.ParentNode;
            }
            moves.Reverse();
            while (touchNode.ParentNode != null)
            {
                touchNode.Move = touchNode.Move.Reverse();
                moves.Add(touchNode);
                touchNode = touchNode.ParentNode;
            }
            return moves;
        }
    }
}
