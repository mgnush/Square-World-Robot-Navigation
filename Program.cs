using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ai_ass1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines(args[0]);
            GridMap map = new GridMap(lines);
            NavAgent agent;
            switch(args[1])
            {
                case "DFS":
                    agent = new DFSAgent(map);
                    break;
                case "BFS":
                    agent = new BFSAgent(map);
                    break;
                case "GBFS":
                    agent = new GBFSAgent(map);
                    break;
                case "AS":
                    agent = new ASAgent(map);
                    break;
                case "BDS":
                    agent = new BDSAgent(map);
                    break;
                case "RBFS":
                    agent = new RBFSAgent(map);
                    break;
                case "ASAG":
                    agent = new AllGoalsAgent(map);
                    break;
                default:
                    agent = new DFSAgent(map);
                    Console.WriteLine("{0} is not a search method", args[1]);
                    return;
            }

            List<Node> moves = agent.TreeSearch();

            if (moves == null)
            {
                int nodes = agent.NodeCount;
                Console.WriteLine("{0} {1} {2} \n", args[0], args[1], nodes);
                Console.WriteLine("There is no path to the goal.");
            }
            else
            {
                int nodes = agent.NodeCount;
                Console.WriteLine("{0} {1} {2} \n", args[0], args[1], nodes);
                foreach (Node n in moves)
                {
                    Console.WriteLine(n.Move.ToString());
                }
            }
        }
    }
}
