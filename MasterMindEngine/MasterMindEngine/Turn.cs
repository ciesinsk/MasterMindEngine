using System.Text;

namespace MasterMindEngine
{
    public class Turn
    {
        public Placement Placement { get; private set; }

        public Hint Hint { get; private set; }

        public int TurnNumber { get; private set; }

        public Turn() { }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Turn: {TurnNumber}");
            sb.AppendLine(Placement.ToString());
            sb.AppendLine(Hint.ToString());
            return sb.ToString();
        }
    }
}
