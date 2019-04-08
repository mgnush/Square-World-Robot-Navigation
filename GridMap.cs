using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ai_ass1
{
    public enum CellStatus
    {
        WHITE,
        GREY,
        RED,
        GREEN
    }

    public static class MoveExtensions
    {
        public static Move Reverse(this Move move)
        {
            switch (move)
            {
                case Move.UP:
                    return Move.DOWN;
                    
                case Move.LEFT:
                    return Move.RIGHT;
                    
                case Move.DOWN:
                    return Move.UP;
                    
                case Move.RIGHT:
                    return Move.LEFT;
                    
                default:
                    return Move.NOOP;
                    
            }
        }
    }

    public enum Move
    {
        UP,
        LEFT,
        DOWN,
        RIGHT,
        NOOP
    }

    public struct Coords
    {
        public int x;
        public int y;

        public Coords(int x0, int y0) 
        {
            x = x0;
            y = y0;
        }

        public bool IsEqual(Coords otherCoords)
        {
            return ((x == otherCoords.x) && (y == otherCoords.y));
        }
    }

    public class GridMap
    {
        private CellStatus[,] _cells;
        private int _n, _m;   // cols, rows

        public GridMap(string[] lines)
        {
            // Map dimensions are in first line
            // Correct location formating is assumed (no out of bounds positions)
            string[] numbers = Regex.Split(lines[0], @"\D+");
            List<string> values = new List<string>();
            // Get map dimensions
            foreach (string n in numbers)
            {
                if (!string.IsNullOrWhiteSpace(n))
                {
                    values.Add(n);
                }
            }
            int.TryParse(values[0], out _m);
            int.TryParse(values[1], out _n);
            _cells = new CellStatus[_n, _m];
            values.Clear();
            Console.WriteLine("The map is {0}x{1}", _n, _m);

            // Get initial agent position (red cell)
            numbers = Regex.Split(lines[1], @"\D+");
            foreach (string n in numbers)
            {
                if (!string.IsNullOrWhiteSpace(n))
                {
                    values.Add(n);
                }
            }
            int.TryParse(values[0], out int agentPosX);
            int.TryParse(values[1], out int agentPosY);
            _cells[agentPosX, agentPosY] = CellStatus.RED;
            values.Clear();
            Console.WriteLine("The red cell is at {0},{1}", agentPosX, agentPosY);

            // Get goal cells (green cell)
            numbers = Regex.Split(lines[2], @"\D+");
            foreach (string n in numbers)
            {
                if (!string.IsNullOrWhiteSpace(n))
                {
                    values.Add(n);
                }
            }
            for (int i = 0; i < (values.Count - 1); i += 2)
            {
                int.TryParse(values[i], out int goalPosX);
                int.TryParse(values[i + 1], out int goalPosY);
                _cells[goalPosX, goalPosY] = CellStatus.GREEN;
                Console.WriteLine("There is a goal cell at {0},{1}", goalPosX, goalPosY);
            }
            values.Clear();

            // Get wall cells (grey cells)
            for (int i = 3; i < lines.Length; i++)
            {
                numbers = Regex.Split(lines[i], @"\D+");
                foreach (string n in numbers)
                {
                    if (!string.IsNullOrWhiteSpace(n))
                    {
                        values.Add(n);
                    }
                }
                int x0 = int.Parse(values[0]);
                int y0 = int.Parse(values[1]);
                for (int j = x0; j < (x0 + int.Parse(values[2])); j++)
                {
                    for (int k = y0; k < (y0 + int.Parse(values[3])); k++)
                    {
                        _cells[j, k] = CellStatus.GREY;
                        Console.WriteLine("{0},{1} IS GREY", j, k);
                    }
                }
                values.Clear();
            }
        }

        public CellStatus GetCellStatus(Coords coords)
        {
            return _cells[coords.x, coords.y];
        }

        public Coords GetRedCell() 
        {
            for (int i = 0; i < _n; i++) 
            {
                for (int j = 0; j < _m; j++)
                {
                    Coords c = new Coords(i, j);
                    if ((GetCellStatus(c) == CellStatus.RED))
                    {
                        return c;
                    }
                }
            }
            return new Coords(-1, -1);
        }

        public List<Coords> GetGreenCells() 
        {
            List<Coords> greenCells = new List<Coords>();
            for (int i = 0; i < _n; i++) 
            {
                for (int j = 0; j < _m; j++)
                {
                    Coords c = new Coords(i, j);
                    if (GetCellStatus(c) == CellStatus.GREEN)
                    {
                        greenCells.Add(c);
                    }
                }
            }
            return greenCells;
        }

        public bool IsInGreenCell(Coords coords)
        {
            List<Coords> greenCells = GetGreenCells();
            foreach (Coords c in greenCells)
            {
                if (c.IsEqual(coords)) { return true; }
            }
            return false;
        }

        public List<Move> GetMoves(Coords coords)
        {
            //Console.WriteLine("{0},{1}", coords.x, coords.y);
            List<Move> possibleMoves = new List<Move>();
            if (coords.y > 0)
            {
                if (_cells[coords.x, coords.y - 1] != CellStatus.GREY)
                {
                    possibleMoves.Add(Move.UP);
                }
            }
            if (coords.x > 0) {
                if (_cells[coords.x - 1, coords.y] != CellStatus.GREY)
                {
                    possibleMoves.Add(Move.LEFT);
                }
            }
            if (coords.y < (_m - 1))
            {
                if (_cells[coords.x, coords.y + 1] != CellStatus.GREY)
                {
                    possibleMoves.Add(Move.DOWN);
                }
            }
            if (coords.x < (_n - 1))
            {
                if (_cells[coords.x + 1, coords.y] != CellStatus.GREY)
                {
                    possibleMoves.Add(Move.RIGHT);
                }
            }
            return possibleMoves;
        }

        public List<Node> GetNodes(Node node)
        {
            List<Node> newNodes = new List<Node>();
            foreach(Move m in GetMoves(node.Coords))
            {
                newNodes.Add(new Node(node.Coords, m, node));
            }
            return newNodes;
        }
    }
}
