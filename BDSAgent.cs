using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ai_ass1
{
    // This class implements a bidirectional search, with BFS in both ends.
    public class BDSAgent : NavAgent
    {
        public BDSAgent(GridMap map) : base(map)
        {

        }

        public override List<Node> Expand(Node node)
        {
            List<Node> expandedNode = new List<Node>();
            expandedNode.AddRange(_map.GetNodes(node));

            NodeCount += expandedNode.Count;   // Increment tree node count

            return expandedNode;
        }

        public override List<Node> TreeSearch()
        {
            List<Node> moves = new List<Node>();   // The path from starting point to goal
            List<Node> frontierForward = new List<Node>();
            List<List<Node>> frontiersBackward = new List<List<Node>>();   // Need a backward frontier for each goal
            Node nodeForward = new Node(_redCoords, Move.NOOP, null);
            Node nodeBackward;
            Node touchNode = null;   // The node at first collision
            frontierForward.Add(nodeForward);   // Add initial state to frontier

            // Populate backward nordes
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
                List<List<Node>> deadEnds = new List<List<Node>>();   // List of backward frontiers with trapped goals (blocked by grey cells)

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

                // Remove front nodes in the frontier LIFO queues until the first node is not a repeated state
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
                frontierForward.AddRange(Expand(nodeForward));   // Add expanded nodes to the end (LIFO queue)

                // Determine if the forward node touches any of the backward nodes
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
                    ln.AddRange(Expand(nodeBackward));   // Expand node in backward frontiers
                } 

            } while (!touch);

            // Complete the path by backtracking from forward node to init state,
            // then add nodes from backwards node to goal
            while (nodeForward.ParentNode != null)
            {
                moves.Add(nodeForward);
                nodeForward = nodeForward.ParentNode;
            }
            moves.Reverse();
            while (touchNode.ParentNode != null)
            {
                touchNode.Move = touchNode.Move.Reverse();   // Reverse the move, i.e. right = left
                moves.Add(touchNode);
                touchNode = touchNode.ParentNode;
            }
            return moves;
        }
    }
}
