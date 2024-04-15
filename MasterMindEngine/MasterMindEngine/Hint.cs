using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}