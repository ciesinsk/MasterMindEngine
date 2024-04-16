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

        public List<Turn> Turns = new List<Turn>();




        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var t in Turns)
            {
                sb.AppendLine(t.ToString());
            }

            return sb.ToString();
        }
    }
}
