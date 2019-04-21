using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ai_ass1
{   
    // This class stores a list of nodes that forms a path from 
    // A to B, while keeping tabs on the total length of the path,
    // based on the path preceding it
    public class Path
    {
        private List<Node> _moves;
        private int _length;
        private List<Coords> _goalsVisited;
        private Path _parentPath;

        public Path(List<Node> moves, Path parentPath)
        {
            _parentPath = parentPath;
            _moves = moves;
            _length = moves.Count;
            _goalsVisited = new List<Coords>();

            if (parentPath != null)
            {
                _goalsVisited.AddRange(parentPath.GoalsVisited);
                _length += parentPath.Length;
            }            
            _goalsVisited.Add(moves.Last().Coords);
        }

        public int Length { get => _length; }
        public List<Node> Moves { get => _moves; }
        public List<Coords> GoalsVisited { get => _goalsVisited; }
        public Path ParentPath { get => _parentPath; }

        public bool VisitedGoal(Coords goalCoords)
        {
            bool visitedGoal = false;

            foreach (Coords c in _goalsVisited)
            {
                if (c.IsEqual(goalCoords)) { visitedGoal = true; }
            }

            return visitedGoal;
        }

      
    }
}
