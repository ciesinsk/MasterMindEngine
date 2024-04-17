using System.Text;
using static MasterMindEngine.GameConfig;

namespace MasterMindEngine
{
    public class Hint
    {                
        public Hint Clone()
        {
            var newHints = new HintColors[CodeLength];
            for (int i = 0; i < CodeLength; i++)
            {
                newHints[i] = Hints[i];
            }

            return new Hint(newHints);
        }


        public Hint(HintColors[] hints)
        {
            Hints = hints;
        }

        public HintColors[] Hints { get; private set; } = new HintColors[CodeLength];

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach(var h in Hints)
            {
                sb.Append(h + " ");                    
            }            

            return sb.ToString();
        }

        internal static Hint? Parse(string? v)
        {
            if(v == null)
            {
                throw new ArgumentException("Invalid input line");;
            }

            var p = new Hint(new HintColors[CodeLength]);

            var colors = v.Split(',').Select(s=>Enum.Parse(typeof(HintColors), s)).OfType<HintColors>().ToArray();

            if(colors.Length != CodeLength)
            {
                throw new ArgumentException("The placement should have exactly 4 Hints");
            }

            p.Hints = colors;

            return p;
        }
    }
}