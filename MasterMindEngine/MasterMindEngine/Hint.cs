using System.Text;
using static MasterMindEngine.GameConfig;

namespace MasterMindEngine
{
    public class Hint : IEquatable<Hint>
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

            if(colors.Length > CodeLength)
            {
                throw new ArgumentException("Too many hints in the input line");
            }   

            p.Hints = Enumerable.Repeat(HintColors.None, CodeLength).ToArray();

            for (int i = 0; i < colors.Length; i++)
            {
                p.Hints[i] = colors[i];
            }

            return p;
        }

        internal static Hint CreateEmpty()
        {            
            var hints = new HintColors[CodeLength];

            for(var i = 0;i< CodeLength;++i)
            {
                 hints[i] = HintColors.None;
            }                  

            var hint = new  Hint(hints);
            return hint;
        }

         /// <summary>
        /// Standard override of the Equals method
        /// </summary>
        public bool Equals(Hint? other)
        {
            if(other == null)
            {
                return false;
            }   

            var hintList = Hints.OrderBy(h => h).ToList(); 
            var otherHintList = other.Hints.OrderBy(h => h).ToList();

            for (int i = 0; i < CodeLength; i++)
            {
                if(otherHintList[i] != hintList[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Standard override of the Equals method
        /// </summary>
        public override bool Equals(object? other)
        {
            var otherHint = other as Hint;
            return Equals(otherHint);
        }

        /// <summary>
        /// Standard override of the GetHashCode method
        /// </summary>
        public override int GetHashCode()
        {
            if (Hints == null) return 0;

            var hintList = Hints.OrderBy(h => h).ToList(); 

            unchecked
            {
                int hash = 17;
                for(int i = 0; i < hintList.Count(); i++)
                    hash = 31 * hash + hintList[i].GetHashCode();
                return hash;
            }
        }
    }
}