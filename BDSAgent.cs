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
            List<Node> frontierBackward = new List<Node>();
            Node nodeForward = new Node(_redCoords, Move.NOOP, null);
            Node nodeBackward = new Node(_redCoords, Move.NOOP, null);
            bool touch = false;

            frontierForward.Add(nodeForward);   // Add initial state to frontier
            frontierBackward.Add(nodeBackward);

            do
            {
                if ((frontierForward.Count == 0) || (frontierBackward.Count == 0)) { return null; }
                while (frontierForward.First().IsRepeatedState())
                {
                    frontierForward.RemoveAt(0);
                }
                while (frontierBackward.First().IsRepeatedState())
                {
                    frontierForward.RemoveAt(0);
                }

                nodeForward = frontierForward.First();
                nodeBackward = frontierBackward.First();
                frontierForward.RemoveAt(0);
                frontierBackward.RemoveAt(0);

                if (nodeForward.Coords.IsEqual(nodeBackward.Coords))
                {
                    touch = true;
                }
                frontierForward.AddRange(Expand(nodeForward));
                frontierBackward.AddRange(Expand(nodeBackward));

            } while (!touch);

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
