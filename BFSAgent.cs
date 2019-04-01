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
                while (frontier.First().IsRepeatedState())
                {
                    frontier.RemoveAt(0);
                }
                node = frontier.First();
                frontier.RemoveAt(0);
                
                if (_map.IsInGreenCell(node.Coords))
                {
                    reachedGoal = true;
                }
                frontier.AddRange(Expand(node));

            } while (!reachedGoal);

            while (node.ParentNode != null)
            {
                moves.Add(node);
                node = node.ParentNode;
            }
            moves.Reverse();
            return moves;
        }
    }
}
