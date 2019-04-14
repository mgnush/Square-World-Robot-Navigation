using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ai_ass1
{
    public class GBFSAgent : NavAgent
    {
        public GBFSAgent(GridMap map) : base(map)
        {

        }

        /* Get the manhattan distance to the closest goal
         * @param nodeCoords The coords of the node in question
         * @return The distance
         */
        private int Heuristic(Coords nodeCoords)
        {
            List<Coords> greenCells = _map.GetGreenCells();
            int manhattanDist = Math.Abs(nodeCoords.x - greenCells.First().x) + Math.Abs(nodeCoords.y - greenCells.First().y);
            int dist;

            foreach (Coords c in _map.GetGreenCells())
            {
                dist = Math.Abs(nodeCoords.x - c.x) + Math.Abs(nodeCoords.y - c.y);
                if (dist < manhattanDist)
                {
                    manhattanDist = dist;
                }
            }

            return manhattanDist;
        }

        private void SortFrontier(List<Node> frontier)
        {
            // No need to sort entire frontier
            Node minFNode = frontier.First();

            foreach (Node n in frontier)
            {
                if (n.F < minFNode.F)
                {
                    minFNode = n;
                }
                else if (n.F == minFNode.F)
                {
                    if (n.Move < minFNode.Move)
                    {
                        minFNode = n;
                    }
                }
            }
            // Move min node to front
            frontier.Remove(minFNode);
            frontier.Insert(0, minFNode);
        }

        public override List<Node> Expand(Node node)
        {
            List<Node> expandedNode = new List<Node>();
            expandedNode.AddRange(_map.GetNodes(node));

            List<Node> repeatedNodes = new List<Node>();
            foreach (Node n in expandedNode)
            {
                if (n.IsRepeatedState()) { repeatedNodes.Add(n); }
            }
            foreach(Node n in repeatedNodes)
            {
                expandedNode.Remove(n);
            }

            foreach(Node n in expandedNode)
            {
                n.F = Heuristic(n.Coords);
            }

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
                //sort
                SortFrontier(frontier);

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
