using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ai_ass1
{
    public class ASAgent : NavAgent
    {
        public ASAgent(GridMap map) : base(map)
        {

        }

        private int GFunction(Node node)
        {
            int depth = 0;
            Node parentNode = node.ParentNode;

            while (parentNode != null)
            {
                depth++;
                parentNode = parentNode.ParentNode;
            }

            return depth;
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

            foreach (Coords c in greenCells)
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
            int g = GFunction(minFNode);
            int manhattan = Heuristic(minFNode.Coords);
            int fMin = g + manhattan;
            int f;

            foreach (Node n in frontier)
            {
                manhattan = Heuristic(n.Coords);
                g = GFunction(n);
                f = manhattan + g;

                if (f < fMin)
                {
                    minFNode = n;
                    fMin = f;
                }
                else if (f == fMin)
                {
                    if (n.Move < minFNode.Move)
                    {
                        minFNode = n;
                    }
                }
            }

            frontier.Remove(minFNode);
            frontier.Insert(0, minFNode);

            //Console.WriteLine("{0},{1}, {2}, {3}, {4}", minFNode.Coords.x, minFNode.Coords.y, Heuristic(minFNode.Coords), GFunction(minFNode), minFNode.Move);
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
