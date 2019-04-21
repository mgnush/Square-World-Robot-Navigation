using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ai_ass1
{   
    public abstract class NavAgent
    {
        protected GridMap _map;
        protected Coords _redCoords;   // Starting point
        private int _nodeCount;   // Amount of nodes created and kept in the search tree  

        public int NodeCount { get => _nodeCount; set => _nodeCount = value; }

        public NavAgent(GridMap map)
        {
            _map = map;
            _redCoords = _map.GetRedCell();
            _nodeCount = 0;
        }

        /* Expand a node by generating a new node for each possible move
         * @param node The node to expand
         * @return The list of nodes in the path the agent found from 
         * starting point to a goal
        */
        public abstract List<Node> TreeSearch();

        /* Expand a node by generating a new node for each possible move
         * @param node The node to expand
         * @return The list of nodes created from expanding the input node
        */
        public abstract List<Node> Expand(Node node);
    }
}

    
