using System.Drawing;
using System.Text;

namespace MasterMindEngine
{
    public class Turn
    {
        public Placement Placement { get; private set; }

        public Hint Hint { get; set; } = new Hint(new HintColors[Placement.Size]);

        public int TurnNumber { get; private set; }

        public Turn(Placement p, int turnNumber) 
        {
            Placement = p;
            TurnNumber = turnNumber;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"Turn: {TurnNumber}: ");
            sb.Append($"{Placement.ToString()} - ");
            sb.AppendLine($"{Hint.ToString()}");
            return sb.ToString();
        }
    }
}
