using System.Text;

namespace MasterMindEngine
{
    public class Hint
    {                
        public Hint Clone()
        {
            var newHints = new HintColors[Placement.Size];
            for (int i = 0; i < Placement.Size; i++)
            {
                newHints[i] = Hints[i];
            }

            return new Hint(newHints);
        }


        public Hint(HintColors[] hints)
        {
            Hints = hints;
        }

        public HintColors[] Hints { get; private set; } = new HintColors[Placement.Size];

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

            var p = new Hint(new HintColors[Placement.Size]);

            var colors = v.Split(',').Select(s=>Enum.Parse(typeof(HintColors), s)).OfType<HintColors>().ToArray();

            if(colors.Length != Placement.Size)
            {
                throw new ArgumentException("The placement should have exactly 4 Hints");
            }

            p.Hints = colors;

            return p;
        }
    }
}