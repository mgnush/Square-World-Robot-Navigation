using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ai_ass1
{
    public class BFSAgent : NavAgent
    {
        public BFSAgent(GridMap map) : base(map)
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
            List<Node> moves = new List<Node>();
            List<Node> frontier = new List<Node>();
            Node node = new Node(_redCoords, Move.NOOP, null);
            bool reachedGoal = false;

            frontier.Add(node);   // Add initial state to frontier

            do
            {
                if (frontier.Count == 0) { return null; }

                // Remove front nodes in the frontier LIFO queue until the first node is not a repeated state
                while (frontier.First().IsRepeatedState())
                {
                    frontier.RemoveAt(0);
                    if (frontier.Count == 0) { return null; }
                }
                
                node = frontier.First();
                frontier.RemoveAt(0);
                
                if (_map.IsInGreenCell(node.Coords))
                {
                    reachedGoal = true;
                }
                frontier.AddRange(Expand(node));   // Add expanded nodes to the end (LIFO queue)

            } while (!reachedGoal);

            // Backtrack nodes from node that reached goal to initial node
            while (node.ParentNode != null)
            {
                moves.Add(node);
                node = node.ParentNode;
            }
            moves.Reverse(); // Reverse to print the moves starting from initial node
            return moves;
        }
    }
}
