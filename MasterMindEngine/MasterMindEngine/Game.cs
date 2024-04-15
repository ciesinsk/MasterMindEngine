using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMindEngine
{
    public class Game
    {
        public const int MaxTurns = 10;

        public List<Placement> Turns = new List<Placement>();


        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Turn: {Turns.Count}");

            return sb.ToString();
        }
    }
}
