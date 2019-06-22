using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ai_ass1
{
    public class AllGoalsAgent : NavAgent
    {
        public AllGoalsAgent(GridMap map) : base(map)
        {

        }

        public List<Coords> GetRemainingGoals(Path path)
        {
            List<Coords> remainingGoals = new List<Coords>();

            foreach(Coords c in _map.GetGreenCells())
            {
                if (!path.VisitedGoal(c)) {
                    remainingGoals.Add(c);
                }
            }

            return remainingGoals;
        }

        /* Generate a path to each unvisited goal from current path
        * @param path The path to expand
        * @return The new paths
        */
        public List<Path> ExpandPath(Path path)
        {
            List<Path> expandedPath = new List<Path>();

            foreach (Coords c in GetRemainingGoals(path))
            {
                expandedPath.Add(TreeSearch(path, c));
            }

            return expandedPath;
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

            //Pick the closest goal
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

        /* Places the minimum node in the front of the frontier.
            * Does not sort the rest of the frontier
            * @param frontier The frontier of nodes to sort
            */
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

            //Console.WriteLine("{0},{1}, {2}, {3}, {4}", minFNode.Coords.x, minFNode.Coords.y, Heuristic(minFNode.Coords), GFunction(minFNode), minFNode.Move);
        }


        public override List<Node> Expand(Node node)
        {
            List<Node> expandedNode = new List<Node>();
            expandedNode.AddRange(_map.GetNodes(node));

            foreach (Node n in expandedNode)
            {
                n.F = n.Depth + Heuristic(n.Coords);
            }

            NodeCount += expandedNode.Count;   // Increment tree node count

            return expandedNode;
        }

        /* AS Tree-search with only one valid goal state
        * @param init The path preceding the path to find
        * @param goal The goal state coords
        * @return The path from init to goal
        */
        private Path TreeSearch(Path init, Coords goal)
        {
            List<Node> moves = new List<Node>();   // The path from starting point to goal
            List<Node> frontier = new List<Node>();
            Node node, initNode;

            // If no preceding path, assume starting point is agent start-location
            // Otherwise, start where preceding path ended
            if (init == null)
            {
                initNode = new Node(_redCoords, Move.NOOP, null);
            } else
            {
                initNode = new Node(init.Moves.Last().Coords, Move.NOOP, null);
            }

            bool reachedGoal = false;

            frontier.Add(initNode);   // Add initial state to frontier

            do
            {
                if (frontier.Count == 0) { return null; }

                // Remove first frontier item when it's a repeated state
                // Not needed for AS, but needed to prevent infinite loop when no solution
                while (frontier.First().IsRepeatedState())
                {
                    frontier.RemoveAt(0);
                    if (frontier.Count == 0) { return null; }
                    SortFrontier(frontier);
                }

                node = frontier.First();
                frontier.RemoveAt(0);

                if (goal.IsEqual(node.Coords))
                {
                    reachedGoal = true;
                }

                frontier.AddRange(Expand(node));

            } while (!reachedGoal);

            // Backtrack nodes from node that reached goal to initial node
            while (node != initNode)
            {
                moves.Add(node);
                node = node.ParentNode;
            }
            moves.Reverse();   // Reverse to print the moves starting from initial node
                        
            return new Path(moves, init);
        }

        /* Fully expand the tree of paths, then determine the shortest 
         * possible path from start thorugh all goals
         * @return The moves in the shortest possible path sequence
        */
        public override List<Node> TreeSearch()
        {
            List<Path> frontier = new List<Path>();
            List<Node> moves = new List<Node>();

            Path path;
            List<Path> paths = new List<Path>();

            // Start by creating a path from agent start locatoin to each goal
            foreach (Coords goal in _map.GetGreenCells())
            {
                Path initPath = TreeSearch(null, goal);
                if (initPath == null)
                {
                    return null;   // Give up if any goal is not accessible from start location               
                } 
                frontier.Add(initPath);            
            }

            // Build the tree of paths until every sequence of paths has reached every goal
            while (frontier.Count != 0)
            {
                path = frontier.First();
                frontier.RemoveAt(0);

                List<Path> newPaths = ExpandPath(path);

                //Expand until every path has reached every goal
                if (newPaths.Count == 0)
                {
                    paths.Add(path);
                }

                frontier.AddRange(newPaths);   // Add expanded nodes to the end (FIFO queue)
            }

            // Find shortest sequence of paths in tree using leaf nodes
            Path shortestPath = paths.First();   
            foreach (Path p in paths)
            {
                if (p.Length < shortestPath.Length)
                {
                    shortestPath = p;
                }
            }

            // Compile complete list of moves by backtracking from leaf in shortest path
            while (shortestPath != null)
            {
                moves.InsertRange(0, shortestPath.Moves);
                shortestPath = shortestPath.ParentPath;
            }

            return moves;
        }
    }
}
