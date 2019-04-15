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
                default:
                    agent = new DFSAgent(map);
                    break;
            }

            if (agent.TreeSearch() == null)
            {
                Console.WriteLine("There is no path to the goal.");
            }
            else
            {
                foreach (Node n in agent.TreeSearch())
                {
                    Console.WriteLine(n.Move.ToString());
                }
            }
        }
    }
}
