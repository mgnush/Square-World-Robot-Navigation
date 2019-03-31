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
                default:
                    agent = new DFSAgent(map);
                    break;
            }
            
            foreach(Node n in agent.TreeSearch())
            {
                Console.WriteLine(n.Move.ToString());
            }
        }
    }
}
