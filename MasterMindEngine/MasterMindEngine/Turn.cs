using System.Text;
using static MasterMindEngine.GameConfig;

namespace MasterMindEngine
{
    /// <summary>
    /// Represents a turn in the game, which consists mainly of a placement and a hint
    /// </summary>
    public class Turn
    {
        public Placement Placement { get; private set; }

        public Hint Hint { get; set; } = new Hint(new HintColors[CodeLength]);

        public int TurnNumber { get; private set; }

        public Turn(Placement p, int turnNumber) 
        {
            Placement = p;
            TurnNumber = turnNumber;
        }

        public Turn(Placement p, Hint hint,  int turnNumber) 
        {
            Placement = p;
            Hint = hint;
            TurnNumber = turnNumber;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"Turn {TurnNumber}     : ");
            sb.Append($"{Placement.ToString()}");
            sb.Append($" - {Hint.ToString()}");
            return sb.ToString();
        }
    }
}
